using System.Collections;
using UnityEngine;
using DragonBones;

public class TotoAnimator : MonoBehaviour
{
    /// <summary>
    /// scripts with character values needed to check
    /// </summary>
    private PlayerMovement playerMovement;
    private CharacterController2D characterController2D;
    private TotoSkills totoSkills;
    /// <summary>
    /// names of animations
    /// </summary>
    private static readonly string idleAnim = "idle";
    private static readonly string runAnim = "run";
    private static readonly string idleNoClipAnim = "idle_no_clip";
    private static readonly string jumpAnim = "jump";
    private static readonly string dashStartAnim = "penetration_start";
    private static readonly string dashAnim = "penetration_cast";
    /// <summary>
    /// animation states
    /// </summary>
    enum State { NULL,IDLE,IDLE_NO_CLIP, WALKING, JUMPING, DASH_START, DASH_CAST}
    private State state = State.IDLE_NO_CLIP;
    private UnityArmatureComponent armatureComponent;
    /// <summary>
    /// storing values of other scripts
    /// </summary>
    private float speed;
    private float xVelocity;
    private float yVelocity;
    private bool isJumping;
    private bool isRolling;
    private bool grounded;

    private float normalRunSpeed;
    private float disabledRunSpeed = 0f;

    public ParticleSystem[] snowBall;
    DragonBones.AnimationState animationState;
    void Start()
    {
        armatureComponent = transform.GetComponentInChildren<UnityArmatureComponent>();
        playerMovement = transform.GetComponent<PlayerMovement>();
        characterController2D = transform.GetComponent<CharacterController2D>();
        totoSkills = transform.GetComponent<TotoSkills>();

        normalRunSpeed = playerMovement.runSpeed;

        foreach (ParticleSystem ps in snowBall)
        {
#pragma warning disable CS0618 // Тип или член устарел
            ps.enableEmission = false;
#pragma warning restore CS0618 // Тип или член устарел
        }
    }

    void Update()
    {
        //Debug.Log(state);
        animationState = armatureComponent.animation.lastAnimationState;
        
        //get some values for condition check
        grounded = characterController2D.m_Grounded;
        speed = Mathf.Abs(playerMovement.horizontalMove);
        yVelocity = characterController2D.velocity.y;
        isRolling = totoSkills.dash;

        if (speed == 0 && grounded && !isRolling)         
             PlayIdle();         
            
        /*if (!armatureComponent.animation.isPlaying)
        {
            state = State.NULL;
            PlayIdle();
        }    */        
        if (speed != 0 && grounded && !isRolling)        
            Run(speed/2);
        if (yVelocity > 0.5f)        
            Jump();

        //Debug.Log(totoSkills.dashKeyPressed);
        if (totoSkills.dashKeyPressed && !totoSkills.dashTimeEnded)
        {
            playerMovement.runSpeed = disabledRunSpeed;
            Dash();
        }
        if (totoSkills.dashTimeEnded || !isRolling || totoSkills.dashKeyReleased)
        {
            playerMovement.runSpeed = normalRunSpeed;
            StopRolling();
        }
        /*if (totoSkills.dashKeyReleased)
        {
            playerMovement.runSpeed = normalRunSpeed;
            StopRolling();
            //state = State.NULL;
        }*/
        //Debug.Log(animationState.name);
    }
    /// <summary>
    /// pick a random animation of idle
    /// </summary>
    void PlayIdle()
    {
        float random = Random.value;
        if (random < 0.2f)
        {            
            Idle();
        }
        if (random > 0.2f)
        {                          
            IdleNoClip();
        }
    }
    #region Animations
    public void Idle()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.timeScale = 1f;
            armatureComponent.animation.FadeIn(idleAnim, 0, -1);            
            state = State.IDLE;
        }
    }
    public void IdleNoClip()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.timeScale = 1f;
            armatureComponent.animation.FadeIn(idleNoClipAnim, 0, 1);            
            state = State.IDLE;
            Idle();
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
    public void Dash()
    {
        if (animationState.name != dashStartAnim)
        {
            armatureComponent.animation.timeScale = 1f;
            armatureComponent.animation.FadeIn(dashStartAnim, 0, 1);            
            //state = State.DASH_START;
        }
        Invoke("Roll", armatureComponent.animation.lastAnimationState._duration);
    }
    void Roll()
    {
        if (animationState.name != dashAnim && state != State.IDLE || state != State.IDLE_NO_CLIP)
        {
            armatureComponent.animation.timeScale = 1f;
            foreach (ParticleSystem ps in snowBall)
            {
#pragma warning disable CS0618 // Тип или член устарел
                ps.enableEmission = true;
#pragma warning restore CS0618 // Тип или член устарел
            }
            armatureComponent.animation.FadeIn(dashAnim, -1.0f, -1);
            state = State.DASH_CAST;
        }
    }
    void StopRolling()
    {
        foreach (ParticleSystem ps in snowBall)
        {
#pragma warning disable CS0618 // Тип или член устарел
            ps.enableEmission = false;
#pragma warning restore CS0618 // Тип или член устарел
        }
    }
    #endregion
}
