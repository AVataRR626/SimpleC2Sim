using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveTimeTrigger : MonoBehaviour
{
    public float triggerTime;    
    public UnityEvent onTimeTrigger;
    public float triggerClock;
    public bool triggerFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        triggerClock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!triggerFlag)
        {
            if (gameObject.activeSelf)
                triggerClock += Time.unscaledDeltaTime;

            if (triggerClock >= triggerTime)
            {
                onTimeTrigger.Invoke();
                triggerFlag = true;
            }
        }
    }

    public void RestartTimer()
    {
        triggerClock = 0;
        TriggerTimer();
    }

    public void TriggerTimer()
    {
        triggerFlag = false;
    }
}
