using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExperimentData
{
    public string participantID;
    public string timeStamp;
    public string trainingFailFlag;
    public int trainingFullRepeatCount = 0;
    public List<string> trialSequence;
    public List<string> eventLog;
    public List<SurveyResults> surveyResults;
    public List<TrialResults> trialResults;

    public ExperimentData()
    {  
        eventLog = new List<string>();
        trialSequence = new List<string>();
        timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-s");
    }
}
