using UnityEngine;
using System.Collections;
using System;

public class DroneSound: MonoBehaviour {

	public float frequency = 110;
	public float gain = 0.05f;

	public float t = 1.0f;

	private float increment;
	private float phase;
	private float sampling_frequency = 48000;

	void OnAudioFilterRead(float[] data, int channels)
	{
		// update increment in case frequency ahs changed
		increment = frequency * 2 * Mathf.PI / sampling_frequency;
		for (int i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// copy audio data to make them available to Unity
			data[i] = (float)(gain * Mathf.Sin (phase));
			if (channels == 2) data[i+1] = data[i];
			if(phase > 2 * Mathf.PI) {
				phase = 0;
			}
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		t -= Time.deltaTime;

		if (t < 0) {

			frequency+=1;
			t = 1.0f;
		}


	}
}

public class Noise : MonoBehaviour {
	// un-optimised noise generator
	private System.Random RandomNumber = new System.Random();
	public float offset = 0f;

	void OnAudioFilterRead(float[] data, int channels)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = offset - 1.0f + (float)RandomNumber.NextDouble() * 2.0f;

		}
	}
}
