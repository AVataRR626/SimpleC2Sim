using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractionRecord
{
    public string agentID;//assigned ID
    public string agentName;//game object name
    public string agentType;//prefab name? maybe the type..
    public string interactionType;//explicit coding, FFC, FCCD, CCD, etc..
    public string interactionTag;//aux coding, "False Positive", etc...
    public float trialTimestamp;//trial start time (relative to experiment start)
    public float stimulusTimestamp;//related stimulus's timestamp (from trial start)
    public float interactionTimestamp;//interaction's timestamp (from trial start)
    public string payload;//misc info

    public InteractionRecord()
    {
        interactionTimestamp = GameManager.Instance.unscaledTrialClock;
        trialTimestamp = GameManager.Instance.gameTimestamp;
    }
}
