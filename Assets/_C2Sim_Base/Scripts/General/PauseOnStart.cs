using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnStart : MonoBehaviour
{
    public float originalTimescale;
    public bool pauseOnStart = true;
    public bool pauseOnEnable = false;

    // Start is called before the first frame update
    void Start()
    {
        if(pauseOnStart)
            Pause();
    }

    private void OnEnable()
    {
        if (pauseOnEnable)
            Pause();
    }

    public void Resume()
    {
        Debug.Log("-----PauseOnStart: Resume TimeScale:" + originalTimescale);
        Time.timeScale = originalTimescale;
    }

    public void Pause()
    {
        if (originalTimescale == 0)
            originalTimescale = Time.timeScale;

        Time.timeScale = 0;

    }
}
