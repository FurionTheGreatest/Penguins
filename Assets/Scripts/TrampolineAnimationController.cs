using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class TrampolineAnimationController : MonoBehaviour
{
    public UnityArmatureComponent armature;
    public TrampolineForceController forceController;

    //DragonBones.AnimationState runSt = null;
    DragonBones.AnimationState idleSt = null;
    DragonBones.AnimationState forceSt = null;

    bool forcePlayer;
    void Start()
    {
        armature.animation.Reset();
        armature.animation.Play("idl");        
    }

    // Update is called once per frame
    void Update()
    {
        forcePlayer = forceController.onTop;

        if (forceSt == null)
         PlayIdle();

         if (!armature.animation.isPlaying)        
             PlayIdle();

        if (forcePlayer)
            Force();
    }


    void PlayIdle()
    {
        if (idleSt == null)
        {
            float random = Random.value;
            if (random < 0.2f)
            {
                Idle1();
            }
            if (random > 0.2f)
            {
                Idle2();
            }
        }
    }

    void Idle1()
    {
        forceSt = null;
        //runSt = null;
        armature.animation.timeScale = 1;
        idleSt = armature.animation.FadeIn("idl", -1.0f, -1, 0);
        idleSt.resetToPose = false;
    }

    void Idle2()
    {
        forceSt = null;
        //runSt = null;
        armature.animation.timeScale = 1;
        idleSt = armature.animation.FadeIn("idl2", -1.0f, -1, 0);
        idleSt.resetToPose = false;
    }

    public void Force()
    {
        idleSt = null;
        if (forceSt == null)
        {
            forceSt = armature.animation.FadeIn("batut", -1.0f, 1, 0);
            forceSt.resetToPose = false;
        }
    }
}
