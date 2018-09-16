using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour, ISpawner  {

    public PowerUp[] powerUpTypes;
    public PowerUpSpawnLocation[] spawnLocations;
    public enum SpawnStrategy
    {
        RANDOM
    }
    public SpawnStrategy spawnStrategy;
    public int[] spawnInterval;
    private Timer powerUpTimer = null;
    private static bool spawn = false;
    private System.Random random = null;

	// Use this for initialization
	void Start () {
        random = new System.Random(System.DateTime.Now.Millisecond);
	}
	
	// Update is called once per frame
	void Update () {
        if(spawn)
        {
            Spawn();
            //SetupTimer();
            spawn = false;
        }
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
        int randomLocation = random.Next(0, spawnLocations.Length);
        int randomPowerUp = random.Next(0, powerUpTypes.Length);
        PowerUp powerup = Instantiate(powerUpTypes[randomPowerUp]);
        powerup.transform.position = spawnLocations[randomLocation].transform.position;
    }

    public void SetupTimer()
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);
        int spawnTime = random.Next(spawnInterval[0], spawnInterval[1]);
        powerUpTimer = new Timer(spawnTime * 1000);
        powerUpTimer.Elapsed += OnTimedEvent;
        powerUpTimer.Enabled = true;
    }

    private static void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        spawn = true;
    }
}
