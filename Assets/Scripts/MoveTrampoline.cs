using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrampoline : MonoBehaviour
{
    public GameObject trampoline;
    public GameObject canvas;
    Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = gameObject.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
                trampoline.GetComponent<Transform>().position = newPosition;
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))            
                trampoline.GetComponent<Transform>().position = newPosition;
            
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvas.SetActive(false);
    }
}
