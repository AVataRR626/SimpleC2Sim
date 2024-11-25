using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCopy : MonoBehaviour
{
    public Transform source;


    // Update is called once per frame
    void Update()
    {
        transform.rotation = source.rotation;
    }
}
