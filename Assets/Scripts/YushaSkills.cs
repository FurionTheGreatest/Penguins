using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YushaSkills : MonoBehaviour {

    public CharacterController2D controller;
    public PlayerMovement playerMovement;

    [Header("Parachute")]
    public float gravityValForParachute = 0.3f;
    public float standGravVal = 3;
    public bool isFlying = false;
    [Space]
    public ParticleSystem legsPartSys;
    private ParticleSystem.EmissionModule emission;

    private Rigidbody2D rb;
    // Use this for initialization
    private void Awake()
    {
        emission = legsPartSys.emission;
    }
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        if (Mathf.Abs(playerMovement.horizontalMove) != 0 && !playerMovement.jump)
        {
            emission.enabled = true;
        }
        else
            emission.enabled = false;
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (!controller.m_Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("fly");
                rb.gravityScale = gravityValForParachute;
                isFlying = true;
            }
        }   
        if (controller.m_Grounded)
        {
            rb.gravityScale = standGravVal;
            isFlying = false;
        }            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ceiling"))
        {
            rb.gravityScale = standGravVal;
            isFlying = false;
        }
    }
}
