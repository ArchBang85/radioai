using UnityEngine;
using System.Collections;


public class KaleidoscopeControls_Copy : MonoBehaviour {

	// Main game behaviour happens to be here

	public bool dbug = true;
	private bool levelEnd = false;

	public GameObject markov;
	// The main dial should control the rotation speed
	public float rotationSpeed = 5.0f; // Rotation speed should go from negative to positive
	public float rotationMax = 5.0f;
	public GameObject rotationDisc;

	// Let's set a particular angle of the kaleidoscope as more desirable than others
	public float[] angleTargets = new float[2];

	// Should be tied to one of the small dials
	public float pulseSize = 0.0f; // Size of the pulsing light
	public float pulseSizeMax = 10.0f;
	public float pulseSizeMin = 0.2f;

	// One of the small  dials needs to control lightning intensity
	public float lightningIntensity = 1.0f;
	public float lightningIntensityMax = 5.0f;// Fierceness of the pulsation could be linked to the Scale of the Lightning Bolts
	public GameObject lightning;
	
	public int[] colourRGB = new int[3];
	public GameObject mainLight; 
	public GameObject[] sideLights;
	public int RGBswitch = 0;
	// Maybe one of the tri-state switches could switch between R G B?
	private float t = 1;

	// Arrays to contain the two sets of target values for each level
	public float[] rotationTargets = new float[2];
	public float[] pulseSizeTargets = new float[2];
	public float[] lightningIntensityTargets = new float[2];
	public int[] RGBTargets = new int[6];

	// The AM / FM / switch could be used to change the amount of angles in the Kaleidoscope
	// 6 & 8 & 12
	public int kaleidoscopeLevel = 0;

	public float[] proximity = new float[2];
	public int inRange = -1;
	// level 0, levels 1, levels 2, levels 3 thresholds
	private float[] proximityLevels = new float[4]; 
	public float[] markovLevels = new float[2];
	// Game progression behaviour 
	public float proximityTimer;
	public float proximityLimit = 2.0f; // How close are the two distinct targets allowed to be to each other
	public float progressionThreshold = 2.0f; // How close does the player need to be to the target to be able to get to the next level
	public float currentScore;
	public GameObject[] bars;
	public Texture bar;
	public Texture barBackground;
	
	// Use this for initialization
	void Start () {
		SetTargets ();
		TargetProximity ();
		// draw bars
		for(int i = 1; i < proximityLevels.Length + 1; i++)
		{
			proximityLevels[i-1] = progressionThreshold * i * 2;
			Debug.Log ("Proxlevels " + proximityLevels[i - 1]);
		}

	}

	/*void onGUI() {
		GUI.DrawTexture(new Rect(transform.position.x, transform.position.y, 60, 60), bar1, ScaleMode.ScaleToFit);
	}*/

	// Update is called once per frame
	void Update () {

		// Keyboard controls
		if (Input.GetKeyDown (KeyCode.Space))
		{

			// This should only happen once per level
			if(inRange >= 0 && !levelEnd)

			{
				markov.GetComponent<Markov>().activateText();
				markov.GetComponent<Markov>().transmitText();
				levelEnd = true;
			}
		}

		if(Input.GetKey ("o"))
		{
			if (pulseSize > pulseSizeMin)
				pulseSize -= 2.0f * Time.deltaTime;
		}
		if(Input.GetKey ("p"))
		{
			if (pulseSize < pulseSizeMax)
			{
				pulseSize += 2.0f * Time.deltaTime;
			}
		}
		if (dbug) {
					Debug.Log (pulseSize);
				}
		// colourRGB adjustment

		if (Input.GetKey ("k")) {
			if(colourRGB[RGBswitch] > 0)
			{
				colourRGB[RGBswitch] -= 1;
			}

		}
		if (Input.GetKey ("l")) {
			if(colourRGB[RGBswitch] < 155)
			{
				colourRGB[RGBswitch] += 1;
			}
		}

		if (Input.GetKeyDown ("c")) {

			RGBswitch += 1;
			if(RGBswitch == 3)
			{
				RGBswitch = 0;
			}
			if(dbug)
				Debug.Log ("RGB switch " + RGBswitch);
			}
		// Lightning intensity
		if (Input.GetKey ("n")) {
				if (lightningIntensity > 0) {

				lightningIntensity -= Time.deltaTime;

				} 
		}
		if (Input.GetKey ("m")) {
			if(lightningIntensity < lightningIntensityMax)
			{
				lightningIntensity += Time.deltaTime;
			}
		}

		// Rotation speed
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if(rotationSpeed > -rotationMax)
			{
				rotationSpeed -= Time.deltaTime * 1.0f;
				Debug.Log (rotationSpeed);
			}
		} 
		if (Input.GetKey (KeyCode.RightArrow)) {
			if(rotationSpeed < rotationMax)
			{
				rotationSpeed += Time.deltaTime * 1.0f;
			}

		}

