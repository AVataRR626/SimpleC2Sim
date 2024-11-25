using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerDisplay;
    public string prefix = "time: ";
    public float time;
    public bool timing = true;

    // Start is called before the first frame update
    void Start()
    {
        if (timerDisplay == null)
            timerDisplay = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timing)        
            time += Time.deltaTime;
        
        if (timerDisplay != null)
            timerDisplay.text = prefix + time.ToString("0.00");
    }

    public void SetTimer(float newTime)
    {
        time = newTime;
    }
}
