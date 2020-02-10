using UnityEngine;

public class GameManagerr : MonoBehaviour
{
    public GameObject failedUI;
    public GameObject passedUI;

    //AudioManager audioManager;

    /*private void Start()
    {
        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        audioManager.StopSound("Music");
        audioManager.PlaySound("GameplayMusic");
    }*/
    public void DisableMovement()
    {
        //audioManager.PlaySound("Death");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController2D>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
