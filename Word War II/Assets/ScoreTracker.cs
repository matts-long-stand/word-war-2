using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour {
    public TextMeshProUGUI goal;
    public TextMeshProUGUI[] playerProgress;

    public int winScore = 5;

    [System.NonSerialized]
    List<string> playerColors = new List<string>() { "red", "green", "blue", "yellow" };
    public Player[] playerComponents;
    List<string> dictionary = new List<string>();
    public string currentGoal = "word";

    System.Random rd = new System.Random();

    // Use this for initialization
	void Start () {
        //foreach (Player playerComponent in FindObjectsOfType<Player>())
        //{
        //    playerComponents.Add(playerComponent);
        //}

        TextAsset text = Resources.Load<TextAsset>("Words");
        dictionary.AddRange(text.ToString().Split(null));
        Debug.Log("dictionary has " + dictionary.Count);

        RandomizeWord();
    }
	
    public void RandomizeWord()
    {
        int wordIndex = rd.Next(dictionary.Count);
        currentGoal = dictionary[wordIndex];
    }

	// Update is called once per frame
	void Update () {
        goal.text = "Current goal: " + currentGoal + "\n";

        for (int i = 0; i < playerProgress.Length; i++)
        {
            playerProgress[i].text = "<color=\"" + playerColors[i] + "\"" + ">" + playerComponents[i].PlayerProgressString() + "</color>\n";
        }

        //Display.text = result;
	}
}
