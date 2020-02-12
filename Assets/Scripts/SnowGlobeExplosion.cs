using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGlobeExplosion : MonoBehaviour {
    
    public GameObject particleSysTriggered;
    public GameObject particleSys;
    public float prewarmTime = 0.5f;

    CharacterSelect characterSelect;
    TotoSkills totoSkills;
    private GameObject player;
    private Collider2D blockCol;
    private bool endOfExplosion = false;
    private SpriteRenderer sprite;

    private bool dash;
    //AudioManager audioManager;
    // Use this for initialization
    void Start () {
        characterSelect = GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelect>();
        //totoSkills = GameObject.Find("Toto").GetComponent<TotoSkills>();
        blockCol = GetComponentInChildren<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        /*if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }*/
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        player = characterSelect.currActiveCharacter;
        if (characterSelect.currActiveCharacter.name == "Toto")
            dash = characterSelect.currActiveCharacter.GetComponent<TotoSkills>().dash;
        else return;

         if (dash && blockCol != null)
         {
             blockCol.enabled = false;
         }
           if (!dash && !endOfExplosion && blockCol != null)
              blockCol.enabled = true;

        if (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < 1.3f && 
            Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < 4.3f && dash)
        {
            DestroyImmediate(blockCol);
            sprite.enabled = false;
            
            InitializeExplosion();
        }

        if (!particleSysTriggered.GetComponent<ParticleSystem>().isPlaying && endOfExplosion)
            Destroy(gameObject);
    }
     
    void InitializeExplosion()
    {
        particleSys.SetActive(true);
        particleSysTriggered.SetActive(true);
        //audioManager.PlaySound("SnowExpl");
        StartCoroutine(Prewarm());
    }
    
    IEnumerator Prewarm()
    {
        yield return new WaitForSeconds(prewarmTime);
        endOfExplosion = true;
    }
}
