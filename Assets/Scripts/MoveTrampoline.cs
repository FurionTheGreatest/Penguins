using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrampoline : MonoBehaviour
{
    public GameObject trampoline;
    public GameObject canvas;
    Vector3 newPosition;

    void Start()
    {
        newPosition = gameObject.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        CallTrampoline();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CallTrampoline();
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        canvas.SetActive(false);
    }
    void CallTrampoline()
    {        
        canvas.SetActive(true);
        if (Input.GetKey(KeyCode.T))
            trampoline.GetComponent<Transform>().position = newPosition;        
    }
}
