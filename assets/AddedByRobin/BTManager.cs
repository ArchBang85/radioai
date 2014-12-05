using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.IO.Ports;
#endif

using System;

public class BTManager : MonoBehaviour
{

    public TextMesh text;
    int prevTouchCount = 0;

    bool btActive = false;

    bool serialActive = false;

    public static BTManager instance;

    public int[] digitalPinValues = new int[7];
    public int[] analogPinValues = new int[3];

    bool reconnect = false;

#if UNITY_EDITOR
    SerialPort stream = new SerialPort("\\COM6", 115200); //Set the port (com4) and the baud rate (9600, is standard on most devices)

#elif UNITY_ANDROID

#else
#endif

    void Start()
    {
        text.text = "starting manager ...";
        instance = this;
#if UNITY_EDITOR

        string[] ports = System.IO.Ports.SerialPort.GetPortNames();
        Debug.Log("" + String.Join("\n", ports));
        //stream.PortName = ports[ports.Length - 1];
        stream.ReadTimeout = 1;
        stream.WriteTimeout = 1;
        try
        {
            stream.Open(); //Open the Serial Stream.
        }
        catch (Exception e)
        {
            serialActive = false;
        }
        serialActive = stream.IsOpen;
        Debug.Log("Serial Active: " + serialActive);
        analogPinValues[0] = 2030;
#elif UNITY_ANDROID
        btActive = true;

        Screen.SetResolution(1280, 800, true);
        
        // Disable screen dimming
	    Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Connect();

#endif


    }

    void Connect()
    {
        BtConnector.moduleName("HC-06");

       // if (!BtConnector.isBluetoothEnabled())
       //     BtConnector.askEnableBluetooth();

        int result = BtConnector.connect();

        text.text = "reconnecting: " + result;
        if (result > 0)
        {
            Debug.Log("BT Connected!");
            text.text = "connected.";
        }

        reconnect = false;
    }


    void FixedUpdate()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0 )
        {
            text.active = !text.active;
        }
