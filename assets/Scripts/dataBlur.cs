using UnityEngine;
using System.Collections;

public class dataBlur : MonoBehaviour {

	public GameObject textObject;
	public GameObject godObject;
	private float timer = 4.0f;
	public bool reverse = false;

	public int maxChar = 50;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		int minChar = 2;
		// Set maxChar based on some external variable
		// Let's say the kaleidoscope rotation speed absolute value
		var rSpeed = godObject.GetComponent<KaleidoscopeControls>().rotationSpeed;
		maxChar = minChar + 1 + Mathf.FloorToInt((Mathf.Abs(rSpeed * rSpeed *rSpeed)));
		//Debug.Log ("MaxChar " + maxChar);

		timer -= Time.deltaTime;
		if (timer < 0) {
			var t = Random.Range (0.7f, 2.5f);
			// relaunch text 
			var s = "";
			// Create data blurb of ones, zeroes and superpositions
			for (int i = 0; i < Random.Range (minChar, maxChar); i++)
			{
				var l = Random.Range (0,4);
				if (l <= 1) 
				{
					s+= 1;
				} else if (l <= 2){
					s+= 0;
				} else {
					s+= "Ø";
				}
			}

			if(reverse)
			{
				textObject.GetComponent<TypeOutScript> ().reverse = true;
			}
			textObject.GetComponent<TypeOutScript> ().reset = true;
			textObject.GetComponent<TypeOutScript> ().FinalText = s;
			textObject.GetComponent<TypeOutScript>().TotalTypeTime = t;
			textObject.GetComponent<TypeOutScript> ().On = true;

			// reset timer
			timer = t*1.3f;
		}
	}
}
