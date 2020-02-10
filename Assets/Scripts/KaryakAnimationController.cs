using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class KaryakAnimationController : MonoBehaviour {

    public UnityArmatureComponent armature;
    public PlayerMovement playerMovement;
    public CharacterController2D characterController2D;
    public KaryakSkills karyakSkills;

    DragonBones.AnimationState runSt = null;
    DragonBones.AnimationState idleSt = null;
    DragonBones.AnimationState invisSt = null;

    private bool isPlaying = false;

    private float speed;
    private float xVelocity;
    private float yVelocity;
    private bool grounded;

    // Use this for initialization
    void Start () {
        armature.animation.Reset();
        armature.animation.Play("idl_eyebrow+glass");
	}

    // Update is called once per frame
    void Update () {
        yVelocity = characterController2D.velocity.y;
        speed = Mathf.Abs(playerMovement.horizontalMove);
        grounded = characterController2D.m_Grounded;

        if (karyakSkills.invisCasts == true && karyakSkills.curState == KaryakSkills.State.Invisible)
        {
            InvisPrepare();
        }
        else if (karyakSkills.curState == KaryakSkills.State.Normal)
        {
            StopInvis();
        }

        if (!grounded && yVelocity < -7 && karyakSkills.curState != KaryakSkills.State.Invisible)
        {
            Fall();
        } else if (grounded)
        {
            isPlaying = false;
        }

        if (speed == 0 && grounded && karyakSkills.curState != KaryakSkills.State.Invisible)
        {
            Idle();
        }
        if (speed != 0 && grounded && karyakSkills.curState != KaryakSkills.State.Invisible)
        {
            Run();
        }
        if (!armature.animation.isPlaying)
        {
            Idle();
        }

    }

    void Idle() {    
        if (idleSt == null)
        {
            armature.animation.timeScale = 1f;
            idleSt = armature.animation.FadeIn("idl_eyebrow+glass", -1.0f, -1, 0);
            idleSt.resetToPose = false;
            runSt = null;
        }
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
    void Fall()
    {
        if (isPlaying)
            return;
        isPlaying = true;
        armature.animation.timeScale = 1;
        armature.animation.FadeIn("fall", -1.0f, 1, 0).resetToPose = false;
    }
    void InvisPrepare()
    {
        invisSt = armature.animation.FadeIn("inv_start", -1.0f, 1, 0);
        invisSt.resetToPose = false;
        Invoke("Invis", armature.animation.lastAnimationState._duration);
    }
    void Invis()
    {
        invisSt = armature.animation.FadeIn("inv_cast", -1.0f, -1, 0);
        invisSt.resetToPose = false;
    }
    void StopInvis()
    {
        armature.animation.Stop("inv_cast");
    }
}
