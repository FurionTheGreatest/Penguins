using System.Collections;
using UnityEngine;
using DragonBones;

public class YushaAnimator : MonoBehaviour
{
    /// <summary>
    /// scripts with character values needed to check
    /// </summary>
    private PlayerMovement playerMovement;
    private CharacterController2D characterController2D;
    private YushaSkills yushaSkills;
    /// <summary>
    /// names of animations
    /// </summary>
    private static readonly string idleAnim = "idl";
    private static readonly string idleUmbrAnim = "zont_idl";
    private static readonly string runAnim = "run";
    private static readonly string runUmbrAnim = "zont_run";
    private static readonly string jumpAnim = "jump";
    private static readonly string jumpUmbrAnim = "zont_jump";
    private static readonly string openUmbrAnim = "zont_open";
    private static readonly string flyUmbrAnim = "zont_fly";
    /// <summary>
    /// animation states
    /// </summary>
    enum State { NULL, IDLE, IDLE_UMBR, WALKING, JUMPING, UMBR_OPEN, UMBR_FLY }
    private State state = State.NULL;
    private UnityArmatureComponent armatureComponent;
    /// <summary>
    /// storing values of other scripts
    /// </summary>
    private float speed;
    private float yVelocity;
    private bool grounded;
    private float openUmbrellaDur;
    DragonBones.AnimationState animationState;
    void Start()
    {
        armatureComponent = transform.GetComponentInChildren<UnityArmatureComponent>();
        playerMovement = transform.GetComponent<PlayerMovement>();
        characterController2D = transform.GetComponent<CharacterController2D>();
        yushaSkills = transform.GetComponent<YushaSkills>();

        armatureComponent.animation.FadeIn(idleAnim, 0, 1);
    }
    void Update()
    {
        animationState = armatureComponent.animation.lastAnimationState;

        //get some values for condition check
        grounded = characterController2D.m_Grounded;
        speed = Mathf.Abs(playerMovement.horizontalMove);
        yVelocity = characterController2D.velocity.y;

        if (speed == 0 && grounded)
            Idle();

        if (speed != 0 && grounded)
            Run(speed / 3);

        if (yVelocity > 0.5f)
            Jump();

        if (yushaSkills.isFlying)
        {
            OpenUmbrella();
        }
    }
    #region Animations
    public void Idle()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.FadeIn(idleAnim, 0, -1);
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
    public void Jump()
    {
        if (state != State.JUMPING)
        {
            armatureComponent.animation.FadeIn(jumpAnim, 0, 1);
            armatureComponent.animation.timeScale = 2f;
            state = State.JUMPING;
        }
    }
    public void OpenUmbrella()
    { 
        if(!animationState.name.Equals(openUmbrAnim) && !animationState.name.Equals(flyUmbrAnim) && !grounded)
        {
            armatureComponent.animation.timeScale = 2f;
            armatureComponent.animation.FadeIn(openUmbrAnim, -1, 1);
            openUmbrellaDur = armatureComponent.animation.GetState(openUmbrAnim)._duration;   
        }
        Invoke("Fly", openUmbrellaDur / armatureComponent.animation.timeScale);
    }
    void Fly()
    {
        if (!armatureComponent.animation.lastAnimationName.Equals(openUmbrAnim))
            return;
        if (!armatureComponent.animation.lastAnimationName.Equals(flyUmbrAnim) && !grounded)        
            armatureComponent.animation.FadeIn(flyUmbrAnim, -1.0f, -1, 0);        
    }
    #endregion
}
