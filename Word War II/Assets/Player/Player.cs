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

	// Use this for initialization
	void Start () {
        foreach(string s in Input.GetJoystickNames())
        {
            Debug.Log(s);
        }
	}
	
	// Update is called once per frame
	void Update () {

        //Handle any player input
        HandleInput();

        //Handle player movement based off of input
        HandleMovement();
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
        if ((Input.GetAxis("Rotate_X_P1") !=0)  || ((Input.GetAxis("Rotate_Y_P1")) != 0))
        {
            //Rotate the player based on the controller
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("Rotate_X_P1"), Input.GetAxis("Rotate_Y_P1"));
            transform.localRotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }
        //Handle player jumping
        if(Input.GetAxis("Jump_P1") > 0)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 0));
            GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        }
    }
}
