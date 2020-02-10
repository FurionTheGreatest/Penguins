using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour {

    public GameObject[] doorStates;
    public BoxCollider2D doorCol;

    public bool doorOpened = false;
    //public AudioManager audioManager;
    /*private void Start()
    {
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
    }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Lolo"))
        {
            if(collision.gameObject.GetComponent<LoloSkills>().haveKey == true)
            {
                //audioManager.PlaySound("LevelEnd");
                doorOpened = true;
                doorCol.enabled = false;
                collision.gameObject.GetComponent<LoloSkills>().haveKey = false;
            }
        }
        else return;
    }

    private void Update()
    {
        if (!doorOpened)
        {
            doorStates[0].gameObject.SetActive(true);
            doorStates[1].gameObject.SetActive(false);
        }
        else
        {
            doorStates[1].gameObject.SetActive(true);
            doorStates[0].gameObject.SetActive(false);
        }
    }
}
