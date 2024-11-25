using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseDownEvent : MonoBehaviour
{
    public UnityEvent onMouseDown;
    public bool oneClickOnly;
    public GameObject activeObjectGatekeeper;

    public bool clickFlag = false;

    private void OnMouseDown()
    {
        Debug.Log("MouseEvent:OnMouseDown by " + name);

        if (oneClickOnly && clickFlag)
            return;


        //if it exists, make sure the gatekeeper object is active
        //before registering any clicks
        if (activeObjectGatekeeper != null)
            if (!activeObjectGatekeeper.activeSelf)
                return;


        onMouseDown.Invoke();
        clickFlag = true;
 
    }
}
