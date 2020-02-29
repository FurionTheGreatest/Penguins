using System.Collections;
using UnityEngine;
using DragonBones;

public class LoloAnimator : MonoBehaviour
{
    /// <summary>
    /// scripts with character values needed to check
    /// </summary>
    private PlayerMovement playerMovement;
    private CharacterController2D characterController2D;
    private LoloSkills loloSkills;
    /// <summary>
    /// names of animations
    /// </summary>
    private static readonly string idleAnim = "idl";
    private static readonly string runAnim = "run";
    private static readonly string idleSpecialAnim = "idl_rare";
    private static readonly string jumpAnim = "jump";
    private static readonly string silWalkAnim = "run_tapki";
    /// <summary>
    /// animation states
    /// </summary>
    enum State { NULL, IDLE, IDLE_RARE, WALKING, JUMPING, SILENT_WALK }
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
        loloSkills = transform.GetComponent<LoloSkills>();               
    }
    void Update()
    {
        //get some values for condition check
        grounded = characterController2D.m_Grounded;
        speed = Mathf.Abs(playerMovement.horizontalMove);
        yVelocity = characterController2D.velocity.y;

        if (speed == 0 && grounded && loloSkills.curState != LoloSkills.State.SilentWalk)        
            PlayIdle();
        if (!armatureComponent.animation.isPlaying)
            state = State.NULL;
        if (speed != 0 && grounded && loloSkills.curState != LoloSkills.State.SilentWalk)
            Run(speed / 7);
        if (yVelocity > 0.5f)
            Jump();
        if (speed != 0 && loloSkills.curState == LoloSkills.State.SilentWalk)
        {
            armatureComponent.animation.timeScale = 1f;
            SilentWalk();
        }
        else if (speed == 0 && loloSkills.curState == LoloSkills.State.SilentWalk)
        {
            armatureComponent.animation.timeScale = 0f;
            SilentWalk();
        }
    }
    /// <summary>
    /// pick a random animation of idle
    /// </summary>
    void PlayIdle()
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
    #region Animations
    public void Idle()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.FadeIn(idleAnim, 0, 1);
            armatureComponent.animation.timeScale = 1f;
            state = State.IDLE;
        }
    }
    public void IdleRare()
    {
        if (state != State.IDLE)
        {
            armatureComponent.animation.FadeIn(idleSpecialAnim, 0, 1);
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
    public void SilentWalk()
    {
        if (state != State.SILENT_WALK)
        {
            armatureComponent.animation.FadeIn(silWalkAnim, 0, 1);
            armatureComponent.animation.timeScale = 1f;            
            state = State.SILENT_WALK;
        }
    }
    #endregion
}
