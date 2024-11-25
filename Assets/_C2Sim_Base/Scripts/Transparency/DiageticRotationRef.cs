using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiageticRotationRef : MonoBehaviour
{
    public static DiageticRotationRef Instance;

    private void Awake()
    {
        Debug.Log("DiageticRotationRef: " + name);
        Instance = this;
    }
}
