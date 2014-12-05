using UnityEngine;
using System.Collections;

public class Bars : MonoBehaviour {


	private float size = 0.0f;

	public Texture2D bar;

	public float x = 1.0f;
	public float y = 1.0f;
	public int width = 60;

	private int maxSize = 600;
	// assume that at a proximity of 30 the bar is empty and the take the proximity threshold from the script

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	// Input proximity
	public void setSize(float proximity) {
		if(proximity <= 0)
		{
			size = maxSize;
		}

		// 2 to 30 -> x = 30 / y + 2
		size = 30 / proximity + 2;
	}

	void OnGUI(){
		//GUI.Colo= new Color(1.0f, 1.0f, 1.0f, 0.1f);
		// adjust size input to the size of the bar
		GUI.DrawTexture(new Rect(Screen.width - width, Screen.height/2, 60, size), bar);
	}
}
