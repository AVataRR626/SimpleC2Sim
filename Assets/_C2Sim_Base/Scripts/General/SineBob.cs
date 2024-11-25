using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineBob : MonoBehaviour
{

    public Vector3 startingPos;
    public float xFactor = 12;
    public float zFactor = 0;
    public float yFactor = 0;

    public float clockScale = 10;
    public float clock;
    float sinValue;
    public Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent == null)
            startingPos = transform.position;
        else
            startingPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        sinValue = Mathf.Sin(clock);

        offset.x = sinValue * xFactor;        
        offset.y = sinValue * yFactor;
        offset.z = sinValue * zFactor;

        if (transform.parent == null)
            transform.position = startingPos + offset;
        else
            transform.localPosition = startingPos + offset;


        clock += Time.unscaledDeltaTime * clockScale;
    }
}