		rotationDisc.GetComponent<Rotation> ().speed = rotationSpeed * 125;

		// Kaleidoscope angularity cycle
		if (Input.GetKeyDown ("a")) {
			kaleidoscopeLevel += 1;
			if(kaleidoscopeLevel == 3)
			{
				kaleidoscopeLevel = 0;
			}

			if (kaleidoscopeLevel == 0)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeIndieEffect>().angle = -30;
				GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeIndieEffect>().number = 6;
			}
			if (kaleidoscopeLevel == 1)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeIndieEffect>().angle = 22;
				GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeIndieEffect>().number = 8;
			}
			if (kaleidoscopeLevel == 2)
			{
				GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeIndieEffect>().angle = 15;
				GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeIndieEffect>().number = 12;
			}
		}

		// Colour control
		mainLight.light.color = new Color (colourRGB [0], colourRGB [1], colourRGB [2], 0.4f);

		// spinning lights
		for (int l = 0; l < sideLights.Length; l++)
		{
			sideLights[l].light.color = new Color(colourRGB[0], colourRGB[1], colourRGB[2], 0.1f);
		}



		// Test increment with time
//		if (t < 0) 
//		{
//			if(RGBswitch == 0)
//			{
//				colourRGB[0] += 1;
//			} else if (RGBswitch == 1) {
//				colourRGB[1] += 1;
//			} else if (RGBswitch == 2){
//			    colourRGB[2] += 1;
//			}
//				
//			t = 1;
//			if (lightningIntensity < lightningIntensityMax){
//				lightningIntensity += 0.1f;
//			}
//		}
//		t -= Time.deltaTime;

		// light intensity
		lightning.gameObject.GetComponent<LightningBolt>().scale = lightningIntensity;

		// Pulse speed
		//lightning.gameObject.GetComponent<LightningBolt> ().speed = pulseSize;

		// Link pulse speed to pulse range somehow

		// A fine minimum is:20 and a maximum is:40 
		//mainLight.light.range = (pulseSizeMax - pulseSize) / (pulseSizeMin/pulseSizeMax) + 10;
		// It gets too twitchy...

		mainLight.light.range = pulseSize + 5;


		// Rotation
		transform.Rotate (Vector3.forward * rotationSpeed * Time.deltaTime);
		if(dbug)
			{
			Debug.Log (rotationSpeed);

			}
		// Proximity check every now and then
		proximityTimer -= Time.deltaTime;
		if (proximityTimer < 0) 
		{
			TargetProximity();
			setProximityLevel();
			proximityTimer = 0.1f;

		}

	}

	
	void SetTargets() 
	{

		pulseSizeTargets[0] = Random.Range (0.0f, pulseSizeMax);
		pulseSizeTargets[1] = Random.Range (0.0f, pulseSizeMax);
		/*while (Mathf.Abs(pulseSizeTargets[1] - pulseSizeTargets[1]) < 0.01f) {
			pulseSizeTargets[0] = Random.Range (0.0f, pulseSizeMax);
			pulseSizeTargets[1] = Random.Range (0.0f, pulseSizeMax);
		}*/
		lightningIntensityTargets[0] = Random.Range (0.0f, lightningIntensityMax);
		lightningIntensityTargets[1] = Random.Range (0.0f, lightningIntensityMax);
		// randomise colours
		var cMax = 50;
		for (int i = 0; i < 3; i++) {
			// Don't allow them all to be maxed out
			colourRGB[i] = Random.Range (0, cMax);
			if(colourRGB[i] > (0.5f * cMax))
			{
				cMax = Mathf.FloorToInt(cMax / 2);
			}
		}

		// randomise colour targets
		for (int i = 0; i < RGBTargets.Length; i++) {
			RGBTargets[i] = Random.Range (15, 175);
		}

		// Randomise angle target
		angleTargets[0] = Random.Range (10.0f, 350.0f);
	
		angleTargets[1] = Random.Range (10.0f, 350.0f);

		if (Mathf.Abs(angleTargets[0] - angleTargets[1]) < 40)
		{
			angleTargets[1] = Random.Range (10.0f, 350.0f);
		}
		// check how close the two overall targets are
		TargetProximity ();

		// Set the markov chains in motion


	}


	void OnGUI(){
		//GUI.Colo= new Color(1.0f, 1.0f, 1.0f, 0.1f);
		// adjust size input to the size of the bar
		// Blank is on top and gets withdrawn as the target is nearer
		var h = 150;
		if(dbug)
		{
			//Debug.Log ("Hello");
			Debug.Log ("drawing bars " + (proximity[0]* 20));
		}

		//GUI.DrawTexture(new Rect(0, h, 60, (600 - Mathf.Min (30, proximity[1]) * 20)), barBackground);

		// Left bar
		GUI.DrawTexture(new Rect(0, 0, 60, Screen.height), bar);
		GUI.DrawTexture(new Rect(0, 0, 60, (proximity[0]) * 25), barBackground);
		// Right bar
		GUI.DrawTexture(new Rect(Screen.width - 60, 0, 60, Screen.height), bar);
		GUI.DrawTexture(new Rect(Screen.width - 60, 0, 60, ((proximity[1]) * 25)), barBackground);
			
	}

	void setProximityLevel()
	{
		for (int i = 0; i < proximity.Length; i++)
		{
			markovLevels[i] = proximityLevels.Length;
			for (int l = 0; l < proximityLevels.Length; l++)
			{
				if(proximity[i] < proximityLevels[l])
				{
					// Set markov accuracy based on 
					markovLevels[i] = l;
					break;
				}
			}
		}

		if(dbug)
		{
		Debug.Log ("markov level" + markovLevels[0]);
		}

	}


	void TargetProximity()
	{
		// Create an aggregate figure for how close the current settings are to one of the two sets of targets
		float colourProximity = 0.0f;;
		for (int i = 0; i < 2; i++)
		{
			proximity[i] = Mathf.Abs (pulseSizeTargets[i] - pulseSize) / 3;
			proximity[i] += Mathf.Abs (lightningIntensityTargets[i] - lightningIntensity) / 2;

			colourProximity = Mathf.Abs (RGBTargets[i*3] - colourRGB[0]);
			colourProximity += Mathf.Abs (RGBTargets[i*3 + 1] - colourRGB[1]);
			colourProximity += Mathf.Abs (RGBTargets[i*3 + 2] - colourRGB[2]);
			colourProximity = colourProximity / 20;

			proximity[i] += colourProximity;

			// Colour the targets the more brightly if the player is closer to one over the other
			//bars[i].gameObject.light.intensity = 1 / proximity[i] * 100 - 4;
			//Debug.Log(1 / proximity[i] * 100 - 2);

			if(bars[i].gameObject.light.intensity < 0.05f)
			{
				bars[i].gameObject.light.intensity = 0.05f;
			}
			else if(bars[i].gameObject.light.intensity > 4.0f)
			{
				bars[i].gameObject.light.intensity = 4.0f;
			}

			// rotational proximity
			//Debug.Log ("eulerangles " + this.transform.eulerAngles.z);
			// Keeping it even when it goes around 360
			var zAngle = this.transform.eulerAngles.z;
			if (zAngle > 180)
			{
				zAngle = 360 - zAngle;
			}
			proximity[i] += (Mathf.Abs(zAngle - angleTargets[i]) / 45);

			if(dbug)
			{
				Debug.Log ("rotational proximity of camera for target " + i + " " + (angleTargets[i] - this.transform.eulerAngles.z) / 45);
			}

			// Update bar size based on proximity
			//bars[i].GetComponent<Bars>().setSize (proximity[i]);
	
			// Check if the player is close enough to one of the targets
			if (proximity[i] <= progressionThreshold)
			{
				// Is close enough, activate the relevant object
				if(inRange < 0)
				{
					inRange = i;
						if(dbug)
						{
							Debug.Log ("In range of target " + i);
						}
					// Show transmittable text
					markov.GetComponent<Markov>().activateText();


				}
			} else {
				// Deactivate object if it is active
				if(inRange > 0)
				{
					inRange = -1;
				}
			}
		}

		// Reset targets if they are too close to each other
		if(Mathf.Abs (proximity[0] - proximity[1]) < 2)
		{
			SetTargets ();
			if(dbug)
				Debug.Log ("resetting targets");
		}
		if (dbug) {

			Debug.Log ("prox to target 1 " + proximity[0]);
			Debug.Log ("prox to target 2 " + proximity[1]);
			Debug.Log (colourProximity);
		}

	}
}
