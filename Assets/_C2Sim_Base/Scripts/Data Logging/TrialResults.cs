using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrialResults
{
    public string sceneName;
    public string trialCode;
    public string conditionString;
    public string transparency;
    public int trialIndex = -1;
    public int droneCount = 0;
    public int maxConcurrent;
    public int score;
    public int losses;    
    public BehaviourDataSummary behaviourSummary;
    public List<InteractionRecord> interactionLog;

    public TrialResults()
    {
        behaviourSummary = new BehaviourDataSummary();
        interactionLog = new List<InteractionRecord>();
    }
}
