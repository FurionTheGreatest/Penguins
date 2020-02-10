using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class TotoAnimationController : MonoBehaviour
{

    public UnityArmatureComponent armature;
    public PlayerMovement playerMovement;
    public CharacterController2D characterController2D;
    public TotoSkills totoSkills;

    public ParticleSystem[] snowBall;

    DragonBones.AnimationState runSt = null;
    DragonBones.AnimationState idleSt = null;
    DragonBones.AnimationState rollSt = null;

    private float speed;
    private float xVelocity;
    private float yVelocity;
    private bool isJumping;
    private bool isRolling;
    private bool grounded;


    private float normalRunSpeed;
    private float disabledRunSpeed = 0f;


    void Start()
    {
        normalRunSpeed = playerMovement.runSpeed;
        armature.animation.Reset();
        //armature._colorTransform.alphaMultiplier = 0.5f;
        armature.animation.Play("idle");
        foreach (ParticleSystem ps in snowBall)
        {
#pragma warning disable CS0618 // Тип или член устарел
            ps.enableEmission = false;
#pragma warning restore CS0618 // Тип или член устарел
        }
    }
    // Update is called once per frame
    void Update()
    {
        grounded = characterController2D.m_Grounded;
        isJumping = playerMovement.jump;

        xVelocity = Mathf.Abs(characterController2D.velocity.x);
        yVelocity = characterController2D.velocity.y;

        isRolling = totoSkills.dash;

        if (totoSkills.dashTimeEnded || !isRolling)
        {
            StopRolling();
        }

        if (totoSkills.dashKeyPressed && !totoSkills.dashTimeEnded)
        {
            playerMovement.runSpeed = disabledRunSpeed;
            RollPrepare();
        }

        if (totoSkills.dashKeyReleased)
        {
            playerMovement.runSpeed = normalRunSpeed;
            rollSt = null;
            if (idleSt == null)
            {
                StopRolling();
            }
        }

        speed = Mathf.Abs(playerMovement.horizontalMove);

        if (speed == 0 && grounded)
        {
            PlayIdle();
        }
        if (speed != 0 && grounded)
        {
            Run();
        }
        /*if (xVelocity < 0.5f && grounded) dash also depends on velocity
        {
            PlayIdle();
        }
        if (xVelocity > 0.5f && grounded)
        {
            Run();
        }*/

        if (yVelocity > 0.5f)
        {
            Jump();
        }

        if (!armature.animation.isPlaying)
        {
            Idle();
        }
    }

    void PlayIdle()
    {
        if (idleSt == null)
        {
            float random = Random.value;
            if (random < 0.2f)
            {
                IdleWithBlink();
            }
            if (random > 0.2f)
            {
                Idle();
            }
        }
    }

    void IdleWithBlink()
    {
        armature.animation.timeScale = 1;
        idleSt = armature.animation.FadeIn("idle", -1.0f, 1, 0);
        idleSt.resetToPose = false;
        runSt = null;
    }

    void Idle()
    {
        armature.animation.timeScale = 1;
        idleSt = armature.animation.FadeIn("idle_no_clip", -1.0f, -1, 0);
        idleSt.resetToPose = false;
        runSt = null;
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

    public void Jump()
    {
        if (!grounded)
            return;
        armature.animation.timeScale = 2;
        armature.animation.FadeIn("jump", -1.0f, 1, 0).resetToPose = false;
    }

    void RollPrepare()
    {
        rollSt = armature.animation.FadeIn("penetration_start", -1.0f, 1, 0);
        rollSt.resetToPose = false;
        Invoke("Roll", armature.animation.lastAnimationState._duration);
    }
    void Roll()
    {
        foreach (ParticleSystem ps in snowBall)
        {
#pragma warning disable CS0618 // Тип или член устарел
            ps.enableEmission = true;
#pragma warning restore CS0618 // Тип или член устарел
        }
        rollSt = armature.animation.FadeIn("penetration_cast", -1.0f, -1, 0);
        rollSt.resetToPose = false;
    }

    void StopRolling()
    {
        foreach (ParticleSystem ps in snowBall)
        {
#pragma warning disable CS0618 // Тип или член устарел
            ps.enableEmission = false;
#pragma warning restore CS0618 // Тип или член устарел
        }
        armature.animation.Stop("penetration_cast");
    }

}
