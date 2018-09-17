using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScoreTracker : MonoBehaviour {
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI goal;
    public TextMeshProUGUI[] playerProgress;
    bool receiveInput = false;

    //When a winner is decided
    public TextMeshProUGUI winScreen;
    string winner = "";

    bool started = false;

    [System.NonSerialized]
    public int winScore = 1;

    [System.NonSerialized]
    List<string> playerColors = new List<string>() { "red", "green", "blue", "yellow" };
    public Player[] players;
    List<string> dictionary = new List<string>();
    public string currentGoal = "word";

    public PowerUpSpawner powerUpSpawner = null;

    System.Random rd = new System.Random();

    public AudioSource bounceSound;
    public AudioSource keyPressSound;

    public GameObject KeysParent;
    public GameObject KeyLightsParent;
    [HideInInspector] public List<GameObject> Keys;
    [HideInInspector] public List<GameObject> KeyLights;

    public bool isReceivingInput()
    {
        return receiveInput;
    }

    // Use this for initialization
	void Start () {
        receiveInput = true;
        for (int i = 0; i < KeysParent.transform.childCount; i++)
        {
            Keys.Add(KeysParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < KeyLightsParent.transform.childCount; i++)
        {
            KeyLights.Add(KeyLightsParent.transform.GetChild(i).gameObject);
        }

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
                if (receiveInput)
                {
                    if (Input.anyKeyDown)
                    {
                        SceneManager.LoadScene("Long");
                    }
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

        winScreen.text = winner + " won!\n";
        winScreen.gameObject.SetActive(true);

        for(int i = 0; i < 100; i++)
        {
            powerUpSpawner.Spawn();
        }

        StartCoroutine(FreezeInput());
        FindObjectOfType<EventSystem>().gameObject.SetActive(false);
    }

    IEnumerator FreezeInput()
    {
        float startTime = Time.realtimeSinceStartup;
        float duration = 0;
        receiveInput = false;
        while (duration < 3f)
        {
            Debug.Log("event system is inactive");
            duration = Time.realtimeSinceStartup - startTime;
            yield return null;
        }

        winScreen.text += "Press any key to restart game";
        receiveInput = true;
    }
}
