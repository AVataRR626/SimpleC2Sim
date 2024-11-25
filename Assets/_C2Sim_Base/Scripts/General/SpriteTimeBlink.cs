using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTimeBlink : MonoBehaviour
{
    public SpriteRenderer myRenderer;

    public int modFactor = 4;
    public int amplification = 30;

    public float clock;
    public bool spriteMode;

    // Start is called before the first frame update
    void Start()
    {
        if (myRenderer == null)
            myRenderer = GetComponent<SpriteRenderer>();

        clock = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
            spriteMode = ((int)(clock * amplification) % modFactor) != 0;
        else
            spriteMode = true;

        myRenderer.enabled = spriteMode;

        clock += Time.deltaTime;
    }
}
