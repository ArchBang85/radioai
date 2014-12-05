using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class branch : MonoBehaviour {
	
	public class branchNode
	
	{
		public int ID;
		// proportions of the corpuses to throw in the mix
		public int[] corpuses = new int[8]; 
		public int nextID; 	
	}
	
	public branchNode[] nodes = new branchNode[31];
	
	void Start () {
		// Create basic tree
		for(int i = 0; i < 31; i++)
		{
			nodes[i] = new branchNode();
			nodes[i].ID = i;

			nodes[i].corpuses[1] = 2 + i;
			nodes[i].nextID = i + 2;

			Debug.Log(nodes[i].corpuses[1]);
			
		}
	
	
	}

	void Update () {

	}

}