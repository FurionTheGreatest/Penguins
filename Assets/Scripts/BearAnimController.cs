using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class BearAnimController : MonoBehaviour
{
    public UnityArmatureComponent armature;
    public PolarBearController bearController;
    public ParticleSystem zParticle;

    DragonBones.AnimationState rageSt = null;
    //DragonBones.AnimationState idleSt = null;
    // Start is called before the first frame update
    void Start()
    {
        armature.animation.Play("idl");
    }

    // Update is called once per frame
    void Update()
    {
        if (bearController.wakedUp)
        {
#pragma warning disable CS0618 // Тип или член устарел
            zParticle.enableEmission = false;
#pragma warning restore CS0618 // Тип или член устарел
            WakeUpRage();
        }
    }

    void WakeUpRage()
    {
        if (rageSt == null)
        {
            rageSt = armature.animation.FadeIn("rage", -1.0f, 1, 0);
            rageSt.resetToPose = false;
        }
    }
}
