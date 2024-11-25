using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeeder : MonoBehaviour
{
    public float speedFactor = 3;
    public KeyCode speedKey = KeyCode.Space;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(speedKey))
        {
            Time.timeScale = speedFactor;
        }
        
        if(Input.GetKeyUp(speedKey))
        {
            Time.timeScale = 1;
        }
    }
}
