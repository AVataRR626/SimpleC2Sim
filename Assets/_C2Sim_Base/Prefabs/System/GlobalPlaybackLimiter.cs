using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlaybackLimiter : MonoBehaviour
{
    public static int playAttempts;
    public int playLimit = 1;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("====GlobalPlaybackLimiter: Remaining Playback: " + (playLimit - playAttempts));

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            if (playAttempts >= playLimit)
            {
                audioSource.playOnAwake = false;
                audioSource.Stop();
            }

            playAttempts++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayAttempts()
    {
        playAttempts = 0;
    }
}
