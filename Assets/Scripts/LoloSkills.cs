using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoloSkills : MonoBehaviour {
    public PlayerMovement playerMovement;
    public CharacterController2D characterController2D;
    public CharacterSelect characterSelect;

    [Header("Key Pick Up")]
    public bool haveKey = false;
    public GameObject key;
    [Header("Silent Walk")]
    public float slowedSpeed;
    public float normalRunSpeed = 30;

    float noJumpForce = 0f;
    float jumpForce;

    public float timeForSlowWalk = 5f;
    public float curTime = 0f;

    public bool readyToUse = true;
    public enum State { Normal, SilentWalk };
    public State curState;
    [Space]
    public ParticleSystem legsPartSys;
    private ParticleSystem.EmissionModule emission;

    private void Awake()
    {
        emission = legsPartSys.emission;
    }

    void Start()
    {
        //normalRunSpeed = playerMovement.runSpeed;
        //slowedSpeed = normalRunSpeed / 2;

        jumpForce = characterController2D.m_JumpForce;
        curState = State.Normal;
    }

    // Update is called once per frame
    void Update () {
        key.gameObject.SetActive(haveKey);

        if (Input.GetKeyDown(KeyCode.F) && readyToUse)
        {
            curState = State.SilentWalk;
        }

        switch (curState)
        {
            case State.SilentWalk:
                SilentWalk();
                break;
            case State.Normal:
                NormalState();
                break;
        }

        if (Mathf.Abs(playerMovement.horizontalMove) != 0 && !playerMovement.jump && curState == State.Normal)
        {
            emission.enabled = true;
        }
        else
            emission.enabled = false;
    }

    void SilentWalk()
    {
        readyToUse = false;
        if (curTime < timeForSlowWalk)
        {
            curTime += Time.deltaTime;
            playerMovement.runSpeed = slowedSpeed;
            characterController2D.m_JumpForce = noJumpForce;
            characterSelect.canSelectCharacter = false;
        }
        else
        {
            curState = State.Normal;
        }
    }

    void NormalState()
    {
        characterSelect.canSelectCharacter = true;
        playerMovement.runSpeed = normalRunSpeed;
        characterController2D.m_JumpForce = jumpForce;
        curTime = 0f;
        readyToUse = true;
    }

}
