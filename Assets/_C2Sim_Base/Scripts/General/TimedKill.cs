using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedKill : MonoBehaviour
{
    public float ttl = 1.5f;
    public bool disableOnTTL;
    // Start is called before the first frame update
    void Start()
    {
        if (!disableOnTTL)
            Destroy(gameObject, ttl);
        else
            Invoke("TTLDisable", ttl);
    }

    public void TTLDisable()
    {
        gameObject.SetActive(false);
    }
}
