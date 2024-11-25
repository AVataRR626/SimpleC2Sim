using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantFollow : MonoBehaviour
{
    public Transform subject;
    public Vector3 offset;
    public bool unparent = true;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - subject.position;

        if (unparent)
            transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = subject.position + offset;   
    }
}
