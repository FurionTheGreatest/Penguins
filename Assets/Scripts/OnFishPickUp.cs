using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnFishPickUp : MonoBehaviour
{
    public Image fish;
    public GameObject sparks;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            var color = fish.color;
            color.r = 255f;
            color.g = 255f;
            color.b = 255f;

            fish.color = color;
            sparks.SetActive(true);
            Destroy(gameObject);
        }
    }
}
