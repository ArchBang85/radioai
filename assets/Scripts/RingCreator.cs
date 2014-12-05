using UnityEngine;
using System.Collections;

public class RingCreator : MonoBehaviour {

	public GameObject ringPart;
	public float ringRadius = 4.5f;
	public float angler = 5f;
	public int segments = 12;
	public int occurrenceProb = 4;


	// Use this for initialization
	void Start () {

		GameObject center = Instantiate (ringPart, new Vector3 (ringRadius, 0, 0), Quaternion.identity) as GameObject;
		center.renderer.enabled = false;
		center.transform.collider.enabled = false;

		float step = 2 * Mathf.PI / segments;
		for (int i = 0; i < segments; i++) {

			GameObject ringPartInstance = Instantiate (ringPart, new Vector3 ((Mathf.Sin (angler) + Mathf.Sin (i * step)) * ringRadius, (Mathf.Cos (i * step)) * ringRadius, 0), Quaternion.identity) as GameObject;
			ringPartInstance.transform.LookAt(center.transform.position);
			ringPartInstance.transform.parent = this.transform;
			// randomise whether the block is active or not
			if (Random.Range (1,10) > occurrenceProb)
			{
				ringPartInstance.renderer.enabled = false;
				ringPartInstance.transform.collider.isTrigger = true;
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
