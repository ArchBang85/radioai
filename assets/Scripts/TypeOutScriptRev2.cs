using UnityEngine;
using System;
using System.Collections;
using System.Text;


[Serializable]
public class TypeOutScriptRev2 : MonoBehaviour {

	public bool On = true;
	public bool reset = false;
	public string FinalText;
	public bool reverse = false;
	public string swapText = "";
	
	public float TotalTypeTime = -1f;

	public float TypeRate;
	private float LastTime;

	public string RandomCharactor;
	public float RandomCharacterChangeRate = 0.1f;
	private float RandomCharacterTime;

	public int i;

	void Start () 
	{
		try
		{
			gameObject.AddComponent(typeof(GUIText));
		}
		catch(UnityException)
		{

		}

	}
	
	private string RandomChar()
	{
		byte value = (byte)UnityEngine.Random.Range(41f,128f);

		string c = Encoding.ASCII.GetString(new byte[]{value});

		return c;

	}

	public void Skip()
	{
		GetComponent<GUIText>().text = FinalText;
		On = false;
	}
	
	// Update is called once per frame
	void OnGUI() 
	{
		if (TotalTypeTime != -1f)
		{
			TypeRate = TotalTypeTime/(float)FinalText.Length;
		}

		if (On == true)
		{

			if (Time.time - RandomCharacterTime >= RandomCharacterChangeRate)
			{
				RandomCharactor = RandomChar();
				RandomCharacterTime = Time.time;
            }

			try
			{
			
				if(!reverse)
				{
					// GetComponent<GUIText>().text = FinalText.Substring(0,i) + RandomCharactor;
					GetComponent<GUIText>().alignment = TextAlignment.Right;
					// How to reverse text? Should look at the existing full text and take a substring from the position 
					// i to the end and print blanks at the start
					// var blanks = "";
					// for (int b = 0; b < (finaltext.length - i); b++)
					// {
						// blanks += " ";
					// }
					// debug.log ("blanks " + blanks + ".");
					// debug.log("reverse:" + FinalText.Substring(1, 3));
					// GetComponent<GUIText>().text = FinalText.Substring(i,FinalText.Length);
					
					Debug.Log("A: " + swapText);
					swapText = swapText + FinalText.Substring(FinalText.Length - i - 2,1);
					Debug.Log("B: " + FinalText);
					GetComponent<GUIText>().text = swapText.Substring(0, i) + RandomCharactor;
				}
				else
				{
					
					GetComponent<GUIText>().alignment = TextAlignment.Right;
					// How to reverse text? Should look at the existing full text and take a substring from the position 
					// i to the end and print blanks at the start
					// var blanks = "";
					// for (int b = 0; b < (finaltext.length - i); b++)
					// {
						// blanks += " ";
					// }
					// debug.log ("blanks " + blanks + ".");
					// debug.log("reverse:" + FinalText.Substring(1, 3));
					// GetComponent<GUIText>().text = FinalText.Substring(i,FinalText.Length);
					swapText = "     ";
					for (int b = FinalText.Length-1; b > 0; b--) {
						swapText.Insert(swapText.Length, FinalText.Substring(b,1));
					}
					FinalText = swapText;
					Debug.Log(FinalText);
					GetComponent<GUIText>().text = FinalText.Substring(0, i) + RandomCharactor;
				}
			}
			catch(ArgumentOutOfRangeException)
			{
				On = false;
				Debug.Log("Whoops "+FinalText);
			}

			if (Time.time- LastTime >= TypeRate)
			{
				i++;
				LastTime = Time.time;
			}

			bool isChar = false;

			while (isChar == false)
			{
				if ((i + 1) < FinalText.Length)
				{
					if (FinalText.Substring(i,1) == " ")
					{
						i++;
					}
					else
					{
						isChar = true;
					}
				}
				else
				{
					isChar = true;
				}
			}

			if (GetComponent<GUIText>().text.Length == FinalText.Length + 1)
			{
				RandomCharactor = RandomChar();
				GetComponent<GUIText>().text = FinalText;
				On = false;
			}

		}

		if (reset == true )
		{
			GetComponent<GUIText>().text = "";
			i = 0;
			reset = false;
		}
	}
}
