using UnityEngine;
using System.Collections;


public class KaleidoscopeControls : MonoBehaviour {

	#region declarations		
	// Radio box controller code:
	public int[] digitalPinValues = new int[7];
	public int[] analogPinValues = new int[3];
	private float snoozePressed = 2.0f;
		
	// Main game behaviour happens to be here
	public bool keyboardControls = true;
	public bool leapmotionControls = true;
	public bool radioBoxControls = false;
	public bool dbug = true;
	public bool showText = true;
	private bool levelEnd = false;
	private bool levelSuccess =false;

	public GameObject leftHandController;
	public GameObject rightHandController;
	public float lateralResetTimer;
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
	private float lightningIntensity = 1.0f;
	private float lightningIntensityMax = 5.0f;// Fierceness of the pulsation could be linked to the Scale of the Lightning Bolts
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
	// No, let's do it as level progression
	public int kaleidoscopeLevel = 0;
	// Let's also have a polarity that switches the sides of the texts to keep the player guessing a little
	// This should reverse the angle of the kaleidoscope
	public bool polarityInversion = false;
	
	public float[] proximity = new float[2];
	public int targetInRange = -1;
	// level 0, levels 1, levels 2, levels 3 thresholds
	private float[] proximityLevels = new float[5]; 
	public float[] markovLevels = new float[2];
	// Game progression behaviour 
	public float proximityTimer;
	public float proximityLimit = 2.0f; // How close are the two distinct targets allowed to be to each other
	public float progressionThreshold = 4.0f; // How close does the player need to be to the target to be able to get to the next level
	public float currentScore;
	public GameObject[] bars;
	public Texture bar;
	public Texture barBackground;

	#endregion

