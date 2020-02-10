using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Lolo"))
        {
            collision.gameObject.GetComponent<LoloSkills>().haveKey = true;
            Destroy(gameObject);
        }
    }
}
