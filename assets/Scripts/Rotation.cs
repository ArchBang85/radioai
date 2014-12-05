using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {
	public float speed = 1.0f;
	public enum Direction{forward, backward, up, down, left, right}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(transform.position, -Vector3.forward, speed * Time.deltaTime);
		//transform.RotateAround(transform.collider.bounds.center, Vector3.forward, speed * Time.deltaTime);
	}
}
