using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour, ISpawnLocation {

	// Use this for initialization
	public virtual void Start () {
        GetComponentInChildren<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}
}
