using UnityEngine;
using System.Collections;

public class TransmissionLight : MonoBehaviour {

	private float counter = 0;
	public GameObject textController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		counter+= Time.deltaTime;
		this.transform.light.range += Time.deltaTime * 50;

		if (counter > 2.2f)
		{
			textController.GetComponent<Markov>().deactivateText();
		}
	}
}