#endif

        if (!serialActive && !btActive)
        {
            analogPinValues[0] = 2030;
            //analogPinValues[2] = 2030;
            // use keyboard input
            if (Input.GetKey(KeyCode.LeftArrow)) analogPinValues[0] -= 2000;
            if (Input.GetKey(KeyCode.RightArrow)) analogPinValues[0] += 2000;
            if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.S)) analogPinValues[2] -= 100;
            if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.W)) analogPinValues[2] += 100;
            if (Input.GetKey(KeyCode.UpArrow)) analogPinValues[1] += 100;
            if (Input.GetKey(KeyCode.DownArrow)) analogPinValues[1] -= 100;

            for (int i = 0; i < digitalPinValues.Length; i++)
                digitalPinValues[i] = 0;

            /*if (Input.GetKey(KeyCode.Alpha1))
            {
                Zeppelin.instance.SetCameraPos(0);
                digitalPinValues[3] = 1;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                Zeppelin.instance.SetCameraPos(1);
                digitalPinValues[4] = 1;
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                Zeppelin.instance.SetCameraPos(2);
                digitalPinValues[3] = 1;
                digitalPinValues[4] = 1;
            }*/
            if (Input.GetKey(KeyCode.Alpha0)) digitalPinValues[0] = 1;
            if (Input.GetKey(KeyCode.Alpha1)) digitalPinValues[1] = 1;
            if (Input.GetKey(KeyCode.Alpha2)) digitalPinValues[2] = 1;
            if (Input.GetKey(KeyCode.Alpha3)) digitalPinValues[3] = 1;
            if (Input.GetKey(KeyCode.Alpha4)) digitalPinValues[4] = 1;
            if (Input.GetKey(KeyCode.Alpha5)) digitalPinValues[5] = 1;
            if (Input.GetKey(KeyCode.Alpha6)) digitalPinValues[6] = 1;
            //if (Input.GetKey(KeyCode.Alpha7)) digitalPinValues[7] = 1;


            analogPinValues[0] = Mathf.Clamp(analogPinValues[0], 0, 4096);
            analogPinValues[1] = Mathf.Clamp(analogPinValues[1], 0, 4096);
            analogPinValues[2] = Mathf.Clamp(analogPinValues[2], 0, 4096);
            //analogPinValues[1] = 0;
            //if (Input.GetKey(KeyCode.DownArrow)) analogPinValues[1] = 3000;
        }
        BTManager.instance.Read();
    }

    void OnApplicationQuit()
    {

#if UNITY_EDITOR
        stream.Close();
        Debug.Log("Closing stream.");
#elif UNITY_ANDROID
        byte[] message = new byte[1];
        message[0] = 220; // shutdown signal
        BtConnector.sendBytes(message);
        BtConnector.close();

#endif
    }

    public void MoveDialTowards(int degrees)
    {
        SendByte((byte)Mathf.Clamp(degrees, 0, 180));
    }

    public void SendShake()
    {
        SendByte(201);
    }

    public void SendLightning()
    {
        SendByte(200);
    }

    private void SendByte(byte number)
    {

        byte[] message = new byte[1];
        message[0] = number;
#if UNITY_EDITOR
        //return;
        if (stream.IsOpen)
        {
            try
            {
                if (stream.IsOpen)
                {
                    stream.Write(message, 0, 1);
                }

            }
            catch (Exception e)
            {

            }
        }
#elif UNITY_ANDROID
        if (BtConnector.isConnected() )
            BtConnector.sendBytes(message);

#endif

    }

    private int Linearize(int input, int lowEnd, float lowEndPos)
    {
        if (input <= lowEnd)
            return (int)Mathf.Lerp(0, 4096 * lowEndPos, (float)input / (float)lowEnd);
        return (int)Mathf.Lerp(4096 * lowEndPos, 4096, (input - lowEndPos) / (4096 - lowEndPos));

    }

    private void Parse(string s)
    {
        string[] words = s.Split();
        if (words.Length != 11 || s[0] == ' ') return;
        Debug.Log("String received: >>" + s + "<<");
        for (int i = 0; i < digitalPinValues.Length; i++)
        {
            digitalPinValues[i] = Convert.ToInt16(words[i]);
        }
        for (int i = 0; i < analogPinValues.Length; i++)
        {
            analogPinValues[i] = Convert.ToInt16(words[i + digitalPinValues.Length]);
        }

        //analogPinValues[0] = 4096 - analogPinValues[0];
        float newv = (int)(Mathf.Exp((float)(4096f - analogPinValues[0]) / 4096f) * 4096f) - 4096;
        float middle = 640f;
        float real = Mathf.Lerp(0f, 0.5f, newv / middle);
        if (newv >= middle)
        {
            real = Mathf.Lerp(0.5f, 1f, (newv - middle) / 3000);
        }
        // zero 180
        // half way 570
        // max 2550
        analogPinValues[2] = Linearize(analogPinValues[2], 500, 0.6f);
        analogPinValues[1] = Linearize(4096 - analogPinValues[1], 500, 0.6f);
        analogPinValues[0] = Linearize(4096 - analogPinValues[0], 800, 0.7f);
        //Debug.Log("Real pin value: " + real + " old: " + analogPinValues[0] + " exp: " + Mathf.Exp((float)analogPinValues[0] / 4096f));
        //analogPinValues[0] = (int)(real * 4096);

    }

    internal byte Read()
    {

#if UNITY_EDITOR
        string s = "";

        if (stream.IsOpen)
        {
            //SendByte(202);

            int count = 0;
            //while (s.Length == 0 && count < 1000)
            {
                count++;
                try
                {
                    if (stream.IsOpen)
                    {
                        s = stream.ReadLine();
                    }

                }
                catch (Exception e)
                {

                }
            }
            //Debug.Log("stream iterations until data: " + count);
        }


        if (s.Length > 0)
        {
            Parse(s);
            //Debug.Log("Received string: " + s);
            //text.text = "received string: >>" + s + "<<";
            return 0; // (byte)s[0];

        }
        else
        {
            //Debug.Log("Nothing received. stream: " + stream.IsOpen);
            return 0;
        }
#elif UNITY_ANDROID
        //text.text = "reading..." + BtConnector.isConnected()  + " " + BtConnector.available() + " "+ BtConnector.controlData();
        // true false 1
        if (BtConnector.isConnected())
        {
            string s = "";
        
            byte[] message = new byte[1];
            message[0] = 202;
            //BtConnector.sendBytes(message);
            try
            {
                s = BtConnector.readLine();
                //byte[] b = BtConnector.readBuffer(100,  Convert.ToByte('\n'));
                //s = System.Text.Encoding.Default.GetString(b);
                //text.text = "count " + count;
                if (s.Length > 0)
                {

                    text.text = " received string: >>" + s + "<<";
                    Parse(s);
                    //return (byte)s[0];

                    // clear rest
                    //while (s.Length > 0)
                    //    s = BtConnector.read();
                    //BtConnector.stopListen();
                    s = BtConnector.read();
                
                }
                else return 0;
            }
            catch (Exception e)
            {}
        }
        else
        {
            reconnect = true;
        }
#endif
        return 0;
    }
}
