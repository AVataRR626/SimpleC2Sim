using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBillboardRotationRef : MonoBehaviour
{
    public static TextBillboardRotationRef Instance;

    private void Awake()
    {
        Debug.Log("TextBillboardRotationRef: " + name);
        Instance = this;
    }
}
