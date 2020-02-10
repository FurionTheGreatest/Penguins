using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    public GameManagerr gameManagerr;
    public GameObject failedUI;

	[SerializeField] public float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
    public Vector2 velocity;
	const float k_GroundedRadius = .3f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]

	public UnityEvent OnLandEvent;

	private bool m_wasCrouching = false;

    public float decelerationTolerance = 4f;
    bool isAlive = true;
    public Vector3 jumpDistance;
    bool isTrampoline = false;

    public Transform GroundCheck { get => m_GroundCheck; set => m_GroundCheck = value; }
    public Transform CeilingCheck { get => m_CeilingCheck; set => m_CeilingCheck = value; }
    public LayerMask WhatIsGround { get => m_WhatIsGround; set => m_WhatIsGround = value; }

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            isTrampoline = true;
        }
        else
            isTrampoline = false;
    }
    private void FixedUpdate()
	{        
        velocity = m_Rigidbody2D.velocity;
        bool isFlying = false;

        bool wasGrounded = m_Grounded;
		m_Grounded = false;

        if (GameObject.Find("Yusha") != null)
        {
            isFlying = gameObject.GetComponent<YushaSkills>().isFlying;
        }

        if (isAlive && !isFlying && !isTrampoline) 
        {
            isAlive = Vector3.Distance(velocity, jumpDistance) < decelerationTolerance;
            jumpDistance = velocity;
        }
        else if(!isAlive)
        {
            failedUI.SetActive(true);
            gameManagerr.DisableMovement();
            Debug.Log("Killed by falling");
        }
        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, k_GroundedRadius, WhatIsGround);
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


	public void Move(float move, bool crouch, bool jump)
	{
        
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(CeilingCheck.position, k_CeilingRadius, WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;					
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				
				} else {

				if (m_wasCrouching)
				{
					m_wasCrouching = false;					
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
		    m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
