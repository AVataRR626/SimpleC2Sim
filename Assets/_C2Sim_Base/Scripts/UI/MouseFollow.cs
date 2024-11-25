using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    public Vector3 offset = new Vector3(0, -5, 0);
    public Vector3 mousePos;
    public Camera myCamera;

    private void Start()
    {
        if (myCamera == null)
            myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(myCamera != null)
        {
            mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);

            transform.position = mousePos + offset;
        }
    }
}
