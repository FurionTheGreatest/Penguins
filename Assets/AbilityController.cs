using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityController : MonoBehaviour, IEventSystemHandler
{
    public bool isButtonPressed;

    private void Update()
    {
        if (isButtonPressed)
            Debug.Log("down");
        else
            Debug.Log("up");

    }
    /*public void OnPointerDown()
    {
        isButtonPressed = true;
    }
    public void OnPointerUp()
    {
        isButtonPressed = false;
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
        isButtonPressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
        isButtonPressed = false;
    }

}
