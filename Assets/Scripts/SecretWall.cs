using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{

    GameObject player;
    SpriteRenderer secretWall;
    float alpha =1 ;
    private void Start()
    {
        secretWall = gameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (secretWall.bounds.Contains(player.transform.position))
        {
            //alpha = 1;
            FadeOutWall();
        }
        //secretWall.enabled = false;

        else
        {
            //alpha = 0;
            FadeInWall();
        }
            //secretWall.enabled = true;

    }

    void FadeInWall()
    {
        var color = secretWall.color;
        
        if(color.a <= 1)
        {
            alpha += Time.deltaTime*2;
            color.a = alpha;
            secretWall.color = color;
        }
    }

    void FadeOutWall()
    {
        var color = secretWall.color;

        if (color.a >= 0)
        {
            alpha -= Time.deltaTime*2;
            color.a = alpha;
            secretWall.color = color;
        }
    }
}
