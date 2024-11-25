using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparenter : MonoBehaviour
{

    public bool unparentOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (unparentOnStart)
            Unparent();
    }

    // Update is called once per frame
    public void Unparent()
    {
        transform.parent = null;
    }
}
