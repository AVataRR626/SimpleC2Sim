using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedActivationTime : MonoBehaviour
{
    public float activationTime = 0;
    public bool activationFlag = false;
    public UnityEvent onEnable;

    public void Activate()
    {
        onEnable.Invoke();
    }
}
