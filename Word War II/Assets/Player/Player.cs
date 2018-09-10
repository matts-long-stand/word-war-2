using Assets.PowerUps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum InputType
    {
        KEYBOARD_MOUSE,
        PS4,
        XBOX360
    };

    public InputType inputType;
    public int playerNumber;
    string currentWord = "";
    int correctLetters = 0;
    List<PowerUp> activePowerups = new List<PowerUp>();
    public bool frozen = false;

	// Use this for initialization
	void Start () {
        foreach(string s in Input.GetJoystickNames())
        {
            Debug.Log(s);
        }
	}
	
    public string getCurrentWord()
    {
        return currentWord;
    }

	// Update is called once per frame
	void Update () {

        if(!frozen)
        {
            //Handle any player input
            HandleInput();

            //Handle player movement based off of input
            HandleMovement();
        }
	}

    private void HandleInput()
    {
        switch(inputType)
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
        if ((Input.GetAxis("Rotate_X_P" + playerNumber.ToString()) !=0)  || ((Input.GetAxis("Rotate_Y_P" + playerNumber.ToString())) != 0))
        {
            //Rotate the player based on the controller
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("Rotate_X_P" + playerNumber.ToString()), Input.GetAxis("Rotate_Y_P" + playerNumber.ToString()));
            transform.localRotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
        //Handle player jumping
        if(Input.GetAxis("Jump_P" + playerNumber.ToString()) > 0)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0));
            GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //We only want to handle collision on the keyboard layer (keys)
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Keyboard"))
        {
            //Check if we got the next letter
            if(ScoreTracker.currentGoal[correctLetters].Equals(collision.gameObject.name.ToLower()[0])) {
                currentWord += collision.gameObject.name.ToLower();
                correctLetters++;
            }

            //Check if we completed the word
            if(ScoreTracker.currentGoal.Equals(currentWord))
            {
                currentWord = "";
                correctLetters = 0;
            }
        }

        //Handle powerup collisions
        else if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Powerup"))
        {
            collision.collider.gameObject.transform.parent.GetComponent<PowerUp>().owner = gameObject.transform.parent.name;
            activePowerups.Add(collision.collider.gameObject.transform.parent.GetComponent<PowerUp>());
            collision.collider.gameObject.transform.parent.GetComponent<PowerUp>().ApplyPowerUp();
            Destroy(collision.collider.gameObject.GetComponentInChildren<MeshRenderer>().gameObject);
            Behaviour halo1 = collision.collider.gameObject.GetComponent("Halo") as Behaviour;
            //gameObject.AddComponent(halo1);
            Behaviour halo = GetComponentInChildren<SphereCollider>().gameObject.GetComponent("Halo") as Behaviour;
            halo.enabled = true;
        }

        else if(collision.collider.gameObject.tag.Equals("Player"))
        {
            foreach(PowerUp powerUp in activePowerups) {
                if(powerUp is ExtraBouncePowerup)
                {
                    collision.collider.attachedRigidbody.AddForce(transform.forward * 2500);
                }
            }
        }
    }
}
