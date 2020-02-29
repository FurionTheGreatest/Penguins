using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineForceController : MonoBehaviour {


    GameObject player;
    public Vector2 velocity;

    public bool onTop = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject;

        Jump();       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        player = collision.gameObject;

        Jump();
    }

    void Jump()
    {
        StartCoroutine(CD());
        player.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    IEnumerator CD()
    {        
        yield return new WaitForSeconds(.5f);
        onTop = false;
    }
}
