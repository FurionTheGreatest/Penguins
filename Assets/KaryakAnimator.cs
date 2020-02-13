using System.Collections;
using UnityEngine;
using DragonBones;

public class KaryakAnimator : MonoBehaviour
{
    /// <summary>
    /// scripts with character values needed to check
    /// </summary>
    private PlayerMovement playerMovement;
    private CharacterController2D characterController2D;
    private KaryakSkills karyakSkills;
    /// <summary>
    /// names of animations
    /// </summary>
    private static readonly string idleAnim = "idl_eyebrow+glass";
    private static readonly string runAnim = "run";
    private static readonly string fallAnim = "fall";
    private static readonly string invAnim = "inv_cast";
    /// <summary>
    /// animation states
    /// </summary>
    enum State { NULL, IDLE, WALKING, FALLING, INVISIBILITY_START, INVISIBILITY_CAST }
    private State state = State.NULL;
    private UnityArmatureComponent armatureComponent;
    /// <summary>
    /// storing values of other scripts
    /// </summary>
    private float speed;
    private float yVelocity;
    private bool grounded;

    void Start()
    {
        armatureComponent = transform.GetComponentInChildren<UnityArmatureComponent>();
        playerMovement = transform.GetComponent<PlayerMovement>();
        characterController2D = transform.GetComponent<CharacterController2D>();
        karyakSkills = transform.GetComponent<KaryakSkills>();
    }

    void Update()
    {
        //get some values for condition check
        yVelocity = characterController2D.velocity.y;
        speed = Mathf.Abs(playerMovement.horizontalMove);
        grounded = characterController2D.m_Grounded;

        if (speed == 0 && grounded && karyakSkills.curState != KaryakSkills.State.Invisible)
            Idle();
        if (!armatureComponent.animation.isPlaying)
            state = State.NULL;
        if (speed != 0 && grounded && karyakSkills.curState != KaryakSkills.State.Invisible)
            Run(speed / 7);
        if (!grounded && yVelocity < -7 && karyakSkills.curState != KaryakSkills.State.Invisible)
            Fall();
        if (karyakSkills.curState == KaryakSkills.State.Invisible)
        {
            Inv();
        }        
    }
    #region Animations
    public void Idle()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.FadeIn(idleAnim);
            armatureComponent.animation.timeScale = 1f;
            state = State.IDLE;
        }
    }

    public void Run(float speed)
    {
        if (speed > 0 && transform.lossyScale.x > 0 || speed < 0 && transform.lossyScale.x < 0)
        {
            if (state != State.WALKING)
            {
                armatureComponent.animation.FadeIn(runAnim, 0.25f, -1);
                state = State.WALKING;
            }
            armatureComponent.animation.timeScale = speed;
        }
        else if (speed < 0 && transform.lossyScale.x > 0 || speed > 0 && transform.lossyScale.x < 0)
        {
            if (state != State.WALKING)
            {
                armatureComponent.animation.FadeIn(runAnim, 0.25f, -1);
                state = State.WALKING;
            }
            armatureComponent.animation.timeScale = -speed;
        }
    }
    public void Fall ()
    {
        if (state != State.FALLING)
        {
            armatureComponent.animation.FadeIn(fallAnim, 0, 1);
            armatureComponent.animation.timeScale = 2f;
            state = State.FALLING;
        }
    }
    /*public void Invisibility()
    {
        if (state != State.INVISIBILITY_START)
        {
            armatureComponent.animation.FadeIn(invStartAnim, 0, 1);
            armatureComponent.animation.timeScale = 1f;
            Invoke("Inv", armatureComponent.animation.lastAnimationState._duration);
            state = State.INVISIBILITY_START;
        }
    }*/
    void Inv()
    {
        if (state != State.INVISIBILITY_CAST)
        {
            armatureComponent.animation.FadeIn(invAnim, -1.0f, -1, 0);
            armatureComponent.animation.timeScale = 1;
            state = State.INVISIBILITY_CAST;
        }
    }
    #endregion
}
