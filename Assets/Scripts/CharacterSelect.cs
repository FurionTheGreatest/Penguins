using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour {

    public TotoSkills totoSkills;
    public KaryakSkills karyakSkills;
    public LoloSkills loloSkills;

    public GameObject[] characters;
    public Transform restrictPoint;

    public bool canSelectCharacter = true;

    public GameObject currActiveCharacter;
    Transform playerTransform;

    public CameraFollow camera;
    private int indexOfPlayer;

    private Vector3 lastPos;
    private Vector3 lastScale;

    private bool grounded;
    public AudioManager audioManager;

    void Start() {
        if (characters[0].activeInHierarchy == true)
            indexOfPlayer = 0;
        if (characters[1].activeInHierarchy == true)
            indexOfPlayer = 1;
        if (characters[2].activeInHierarchy == true)
            indexOfPlayer = 2;
        if (characters[3].activeInHierarchy == true)
            indexOfPlayer = 3;

        if(camera == null)
        {
            Debug.LogError("Camera is not found");
        }

        if(currActiveCharacter == null)
        {
            currActiveCharacter = GameObject.FindGameObjectWithTag("Player");
            playerTransform = currActiveCharacter.transform;
            camera.TargetToFollow = playerTransform;
        }
    }

    void Update() {
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        grounded = currActiveCharacter.GetComponent<CharacterController2D>().m_Grounded;

        if (!grounded || totoSkills.dash || karyakSkills.curState != KaryakSkills.State.Normal || loloSkills.curState != LoloSkills.State.Normal)
        {
            canSelectCharacter = false;
        }
        else        
            canSelectCharacter = true;

        if (restrictPoint != null && restrictPoint.gameObject.activeInHierarchy)
        {
            RestrictChecker();
            SnowRestrict();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && canSelectCharacter)
        {
            ActivatePlayer(0);

        } else if (Input.GetKeyDown(KeyCode.Alpha2) && canSelectCharacter)
        {
            ActivatePlayer(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && canSelectCharacter)
        {
            ActivatePlayer(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && canSelectCharacter)
        {
            ActivatePlayer(3);
        }
    }

     //TODO +2 points to check?
    bool RestrictChecker()
    {
        GameObject[]  ceilings = GameObject.FindGameObjectsWithTag("Ceiling");
        foreach (GameObject ceiling in ceilings)
        {
            if (ceiling.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(restrictPoint.position.x, restrictPoint.position.y, 0f)))
            {                
                canSelectCharacter = false;
                break;
            }            
        }
        return canSelectCharacter;
    }

    bool SnowRestrict()
    {
        GameObject[] snows = GameObject.FindGameObjectsWithTag("Snow");

        foreach (GameObject snow in snows)
        {
            if (snow.GetComponent<BoxCollider2D>() != null)
            {
                if (snow.GetComponent<BoxCollider2D>().bounds.Contains(new Vector3(restrictPoint.position.x, restrictPoint.position.y, 0f)))
                {
                    canSelectCharacter = false;
                    break;
                }                
            }
            else
            {
                snow.tag = "Untagged";
                break;
            }                
        }

        return canSelectCharacter;
    }
    //TODO lastScale DB
    void ChangeTransformWithCharacterChanging(GameObject currentCharacter)
    {
       currentCharacter.transform.position = lastPos;
       
       //currentCharacter.transform.localScale = lastScale;
    }

    void ActivatePlayer(int index)
    {
        audioManager.PlaySound("PlayerChange");
        //find new character
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //check if this character active now, disable all characters, enable needed
        if (characters[index].activeInHierarchy == false)
        {
            //last position of disabled character = to new character
            lastPos = playerTransform.position;
            //lastScale = playerTransform.localScale;
            foreach (var character in characters)
                character.SetActive(false);
        
            characters[index].SetActive(true);        
            currActiveCharacter = characters[index];

            CameraFollow(currActiveCharacter.transform);
            ChangeTransformWithCharacterChanging(currActiveCharacter);
        }
    }

    void CameraFollow(Transform playerToFollow)
    {
        camera.TargetToFollow = playerToFollow;
    }
}
