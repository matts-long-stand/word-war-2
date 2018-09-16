using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    public GameObject[] playersToSpawn;
    public PlayerSpawnLocation[] spawnLocations;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            Spawn(1);
        }
    }

    public void Spawn(int playerNumber)
    {
        switch (spawnStrategy)
        {
            case SpawnStrategy.RANDOM:
                SpawnRandom(playerNumber);
                break;
        }
    }

    private void SpawnRandom(int playerNumber)
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);
        int randomLocation = random.Next(0, spawnLocations.Length);
        Vector3 newPosition = spawnLocations[randomLocation].transform.position;
        newPosition.y += 5;
        playersToSpawn[playerNumber - 1].GetComponentInChildren<Player>().transform.position = newPosition;
    }

}
