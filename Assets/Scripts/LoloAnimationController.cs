using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class LoloAnimationController : MonoBehaviour
{
    public UnityArmatureComponent armature;
    public PlayerMovement playerMovement;
    public CharacterController2D characterController2D;
    public LoloSkills loloSkills;

    DragonBones.AnimationState runSt = null;
    DragonBones.AnimationState idleSt = null;
    DragonBones.AnimationState walkSt = null;

    private float speed;
    private float xVelocity;
    private float yVelocity;
    private bool grounded;
    private bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        armature.animation.Reset();
        armature.animation.Play("idl");
    }

    // Update is called once per frame
    void Update()
    {
        xVelocity = Mathf.Abs(characterController2D.velocity.x);
        yVelocity = characterController2D.velocity.y;

        speed = Mathf.Abs(playerMovement.horizontalMove);
        grounded = characterController2D.m_Grounded;
        isJumping = playerMovement.jump;
        if (!armature.animation.isPlaying)
        {
            Idle();
        }
        if (speed != 0 && loloSkills.curState == LoloSkills.State.SilentWalk)
        {
            if (walkSt == null)
                SilentWalk();
            else return;
        } else if (speed == 0 && loloSkills.curState == LoloSkills.State.SilentWalk)
        {
            SilentWalk();
        }
        else if (loloSkills.curState == LoloSkills.State.Normal)
        {
            StopSilentWalk();
        }

        if (!grounded && yVelocity < -7 && loloSkills.curState != LoloSkills.State.SilentWalk)
        {
            //Fall();
        }

        if (speed == 0 && grounded && loloSkills.curState != LoloSkills.State.SilentWalk)
        {
            PlayIdle();
        }
        if (speed != 0 && grounded && loloSkills.curState != LoloSkills.State.SilentWalk)
        {
            Run();
        }
        if (yVelocity > 0.5f)
        {
            Jump();
        }
    }
    void PlayIdle()
    {
        if (idleSt == null)
        {
            float random = Random.value;
            if (random < 0.1f)
            {
                IdleRare();
            }
            if (random > 0.1f)
            {
                Idle();
            }
        }
    }
    void Idle()
    {        
        armature.animation.timeScale = 1f;
        idleSt = armature.animation.FadeIn("idl", -1.0f, -1, 0);
        idleSt.resetToPose = false;
        runSt = null;        
    }
    void IdleRare()
    {
        armature.animation.timeScale = 1;
        idleSt = armature.animation.FadeIn("idl_rare", -1.0f, 1, 0);
        idleSt.resetToPose = false;
        runSt = null;
        PlayIdle();
    }
    void Run()
    {
        if (runSt == null)
        {
            armature.animation.timeScale = 2;
            runSt = armature.animation.FadeIn("run", -1.0f, -1, 0);
            runSt.resetToPose = false;
            idleSt = null;
        }
    }
    /*void Fall()
    {
        if (isPlaying)
            return;
        isPlaying = true;
        armature.animation.timeScale = 1;
        armature.animation.FadeIn("fall", -1.0f, 1, 0).resetToPose = false;
    }*/
    void SilentWalk()
    {
        walkSt = armature.animation.FadeIn("run_tapki", -1.0f, -1, 0);
        walkSt.resetToPose = false;

        //Invoke("Invis", armature.animation.lastAnimationState._duration);
    }
    void StopSilentWalk()
    {
        armature.animation.Stop("run_tapki");
    }
    public void Jump()
    {
        if (!grounded)
            return;
        armature.animation.timeScale = 2;
        armature.animation.FadeIn("jump", -1.0f, 1, 0);
        idleSt = null;
    }

}
