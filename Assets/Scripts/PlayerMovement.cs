﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FixedJoystick))]
public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;
    public float runSpeed = 40f;
    public bool jump = false;
    public float horizontalMove = 0f;
    public static PlayerMovement instance;

    private FixedJoystick fixedJoystick;
    //public AudioManager audioManager;

    //bool played = false;
    //float walkCD = 0.5f;
    private float _jostickPositionToJump = 0.4f;

    private void Start()
    {
        /*if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }*/
        if (fixedJoystick == null)
            fixedJoystick = FindObjectOfType<FixedJoystick>();
    }

    void Update () {
        /**switch (runSpeed)
        {
            case 22: walkCD = 0.5f;
                break;
            case 25:
                walkCD = 0.4f;
                break;
            case 30:
                walkCD = 0.3f;
                break;
        }*/

        

        if (Input.GetButtonDown("Jump") || fixedJoystick.Vertical > 0.5f)
        {
            if (controller.m_JumpForce != 0)
            {
                jump = true;
                //if(controller.m_Grounded)
                //audioManager.PlaySound("Jump");
            }
        }


#if UNITY_ANDROID
        horizontalMove = fixedJoystick.Horizontal * runSpeed;
#elif UNITY_EDITOR
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
#endif

        /*if (Mathf.Abs(horizontalMove) > 0 && controller.m_Grounded)
        {
            if(!played)
                StartCoroutine(CD());
    }*/
        if (Input.GetButtonDown("Jump") || fixedJoystick.Vertical > _jostickPositionToJump)
        {
            if(controller.m_JumpForce != 0)
            {
                jump = true;
                //if(controller.m_Grounded)
                    //audioManager.PlaySound("Jump");
            }
                
        }
    }
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime ,false,jump);
        jump = false;               
    }
    /*IEnumerator CD()
    {
        played = true;
        //audioManager.PlaySound("Walk");
        yield return new WaitForSeconds(walkCD);
        //audioManager.StopSound("Walk");
        played = false;
    }*/
}
