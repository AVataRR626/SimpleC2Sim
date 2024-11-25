using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedActivator : MonoBehaviour
{
    public bool globalMode = true;
    public bool autoPopulate = true;
    public bool initOnEnable = false;
    public DelayedActivationTime[] subjects;
    public float [] timers;    

    // Start is called before the first frame update
    void Start()
    {   
        if(!initOnEnable)
            Init();
    }

    private void OnEnable()
    {
        if(initOnEnable)
            Init();
    }

    public void Init()
    {
        Debug.Log("DelayedActivator:Init() " + name);

        InitTimers();
        DeactivateSubjects();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ManageActivations();
        TimerCountdown();
    }

    public void InitTimers()
    {
        if (autoPopulate && globalMode)
            subjects = FindObjectsOfType<DelayedActivationTime>();

        timers = new float[subjects.Length];

        for(int i = 0; i < timers.Length; i++)
        {
            timers[i] = subjects[i].activationTime;
        }
    }

    public void DeactivateSubjects()
    {
        Debug.Log("DelayedActivator:DeactivateSubjects() " + name);
        foreach (DelayedActivationTime o in subjects)
        {
            o.gameObject.SetActive(false);
        }
    }

    public void ManageActivations()
    {
        for (int i = 0; i < timers.Length; i++)
        {
            if(timers[i] <= 0)
            {
                if (subjects[i] != null)
                {
                    if (!subjects[i].activationFlag)
                    {
                        subjects[i].gameObject.SetActive(true);
                        subjects[i].Activate();
                        subjects[i].activationFlag = true;
                    }
                }
            }
        }
    }

    public void TimerCountdown()
    {
        for (int i = 0; i < timers.Length; i++)
        {
            if (timers[i] > 0)
                timers[i] -= Time.fixedDeltaTime;
            else
                timers[i] = 0;
        }
    }

}
