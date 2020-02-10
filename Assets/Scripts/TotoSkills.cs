using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotoSkills : MonoBehaviour {
    public CharacterController2D controller;
    public CharacterSelect characterSelect;
    public PlayerMovement playerMovement;

    [Header("Dash")]
    public float maxDashTime = 2f;
    public float dashSpeed;
    public float currentDashSpeed;
    public float currentDashTime;
    [Range(0, .3f)] public float dashSmoothing;

    public bool dashTimeEnded = false;
    public bool dashKeyPressed = false;
    public bool dashKeyReleased = false;


    public bool dash = false;
    [Space]
    public ParticleSystem legsPartSys;
    private ParticleSystem.EmissionModule emission;

    public Rigidbody2D rb;
    private Vector3 m_Velocity = Vector3.zero;

    //AudioManager audioManager;
    private void Awake()
    {
        emission = legsPartSys.emission;        
    }
    // Update is called once per frame
    /*private void Start()
    {
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
    }*/
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyDown(KeyCode.F))
        {
            DashCanceled();
            dashKeyReleased = true;
        }
        else dashKeyReleased = false;

        if (Input.GetKeyDown(KeyCode.F))
        {
            dashKeyPressed = true;
        }
        else dashKeyPressed = false;

        if (Mathf.Abs(playerMovement.horizontalMove) != 0 && !playerMovement.jump)
        {
            emission.enabled = true;
        }
        else
            emission.enabled = false;

    }
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.F))
        {
            if (currentDashTime < maxDashTime && controller.m_Grounded)
            {
                Dash();
            }
            else
            {
                EndOfDash();
            }
        }
    }

    void Dash()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            //audioManager.PlaySound("Roll");
            dash = true;
            characterSelect.canSelectCharacter = false;
            playerMovement.runSpeed = 0;
            currentDashSpeed = dashSpeed;
            currentDashSpeed += Time.deltaTime;
            currentDashTime += Time.deltaTime;

            Vector3 targetVel = new Vector2(gameObject.GetComponent<Transform>().localScale.x * dashSpeed, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity * Time.deltaTime, targetVel, ref m_Velocity, dashSmoothing);
        }
    }

    void EndOfDash()
    {
        //audioManager.StopSound("Roll");
        dash = false;
        currentDashSpeed = 0;
        characterSelect.canSelectCharacter = true;

        dashTimeEnded = true;
    }
    void DashCanceled()
    {
        dash = false;
        currentDashSpeed = 0.0f;
    }
}
