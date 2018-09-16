using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreTracker : MonoBehaviour {
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI goal;
    public TextMeshProUGUI[] playerProgress;

    //When a winner is decided
    public TextMeshProUGUI winScreen;
    string winner = "";

    bool started = false;

    [System.NonSerialized]
    public int winScore = 2;

    [System.NonSerialized]
    List<string> playerColors = new List<string>() { "red", "green", "blue", "yellow" };
    public Player[] players;
    List<string> dictionary = new List<string>();
    public string currentGoal = "word";

    public PowerUpSpawner powerUpSpawner = null;

    System.Random rd = new System.Random();

    public AudioSource bounceSound;
    public AudioSource keyPressSound;

    // Use this for initialization
	void Start () {
        TextAsset text = Resources.Load<TextAsset>("Words");
        dictionary.AddRange(text.ToString().Split(null));
        Debug.Log("dictionary has " + dictionary.Count);

        RandomizeWord();

        instructions.gameObject.SetActive(true);
        goal.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
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

        foreach (Player eachPlayer in players)
        {
            eachPlayer.ResetProgress();
        }
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
                powerUpSpawner.SetupTimer();
            }
        }
        else
        {
            if (winner == "")
            {
                goal.text = "Current goal: " + currentGoal + "\n";

                for (int i = 0; i < playerProgress.Length; i++)
                {
                    playerProgress[i].text = "<color=\"" + playerColors[i] + "\"" + ">" + players[i].PlayerProgressString() + "</color>\n";
                }
            } else
            {
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene("Long");
                }

            }
        }
        //Display.text = result;
	}

    public void Winner(Player playerWon)
    {
        winner = "Player " + playerWon.playerNumber;

        goal.gameObject.SetActive(false);
        foreach (TextMeshProUGUI eachProgress in playerProgress)
        {
            eachProgress.gameObject.SetActive(false);
        }

        winScreen.text = winner + " won!\nPress any key to restart game";
        winScreen.gameObject.SetActive(true);

        for(int i = 0; i < 100; i++)
        {
            powerUpSpawner.Spawn();
        }

    }
}
