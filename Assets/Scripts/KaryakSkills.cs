using UnityEngine;


public class KaryakSkills : MonoBehaviour {

    public PlayerMovement playerMovement;
    public CharacterSelect characterSelect;

    public enum State { Invisible, TimeSlow, Normal };
    public State curState;

    [Header("Invis options")]
    public float timeForInvis = 5f;
    public float curTimeInvi = 0f;
    public bool invisCasts = false;

    private Color colorOfSprite;
    private bool readyToUse = true;
    [Header("SlowMo options")]
    public float slowMoSpeed = 0.5f;
    public float normalSpeed = 1f;

    public float timeForSlowMo = 5f;
    public float curTimeSlow = 0f;

    [Header("Stats changing")]
    public float doubledSpeed;
    public float normalRunSpeed;

    [Space]
    public ParticleSystem legsPartSys;
    private ParticleSystem.EmissionModule emission;

    //AudioManager audioManager;
    private void Awake()
    {
        emission = legsPartSys.emission;
    }
    // Use this for initialization
    void Start () {
        curState = State.Normal;
        //colorOfSprite = GameObject.Find("Karyak").GetComponent<SpriteRenderer>().color;

        normalRunSpeed = playerMovement.runSpeed;
        doubledSpeed = normalRunSpeed * 2;

        /*if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }*/
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.F) && readyToUse)
        {

            curState = State.Invisible;
            invisCasts = true;
        }
        else invisCasts = false;

        if (Input.GetKeyDown(KeyCode.G) && readyToUse)
        {
            //audioManager.PlaySound("TimeSlow");
            curState = State.TimeSlow;
        }

        switch (curState)
        {
            case State.Invisible:
                Invisible();
                break;
            case State.TimeSlow:
                TimeSlow();
                break;
            case State.Normal:
                NormalState();
                break;
        }

        if (Mathf.Abs(playerMovement.horizontalMove) != 0 && !playerMovement.jump)
        {
            emission.enabled = true;
        }
        else
            emission.enabled = false;
    }

    void Invisible(){
        readyToUse = false;
        
        if(curTimeInvi < timeForInvis)
        {
            characterSelect.canSelectCharacter = false;
            curTimeInvi += Time.deltaTime;        
        }
        else            
            curState = State.Normal;        
    }

    void TimeSlow()
    {
        readyToUse = false;

        if (curTimeSlow < timeForSlowMo)
        {
            characterSelect.canSelectCharacter = false;
            curTimeSlow += Time.unscaledDeltaTime;
            Time.timeScale = slowMoSpeed;
            ChangeStats();
        }
        else
        {
            curState = State.Normal;
        }
    }

    void NormalState()
    {
        Time.timeScale = normalSpeed;

        playerMovement.runSpeed = normalRunSpeed;

        curTimeInvi = 0f;
        curTimeSlow = 0f;

        readyToUse = true;
        characterSelect.canSelectCharacter = true;
    }

    void ChangeStats()
    {
        playerMovement.runSpeed = doubledSpeed;
    }
}
