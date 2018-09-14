using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour {
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI goal;
    public TextMeshProUGUI[] playerProgress;

    bool started = false;

    public int winScore = 5;

    [System.NonSerialized]
    List<string> playerColors = new List<string>() { "red", "green", "blue", "yellow" };
    public Player[] players;
    List<string> dictionary = new List<string>();
    public string currentGoal = "word";

    System.Random rd = new System.Random();

    // Use this for initialization
	void Start () {
        TextAsset text = Resources.Load<TextAsset>("Words");
        dictionary.AddRange(text.ToString().Split(null));
        Debug.Log("dictionary has " + dictionary.Count);

        RandomizeWord();

        instructions.gameObject.SetActive(true);
        goal.gameObject.SetActive(false);
        foreach (TextMeshProUGUI eachProgress in playerProgress)
        {
            eachProgress.gameObject.SetActive(false);
        }
        foreach (Player eachPlayer in players)
        {
            eachPlayer.gameObject.SetActive(false);
        }
    }
	
    public void RandomizeWord()
    {
        int wordIndex = rd.Next(dictionary.Count);
        currentGoal = dictionary[wordIndex];
    }

    // Update is called once per frame
    void Update() {
        if (!started)
        {
            if (Input.anyKeyDown)
            {
                instructions.gameObject.SetActive(false);
                goal.gameObject.SetActive(true);
                foreach (TextMeshProUGUI eachProgress in playerProgress)
                {
                    eachProgress.gameObject.SetActive(true);
                }
                foreach (Player eachPlayer in players)
                {
                    eachPlayer.gameObject.SetActive(true);
                }

                started = true;
            }
        }
        else
        {
            goal.text = "Current goal: " + currentGoal + "\n";

            for (int i = 0; i < playerProgress.Length; i++)
            {
                playerProgress[i].text = "<color=\"" + playerColors[i] + "\"" + ">" + players[i].PlayerProgressString() + "</color>\n";
            }
        }
        //Display.text = result;
	}
}
