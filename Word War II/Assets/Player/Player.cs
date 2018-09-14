using Assets.PowerUps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public enum InputType
    {
        KEYBOARD_MOUSE,
        PS4,
        XBOX360
    };

    public InputType inputType;
    public int playerNumber;

    List<GameObject> currentColliders = new List<GameObject>();
    List<PowerUp> activePowerups = new List<PowerUp>();
    public bool frozen = false;

    //For movement
    float initialForce = 600;
    float forceInterval = 35;
    int jumpSteps = 0;
    int maxJumpSteps = 15;

    //For score tracking
    string currentWord = "";
    int correctLetters = 0;
    int currentScore = 0;

    // Use this for initialization
    void Start()
    {
        foreach (string s in Input.GetJoystickNames())
        {
            Debug.Log(s);
        }
    }

    public string getCurrentWord()
    {
        return currentWord;
    }

    // Update is called once per frame
    void Update()
    {

        if (!frozen)
        {
            //Handle any player input
            HandleInput();

            //Handle player movement based off of input
            HandleMovement();
        }
    }

    private void HandleInput()
    {
        switch (inputType)
        {
            case InputType.KEYBOARD_MOUSE:
                HandleKeyboardMouseInput();
                break;
            case InputType.PS4:
                HandlePS4Input();
                break;
            case InputType.XBOX360:
                HandleXbox360Input();
                break;
        }
    }

    private void HandleKeyboardMouseInput()
    {

    }

    private void HandlePS4Input()
    {

    }

    private void HandleXbox360Input()
    {

    }

    private void HandleMovement()
    {
        if ((Input.GetAxis("Rotate_X_P" + playerNumber.ToString()) != 0) || ((Input.GetAxis("Rotate_Y_P" + playerNumber.ToString())) != 0))
        {
            //Rotate the player based on the controller
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("Rotate_X_P" + playerNumber.ToString()), Input.GetAxis("Rotate_Y_P" + playerNumber.ToString()));
            transform.localRotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }

        //Handle player jumping
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if (Input.GetAxis("Jump_P" + playerNumber.ToString()) > 0)
        {
            Debug.Log(currentColliders.Count + " colliders");
            if ((jumpSteps == 0) && (currentColliders.Count > 0))
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, initialForce, 0));
                Debug.Log("Applied " + initialForce);
                GetComponent<Rigidbody>().AddForce(transform.forward * 50);
                jumpSteps++;
            }
            else if ((jumpSteps > 0) && (jumpSteps < maxJumpSteps) && (currentColliders.Count == 0))
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, forceInterval, 0));
                Debug.Log("Applied " + forceInterval);
                GetComponent<Rigidbody>().AddForce(transform.forward * 5);
                jumpSteps++;
            }

            //if (transform.localPosition.y <= (GetComponent<MeshRenderer>().bounds.extents.y/2))
        }
        else
        {
            jumpSteps = 0;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        currentColliders.Remove(collision.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        ScoreTracker scoreTracker = FindObjectOfType<ScoreTracker>();

        if (!currentColliders.Contains(collision.gameObject))
        {
            currentColliders.Add(collision.gameObject);
        }

        //We only want to handle collision on the keyboard layer (keys)
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Keyboard"))
        {
            //Check if we got the next letter
            if (scoreTracker.currentGoal[correctLetters].Equals(collision.gameObject.name.ToLower()[0]))
            {
                currentWord += collision.gameObject.name.ToLower();
                correctLetters++;
            }

            //Check if we completed the word
            if (scoreTracker.currentGoal.Equals(currentWord))
            {
                currentWord = "";
                correctLetters = 0;
                currentScore += 1;
                scoreTracker.RandomizeWord();
            }
        }

        //Handle powerup collisions
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Powerup"))
        {
            collision.collider.gameObject.transform.parent.GetComponent<PowerUp>().owner = gameObject.name;
            activePowerups.Add(collision.collider.gameObject.transform.parent.GetComponent<PowerUp>());
            collision.collider.gameObject.transform.parent.GetComponent<PowerUp>().ApplyPowerUp();
            Destroy(collision.collider.gameObject.GetComponentInChildren<MeshRenderer>().gameObject);
            Behaviour halo1 = collision.collider.gameObject.GetComponent("Halo") as Behaviour;
            //gameObject.AddComponent(halo1);
            Behaviour halo = GetComponentInChildren<SphereCollider>().gameObject.GetComponent("Halo") as Behaviour;
            halo.enabled = true;
        }

        else if (collision.collider.gameObject.tag.Equals("Player"))
        {
            foreach (PowerUp powerUp in activePowerups)
            {
                if (powerUp is ExtraBouncePowerup)
                {
                    collision.collider.attachedRigidbody.AddForce(transform.forward * 2500);
                }
            }
        }
    }

    public string PlayerProgressString()
    {
        string result = "";
        result += "Player " + playerNumber + "\n";
        result += "Score: " + currentScore + "\n";
        result += "Progress: " + currentWord + "\n";

        return result;
    }
}
