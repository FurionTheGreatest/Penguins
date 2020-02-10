using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PolarBearController : MonoBehaviour {

    GameObject player;
    public float rangeToBear;
    public GameObject deathScreen;
    public GameManagerr gameManager;
    public GameObject failedUI;
    bool isLolo = false;

    public bool wakedUp = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        wakedUp = false;
    }
	
	// TODO if(&& anySoundIsPlaying) steps etc
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        if (player.name.Equals("Lolo"))
        {            
            if(player.GetComponent<LoloSkills>().curState == LoloSkills.State.SilentWalk)
                isLolo = true;
        }            
        else
            isLolo = false;

        if (player != null && !isLolo)
        {
            if (Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < rangeToBear && Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < rangeToBear/2)
            {
                wakedUp = true;
                deathScreen.SetActive(true);
                gameManager.DisableMovement();
                Invoke("ActivateFailedUI", 4.5f);
            }                
        }
	}
    void ActivateFailedUI()
    {
        failedUI.SetActive(true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawLine(gameObject.transform.position + new Vector3(rangeToBear,0f,0f) , gameObject.transform.position + new Vector3(rangeToBear, rangeToBear / 2, 0f));       
        Handles.DrawLine(gameObject.transform.position - new Vector3(rangeToBear,0f,0f) , gameObject.transform.position + new Vector3(-rangeToBear, rangeToBear / 2, 0f)); 
        
        Handles.DrawLine(gameObject.transform.position + new Vector3(-rangeToBear, rangeToBear / 2, 0f), gameObject.transform.position + new Vector3(rangeToBear, rangeToBear / 2, 0f));       
    }
#endif
}
