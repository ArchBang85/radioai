using UnityEngine;
using System.Collections;

public class lightControl : MonoBehaviour {

	public float pulseSpeed = 3.0f;
	private float pulseTimer = 0.7f;
	private bool increasing = true;
	public GameObject light;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		pulseTimer -= Time.deltaTime;
		if(pulseTimer < 0)
		{
		
			increasing = !increasing;
		

			pulseTimer = 0.7f;
		}
		if(increasing)
		{ 
			light.light.intensity += Time.deltaTime * pulseSpeed;
		} else if (!increasing){
			light.light.intensity -= Time.deltaTime * pulseSpeed;
		}
	
	}
}
