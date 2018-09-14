using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour, ISpawner  {

    public PowerUp[] powerUpTypes;
    public PowerUpSpawnLocation[] spawnLocations;
    public enum SpawnStrategy
    {
        RANDOM
    }
    public SpawnStrategy spawnStrategy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S))
        {
            Spawn();
        }
	}

    public void Spawn()
    {
        switch(spawnStrategy)
        {
            case SpawnStrategy.RANDOM:
                SpawnRandom();
                break;
        }
    }

    private void SpawnRandom()
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);
        int randomLocation = random.Next(0, spawnLocations.Length);
        int randomPowerUp = random.Next(0, powerUpTypes.Length);
        PowerUp powerup = Instantiate(powerUpTypes[randomPowerUp]);
        powerup.transform.position = spawnLocations[randomLocation].transform.position;
    }
}
