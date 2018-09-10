using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour {
    public TextMeshProUGUI Display;

    [System.NonSerialized]
    List<string> playerColors = new List<string>() { "red", "green", "blue", "yellow" };
    List<Player> playerComponents = new List<Player>();
    string currentGoal = "Word";

    // Use this for initialization
	void Start () {
        foreach (Player playerComponent in FindObjectsOfType<Player>())
        {
            playerComponents.Add(playerComponent);
        }
	}
	
	// Update is called once per frame
	void Update () {
        string result = "Current goal: " + currentGoal + "\n";

        foreach (Player playerComponent in playerComponents)
        {
            int index = playerComponents.IndexOf(playerComponent);
            result += "<color=\"" + playerColors[index] + "\"" + ">Player " + index.ToString() + ": " + playerComponent.getCurrentWord() + "</color>\n";
        }

        Display.text = result;
	}
}
