using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	float deactivationTimer = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown("z"))
		{
			redFlash ();
		} 

		if(Input.GetKeyDown ("x"))
		{
			greenFlash ();
		}


		if(deactivationTimer > 0)
		{
			deactivationTimer -= Time.deltaTime;
		}

		if(deactivationTimer < 0)
		{
			this.light.enabled = false;
		}
	}

	public void redFlash(){
		this.light.enabled = true;
		this.light.color =  new Color(0.8f, 0.1f, 0.1f, 1.0f);
		deactivationTimer = 1.2f;
	}
	public void greenFlash(){
		this.light.enabled = true;
		this.light.color = new Color(0.1f, 0.8f, 0.1f, 1.0f);
		deactivationTimer = 1.2f;
	}
}
