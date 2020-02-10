using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class IcicleController : MonoBehaviour {
    [Header("Floor icicle")]
    public bool onFloor = false;

    [Header("Scanning settings")]
    [SerializeField] private LayerMask playerMask;
    public Transform originPos;
    public Vector2 dir = new Vector2(-0.7f, -8f);
    public float distance;
    
    Rigidbody2D icicleRB;
    [Header("Random falling properties")]
    public bool rolled = false;
    public float waitTimeForNewRoll;
    [Space]
    public KaryakSkills state;

    LevelManager levelManager;
    GameManagerr gameManagerr;
    public UnityEvent icicleFall;

    public GameObject failedUI;

    private void Awake()
    {
        icicleRB = gameObject.GetComponent<Rigidbody2D>();
        originPos = gameObject.transform;
        levelManager = GameObject.FindGameObjectWithTag("GM").GetComponent<LevelManager>();
        gameManagerr = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManagerr>();
    }

    //Casting a ray and looking for a player
    void FixedUpdate () {
        state = FindObjectOfType<KaryakSkills>();
        if (!onFloor)
        {
            RaycastHit2D[] colliders = Physics2D.RaycastAll(originPos.position, dir, Vector2.Distance(originPos.position, dir));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].transform.CompareTag("Player") && !rolled)
                    icicleFall.Invoke();
            }
        }
    }
    //50% chance to drop an icicle 
    public void IcicleFall()
    {
        float random = Random.value;
        if (random <= 0.5)
            icicleRB.isKinematic = false;

        rolled = true;
        StartCoroutine("WaitForRoll");
    }
    //Wait for a new roll for a random value
    private IEnumerator WaitForRoll()
    {
        yield return new WaitForSeconds(waitTimeForNewRoll);
        rolled = false;
    }
    //Visualisation for Raycast
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!onFloor)
        {
            Handles.color = Color.red;
            Handles.DrawLine(gameObject.transform.position, (Vector2)gameObject.transform.position + dir);
        }      
    }
    #endif
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Karyak") && state.curState == KaryakSkills.State.Invisible)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            failedUI.SetActive(true);
            gameManagerr.DisableMovement();
            icicleRB.bodyType = RigidbodyType2D.Static;
            if (collision.gameObject.name.Equals("Karyak"))
                state.curState = KaryakSkills.State.Normal;
            Debug.Log("Killed by icicle");
        }
               
    }
}

