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
    string currentWord = "";
    int correctLetters = 0;
    List<GameObject> currentColliders = new List<GameObject>();
    List<PowerUp> activePowerups = new List<PowerUp>();
    public bool frozen = false;
    float initialForce = 500;
    float forceAdded = 0;
    float capForce = 1000;
    float forceInterval = 100;
    bool jumpedOnObject = false;
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

    void CanBounceAgain()
    {
        forceAdded = 0;

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
            if (currentColliders.Count > 0)
            {
                jumpedOnObject = true;
            }

            //Debug.Log(velocity.x + " " + velocity.y + " " + velocity.z);
            Debug.Log(Input.GetAxis("Jump_P" + playerNumber.ToString()));
            //if (transform.localPosition.y <= (GetComponent<MeshRenderer>().bounds.extents.y/2))
            if (jumpedOnObject)
            {
                if (forceAdded == 0)
                {
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, initialForce, 0));
                    Debug.Log("Applied " + initialForce);
                    forceAdded += forceInterval;
                    GetComponent<Rigidbody>().AddForce(transform.forward * 10);
                }
                else if (forceAdded > 0)
                {
                    if ((forceAdded + initialForce) < capForce)
                    {
                        GetComponent<Rigidbody>().AddForce(new Vector3(0, forceAdded, 0));
                        Debug.Log("Applied " + forceAdded);
                        forceAdded += forceInterval;
                        GetComponent<Rigidbody>().AddForce(transform.forward * 10);
                    }
                }
            }
        }

        if ((velocity.y == 0) && (currentColliders.Count > 0))
        {
            CanBounceAgain();
        }

    }

    void OnCollisionExit(Collision collision)
    {
        currentColliders.Remove(collision.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {


        //We only want to handle collision on the keyboard layer (keys)
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Keyboard"))
        {
            currentColliders.Add(collision.gameObject);
            jumpedOnObject = false;
            //Reset the ability to jump
            CanBounceAgain();

            //Check if we got the next letter
            if (ScoreTracker.currentGoal[correctLetters].Equals(collision.gameObject.name.ToLower()[0]))
            {
                currentWord += collision.gameObject.name.ToLower();
                correctLetters++;
            }

            //Check if we completed the word
            if (ScoreTracker.currentGoal.Equals(currentWord))
            {
                currentWord = "";
                correctLetters = 0;
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
}