	// Use this for initialization
	void Start () {
		SetTargets ();
		TargetProximity ();
		// draw bars
		for(int i = 1; i < proximityLevels.Length + 1; i++)
		{
			proximityLevels[i-1] = progressionThreshold * i * 1.5f;
			Debug.Log ("Proxlevels " + proximityLevels[i - 1]);
		}

		
		// leapMotionController overrides because targets need to be set differently
		if(leapmotionControls)
		{
			keyboardControls = false;
			radioBoxControls = false;
		} 

		if(dbug)
		{
			// Testing control mapping
			for (int i = 4096; i > 4; i -= 400)
			{
				Debug.Log ((float)i * -5 / 4092 + 5);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(keyboardControls)
		{
			keyboardController();

		}

		if(radioBoxControls)
		{
			radioBoxController();
		}

		if(leapmotionControls)
		{
			leapMotionController();
		}

		//////										  //////
		/// Updating the variables needed for targetting ///
		//////										  //////

		// Colour control
		mainLight.light.color = new Color (colourRGB [0], colourRGB [1], colourRGB [2], 0.4f);
		
		// spinning light colour
		for (int l = 0; l < sideLights.Length; l++)
		{
			sideLights[l].light.color = new Color(colourRGB[0], colourRGB[1], colourRGB[2], 0.1f);
		}
		
		// light intensity
		lightning.gameObject.GetComponent<LightningBolt>().scale = lightningIntensity;
		
		// Pulse speed
		//lightning.gameObject.GetComponent<LightningBolt> ().speed = pulseSize;
		
		// Link pulse speed to pulse range somehow
		
		// A fine minimum is:20 and a maximum is:40 
		//mainLight.light.range = (pulseSizeMax - pulseSize) / (pulseSizeMin/pulseSizeMax) + 10;
		mainLight.light.range = pulseSize + 5;
		
		// Rotation
		transform.Rotate (Vector3.forward * rotationSpeed * Time.deltaTime);


		if(levelEnd)
		{
			levelEnd = false;
			int closerSide = -1;
			// set level success
			if(proximity[0] < proximity[1])
			{
				closerSide = 0;
			} else {
				closerSide = 1;
			}
			if(proximity[closerSide] < proximityLevels[0])
			// Closer than the smallest
			{
				// The winning side text is sent
				if(closerSide == markov.GetComponent<Markov>().winningSide)
					{
						levelSuccess = true;
					}
			}
			if(levelSuccess){
				GameObject.Find ("IndicatorLight").GetComponent<Flash>().greenFlash();
			} else {
				GameObject.Find ("IndicatorLight").GetComponent<Flash>().redFlash();
			}

			// Do level end things
			proximity[0] = 0;
			proximity[1] = 0;
			SetTargets();
			markov.GetComponent<Markov>().levelController();
			// Flash light based on success or not


			if(levelSuccess){
				if(kaleidoscopeLevel < 4)
				{
					kaleidoscopeLevel += 1;
				}
			} else {
				kaleidoscopeLevel = 0;

			}
			levelSuccess = false;
			LevelSetter();
		
		}


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

	void radioBoxController(){
		
		// Radiobox controls
		if(radioBoxControls)
		{
			// Main dial
		
			// Rotation speed from -5 to 5 or so
			// main dial ranges from min: 3895 to half 3500 to max 1900
			// Robin already has a function to map this linearly? 
			// -10/1995x + 14.5 = -5 when x = 3895 and 5 when x = 1900	
			var mainDial = analogPinValues[0];
			rotationSpeed = -10/1995 * mainDial + 14.5f;

			// Lightning intensity
			// Left small dial
			// should be let's say from 0 to 5
			// x = -5 / 4092
			// 4096 * -5 / 4092 + y = 0 when y = 4096
			// 4 * -5 / 4092 + y = 5 when y = 
			var leftDial = analogPinValues[1]; // min 4096 half 3700 max 4
			lightningIntensity = 4096 * -5 / 4092 + leftDial;

			// Colour switch
			if (digitalPinValues[3] == 0 && digitalPinValues[4] == 1)
			{
				// Auto
				RGBswitch = 0;

			} else if (digitalPinValues[3] == 1 && digitalPinValues[4] == 1) {
				// Buzzer
				RGBswitch = 1;
			} else if (digitalPinValues[3] == 1 && digitalPinValues[4] == 0) {
				// Radio on
				RGBswitch = 2;
			}
			
			// Colour value
			// right dial 
			// Values range from low 3 to high 4096
			var rightDial = analogPinValues[2];

			rightDial = rightDial * (255/4093) + (-3 * 255 / 4093);

			// Make surightDialightDiale it isn't out of bounds, just in case:
			if (rightDial < 0) rightDial = 10;
			if (rightDial > 255) rightDial = 245;

			colourRGB[RGBswitch] = rightDial;

			// Colour size
			
			// Message transmission
			// Snooze button pressed
			snoozePressed -= Time.deltaTime;

			if(digitalPinValues[6] == 0 && snoozePressed < 0 || Input.GetKeyDown ("r"))
			{

				// Transmit text
				if(targetInRange >= 0 && !levelEnd)
					
				{

					// Send/display whichever text is closer to completion:
					markov.GetComponent<Markov>().transmitText();
					levelEnd = true;
				}

				// there should be a cooldown timer here
				snoozePressed = 6.0f;

			}
			
			// left switch could be for view switching. No, view switching should happen as you progress through levels

			// what about the power switch?
			// This could be for switching the text on and off
			if(digitalPinValues[2] == 1)
			{
				showText = false;
			} else {
				showText = true;
			}

			// what about the light switch?
			// Could be rhythmic growing and shrinking of the colour range?
			if(digitalPinValues[0] == 0 && digitalPinValues[1] == 1)
			{
				// FM
				this.gameObject.GetComponent<TwirlEffect>().enabled = false;
				this.gameObject.GetComponent<VortexEffect>().enabled = true;
			} else if (digitalPinValues[0] == 1 && digitalPinValues[1] == 1){
				// LW - Standard view
				this.gameObject.GetComponent<TwirlEffect>().enabled = false;
				this.gameObject.GetComponent<VortexEffect>().enabled = false;

			} else if (digitalPinValues[0] == 1 && digitalPinValues[1] == 0){
				// AM
				// Set twirl view
				this.gameObject.GetComponent<TwirlEffect>().enabled = true;
				this.gameObject.GetComponent<VortexEffect>().enabled = false;
			}
		}

	}
	void keyboardController(){

		// Keyboard controls
		if(keyboardControls){

			if (Input.GetKeyDown ("t")){

					if(this.gameObject.GetComponent<TwirlEffect>().enabled == false && this.gameObject.GetComponent<VortexEffect>().enabled == true)
					{
						this.gameObject.GetComponent<VortexEffect>().enabled = false;
					} else if (this.gameObject.GetComponent<TwirlEffect>().enabled == false && this.gameObject.GetComponent<VortexEffect>().enabled == false)
					{
						this.gameObject.GetComponent<TwirlEffect>().enabled = true;
					} else if (this.gameObject.GetComponent<TwirlEffect>().enabled == true && this.gameObject.GetComponent<VortexEffect>().enabled == false) {
						this.gameObject.GetComponent<TwirlEffect>().enabled = false;
						this.gameObject.GetComponent<VortexEffect>().enabled = true;
					}

				}

			if (Input.GetKeyDown (KeyCode.Space))
			{
				
				//GameObject.Find ("Tweeter").GetComponent<Tweeter>().Send("Hello World!");
				// test tweet
				// Tweeter.Send ("Hello World 4");
				
				// This should only happen once per level
				if(targetInRange >= 0 && !levelEnd)
					
				{
					//markov.GetComponent<Markov>().activateText();
					markov.GetComponent<Markov>().transmitText();
					levelEnd = true;
					snoozePressed = 6.0f;
				} else { 
					
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
				if(colourRGB[RGBswitch] > 10)
				{
					colourRGB[RGBswitch] -= 1;
				} else {
				colourRGB[RGBswitch] = 10;
			}
				
			}
			if (Input.GetKey ("l")) {
				if(colourRGB[RGBswitch] < 155)
				{
					colourRGB[RGBswitch] += 1;
				} else {
					colourRGB[RGBswitch] = 154;
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
			if (Input.GetKeyDown ("a") && Random.Range (0,1000) > 990) {
				kaleidoscopeLevel += 1;
				if(kaleidoscopeLevel == 3)
				{
					kaleidoscopeLevel = 0;
				}
				int angle;
				if (kaleidoscopeLevel == 0)
				{
					angle = 30;
					if(polarityInversion)
					{
						angle = -angle;
					}
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 6;
				}
				if (kaleidoscopeLevel == 1)
				{
					angle = 22;
					if(polarityInversion)
					{
						angle = -angle;
					}
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 8;
				}
				if (kaleidoscopeLevel == 2)
				{
					angle = 15;
					if(polarityInversion)
					{
						angle = -angle;
					}
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 12;
				}
				if (kaleidoscopeLevel == 3)
				{
					angle = 20;
					if(polarityInversion)
					{
						angle = -angle;
					}
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
					GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 24;
				}
			}
		}	
	}
	
	void leapMotionController()
	{
		// Leap Motion controls 
		if(leapmotionControls)
		{		
			// Check if left hand is set
			if(leftHandController == null)
			{
				leftHandController = GameObject.Find("leftHand");		
				// Make the sub-parts of the hand invisible
				foreach(Transform child in leftHandController.transform)
				{
					try
					{
						child.gameObject.GetComponent<LineRenderer>().enabled = false;
						child.gameObject.renderer.enabled = false;
					}
					catch
					{
						Debug.Log (child.gameObject + " does not have a renderer to disable");
					}
				}
			}
			
			// Check if right hand is set
			if(rightHandController == null)
			{
				rightHandController = GameObject.Find ("rightHand");
				// Hide the hand physical objects
				
				foreach(Transform child in rightHandController.transform)
				{
					if(dbug)
					{
						Debug.Log (child.gameObject);
					}
					
					try
					{
						child.gameObject.renderer.enabled = false;
						child.gameObject.GetComponent<LineRenderer>().enabled = false;
					}
					catch
					{
						Debug.Log (child.gameObject + " does not have a renderer to disable");
					}
				}			
				
				
			}
			// Rotation control
			float lateral = -rightHandController.transform.rotation.z;
			
			// sensitivity
			if(Mathf.Abs (lateral) > 0.08f)
			{	
				// Positive (tilt right)
				if (lateral > 0.08f){
					if(rotationSpeed < rotationMax)
					{
						rotationSpeed += Time.deltaTime * 0.5f;
						lateralResetTimer = 1.0f;
						//			Debug.Log (rotationSpeed);
					}
				} else if (lateral < -0.08f) {
					if(rotationSpeed > -rotationMax)
					{
						rotationSpeed -= Time.deltaTime * 0.5f;
						lateralResetTimer = 1.0f;
						//			Debug.Log (rotationSpeed);
					}
				}
				lateralResetTimer -= Time.deltaTime;
			}
			
			rotationDisc.GetComponent<Rotation> ().speed = rotationSpeed * 125;
			
			if(lateralResetTimer < 0)
			{
				//rotationDisc.GetComponent<Rotation> ().speed = 0;
				//lateralResetTimer = 0;
			}
			
			// Particle intensity adjustment (tilt forward and back)
			float xRotation = rightHandController.transform.rotation.x;		
			/*	if (xRotation > 0.08f)
			{
				if(lightningIntensity < lightningIntensityMax)
				{
					lightningIntensity += Time.deltaTime * 0.5f;
				}
			}
			else if (xRotation < -0.08f) 
			{
				if(lightningIntensity > -lightningIntensityMax)
				{
					lightningIntensity = xRotation * 5.0f;
				}
			}
		*/
			// Mapping intensity to right hand rotation
			//lightningIntensity = xRotation * 5.0f;
			
			// Mapping particle intensity to right hand elevation
			float rightHandElevation = rightHandController.transform.position.y;
			// Need to map it on a level from about -5 to 5
			float elevationLimit = 5.0f;
			lightningIntensity = rightHandElevation * 5.0f;
		}

	}



	
	#region MappingFunctions
		// Mapping functions
		float mapElevation (float e)
		{
				// What are the actual inputs? Seem to be between 0 and 7
				// outputs from -5 to 5
				// Function needed:
				e = e * 10 / 7 - 5;

				// Set limits:
				float elevationLimit = 5.0f;
				if (e <= -elevationLimit) {
						return -elevationLimit;
				} else if (e >= elevationLimit) {
						return elevationLimit;
				} 
			
				return e;

		}
	#endregion

	#region ProgressionTargets


	void LevelSetter()
	{
		int angle;
		if (kaleidoscopeLevel == 0)
		{
			angle = 30;
			if(polarityInversion)
			{
				angle = -angle;
			}
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 6;
		}
		if (kaleidoscopeLevel == 1)
		{
			angle = 22;
			if(polarityInversion)
			{
				angle = -angle;
			}
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 8;
		}
		if (kaleidoscopeLevel == 2)
		{
			angle = 15;
			if(polarityInversion)
			{
				angle = -angle;
			}
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 12;
		}
		if (kaleidoscopeLevel == 3)
		{
			angle = 20;
			if(polarityInversion)
			{
				angle = -angle;
			}
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().angle = angle;
			GameObject.FindWithTag("MainCamera").GetComponent<KaleidoscopeEffect>().number = 24;
		}
	}
	
	void SetTargets() 
	{
		Debug.Log ("setting targets");
		// Should be different based on the MAIN control method -> Needs to be a main control method
		// If Leapmotion is active, then there should be fewer dimensions to target
		// Keyboard and radiobox should be equivalent
			pulseSizeTargets[0] = Random.Range (0.0f, pulseSizeMax);
			pulseSizeTargets[1] = Random.Range (0.0f, pulseSizeMax);
			/*while (Mathf.Abs(pulseSizeTargets[1] - pulseSizeTargets[1]) < 0.01f) {
				pulseSizeTargets[0] = Random.Range (0.0f, pulseSizeMax);
				pulseSizeTargets[1] = Random.Range (0.0f, pulseSizeMax);
			}*/
			lightningIntensityTargets[0] = Random.Range (0.0f, lightningIntensityMax);
			lightningIntensityTargets[1] = Random.Range (0.0f, lightningIntensityMax);
			// randomise colours
			var cMax = 70;
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

		// Proximity dimensions link to control scheme
		if(!leapmotionControls)
		{


			// Create an aggregate figure for how close the current settings are to one of the two sets of targets
			float colourProximity = 0.0f;
			for (int i = 0; i < 2; i++)
			{
				proximity[i] = 0;
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
				if (zAngle + proximity[i] > 180)
				{
					zAngle = 360 - zAngle - proximity[i];
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
					
					//Debug.Log ("IN RANGE");
					// Is close enough, activate the relevant object
					
					targetInRange = i;
					//if(dbug)
					//{
					Debug.Log ("In range of target " + i);
					//}
					// Show transmittable text
					markov.GetComponent<Markov>().activateText();	
					
					// Set new targets & level
					// SetTargets();
					
					
				} else if (targetInRange > 0)
				{
					targetInRange = -1;
				}
			}
			
			// Reset targets if they are too close to each other // WRONG
		//	if(Mathf.Abs (proximity[0] - proximity[1]) < 2)
	//		{
//				SetTargets ();
//				if(dbug)
//					Debug.Log ("resetting targets");
//			}
			if (dbug) {
				
				Debug.Log ("prox to target 1 " + proximity[0]);
				Debug.Log ("prox to target 2 " + proximity[1]);
				Debug.Log (colourProximity);
			}
		} else {

			for (int i = 0; i < 2; i++)
			{
				// Leapmotion-specific targetting
				// How to ensure that the staggering is just about right? 
				proximity[i] += Mathf.Abs (lightningIntensityTargets[i] - lightningIntensity);
				// Keeping it even when it goes around 360
				var zAngle = this.transform.eulerAngles.z;
				if (zAngle > 180)
				{
					zAngle = 360 - zAngle;
				}
				proximity[i] += (Mathf.Abs(zAngle - angleTargets[i]) / 20);
			}
		}
	}
	#endregion
}