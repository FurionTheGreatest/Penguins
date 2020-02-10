using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class YushaAnimationController : MonoBehaviour
{
    public UnityArmatureComponent armature;
    public PlayerMovement playerMovement;
    public CharacterController2D characterController2D;
    public YushaSkills yushaSkills;

    DragonBones.AnimationState runSt = null;
    DragonBones.AnimationState idleSt = null;
    //DragonBones.AnimationState walkSt = null;

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
        //Debug.Log(armature.animation.isPlaying);
        /*if (!grounded && yVelocity < -7 )
        {
            Jump();
        }*/
        if (speed == 0 && grounded )
        {
            Idle();
        }
        if (speed != 0 && grounded)
        {
            Run();
        }
        /*if (yVelocity > 0.5f && !yushaSkills.isFlying)
        {
            Jump();
        }*/
        if (yushaSkills.isFlying)
        {
            if (!armature.animation.lastAnimationName.Equals("zont_fly"))
                OpenUmbrella();
        }
        else
        {            
            if (!armature.animation.isPlaying)
            {
                Idle();
            }
        }
    }
    void PlayIdle()
    {
        if (idleSt == null)
        {
            float random = Random.value;
            if (random < 0.5f)
            {
                OpenUmbrella();
            }
            if (random > 0.5f)
            {
                Idle();
            }
        }
    }
    void Idle()
    {
        if (idleSt == null)
        {
            armature.animation.timeScale = 1f;
            idleSt = armature.animation.FadeIn("idl", -1.0f, -1, 0);
            idleSt.resetToPose = false;
            runSt = null;
        }
    }
    void OpenUmbrella()
    {
        if (!armature.animation.lastAnimationName.Equals("zont_open") && !grounded)
        {
            armature.animation.timeScale = 2f;
            idleSt = armature.animation.FadeIn("zont_open", -1.0f, 1, 0);
            idleSt.resetToPose = false;
        }
        Invoke("Fly", armature.animation.lastAnimationState._duration/2);
    }
    void CloseUmbrella()
    {
        if (!armature.animation.lastAnimationName.Equals("zont_open"))
        {
            armature.animation.timeScale = -2f;
            idleSt = armature.animation.FadeIn("zont_open", -1.0f, 1, 0);
            idleSt.resetToPose = false;
        }
    }
        void IdleRare()
    {
        armature.animation.timeScale = 1;
        idleSt = armature.animation.FadeIn("zont_idl", -1.0f, -1, 0);
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
    /*void Fall()
    {
        if (isPlaying)
            return;
        isPlaying = true;
        armature.animation.timeScale = 1;
        armature.animation.FadeIn("fall", -1.0f, 1, 0).resetToPose = false;
    }*/

    public void Jump()
    {
        if (!grounded)
            return;
        armature.animation.timeScale = 2;
        armature.animation.FadeIn("jump", -1.0f, -1, 0);
        idleSt = null;
        runSt = null;
    }
    void Fly()
    {
        if (grounded)
            return;
        else
        {
            armature.animation.timeScale = 2;
            armature.animation.FadeIn("zont_fly", -1.0f, -1, 0);
            idleSt = null;
            runSt = null;
        }
    }
}
