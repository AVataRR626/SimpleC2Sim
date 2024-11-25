using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclusiveAudio : MonoBehaviour
{
    public bool stopOthersOnStart = true;
    public AudioSource exclusiveSource;
    // Start is called before the first frame update
    void Start()
    {
        if (stopOthersOnStart)
            StopAllOthers();
    }

    public void StopAllOthers()
    {
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();

        foreach (AudioSource a in allAudio)
        {
            if(a != exclusiveSource)
                a.Stop();
        }
    }
}
