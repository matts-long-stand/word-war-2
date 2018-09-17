using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public Player owner;
    protected float startTime;

    // Use this for initialization


    // Update is called once per frame
    void Update () {
		
	}

    public virtual void ApplyPowerUp()
    {
        FindObjectOfType<ScoreTracker>().powerupSound.Play();
    }
}
