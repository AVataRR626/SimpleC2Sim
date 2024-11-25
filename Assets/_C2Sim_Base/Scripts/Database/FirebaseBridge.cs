using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;

public class FirebaseBridge : MonoBehaviour
{
    public static FirebaseBridge Instance;



    [Header("System")]
    public string firebaseLink = "https://your-url.firebaseio.com/";
    public string fullPostLink;
    public bool post2Database = true;
    public List<string> eventLogCache;
    public bool verboseEventLog = false;

    [Header("Experiment Data")]
    public string experimentString = "mewowow";
    public string participantID = "moocow";
    public ExperimentData ed;//experimental data
    //public ExperimentDataWrapper edw;
    

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //experimentString = ExperimentManager.Instance.experimentString;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        TryInitED();

        if (!post2Database)
            Debug.LogError("WARNING: FirebaseBridge: Not Posting to Database");

    }

    void TryInitED()
    {
        if (ed == null)
        {
            

            ed = new ExperimentData();
            //ed.timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            DateTime timeStamp = System.DateTime.UtcNow;
            timeStamp.AddHours(11);
            ed.timeStamp = timeStamp.ToString("yyyy-MM-dd-HH-mm-ss");
            ed.participantID = participantID;
        }
        else
        {
            if(ed.timeStamp.Length == 0)
                ed.timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        GenerateFilename();
    }
     
    public void ReportTrialSequence(List <string> newSequence)
    {
        ed.trialSequence = newSequence;
    }

    public void ReportTrialSequence(string newTrial)
    {
        ed.trialSequence.Add(newTrial);
    }

    public void AddSurvey(SurveyResults sr)
    {
        if (ed.surveyResults == null)
            ed.surveyResults = new List<SurveyResults>();

        ed.surveyResults.Add(sr);
    }

    public void GenerateFilename()
    {
        fullPostLink = firebaseLink + experimentString + "_" + ed.timeStamp + "_" + participantID + ".json";
        //fullPostLink = firebaseLink + ed.timeStamp + "_" + experimentString + ".json";
    }

    public void TrainingFailFlag(string message)
    {
        ed.trainingFailFlag = message;
    }

    public void AddEvent(string newEvent)
    {
        if(verboseEventLog)
            eventLogCache.Add(Time.time + ": " + newEvent);
    }

    public void AddTrial()
    {
        TrialResults tr = new TrialResults();

        tr.trialIndex = ExperimentManager.Instance.trialIndex;
        tr.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        tr.transparency = TrialManager.Instance.conditionString;
        tr.conditionString = TrialManager.Instance.conditionString;
        tr.trialCode = TrialManager.Instance.trialCode;
        tr.droneCount = TrialManager.Instance.GetDroneCount();
        tr.maxConcurrent = TrialManager.Instance.maxConcurrent;

        TryInitED();

        ed.trialResults.Add(tr);
    }

    public void ReportAction(int trialIndex)
    {
        if(trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.actions++;
    }

    public void BehaviourTallyLegacy(int trialIndex, string interventionString)
    {
        if (interventionString == "fcc")
            ed.trialResults[trialIndex].behaviourSummary.divertFCC++;

        if (interventionString == "cc")
            ed.trialResults[trialIndex].behaviourSummary.divertCC++;

        if (interventionString == "dzsi")
            ed.trialResults[trialIndex].behaviourSummary.divertDZSI++;

        if (interventionString == "dzdi")
            ed.trialResults[trialIndex].behaviourSummary.divertDZDI++;

        if (interventionString == "dzdi_dc")
            ed.trialResults[trialIndex].behaviourSummary.divertCancelDZDI++;

        if (interventionString == "dzsi_dc")
            ed.trialResults[trialIndex].behaviourSummary.divertCancelDZSI++;

        if (interventionString == "fccd_dc")
            ed.trialResults[trialIndex].behaviourSummary.divertCancelFCCD++;

        if (interventionString == "dzna_dc")
            ed.trialResults[trialIndex].behaviourSummary.divertCancelDZNA++;

        if (interventionString == "dzua_dc")
            ed.trialResults[trialIndex].behaviourSummary.divertCancelDZUA++;
    }

    public void ReportIntervention(int trialIndex, string interventionString)
    {

        ReportIntervention(trialIndex, interventionString, "");

        /*
        ed.trialResults[trialIndex].behaviourSummary.interventions++;

        BehaviourTallyLegacy(trialIndex, interventionString);

        InteractionRecord ir = new InteractionRecord();
        ir.interactionTimestamp = GameManager.Instance.unscaledTrialClock;
        ir.trialTimestamp = GameManager.Instance.gameTimestamp;
        ir.interactionType = interventionString;
        ed.trialResults[trialIndex].interactionLog.Add(ir);
        */
    }

    public void ReportIntervention(int trialIndex, string interventionString, string interventionType)
    {

        ReportIntervention(trialIndex, interventionString, interventionType, "", "");

        /*
        ed.trialResults[trialIndex].behaviourSummary.interventions++;

        BehaviourTallyLegacy(trialIndex, interventionString);

        InteractionRecord ir = new InteractionRecord();
        ir.interactionTimestamp = GameManager.Instance.unscaledTrialClock;
        ir.trialTimestamp = GameManager.Instance.gameTimestamp;
        ir.interactionType = interventionType;
        ir.interactionTag = interventionString;
        ed.trialResults[trialIndex].interactionLog.Add(ir);
        */
    }

    //for interventions coming from individual agents
    public void ReportIntervention(int trialIndex, string interventionString, string interventionType, string agentName, string agentType)
    {
        if (trialIndex > -1)
        {
            ed.trialResults[trialIndex].behaviourSummary.interventions++;

            BehaviourTallyLegacy(trialIndex, interventionString);

            InteractionRecord ir = new InteractionRecord();

            ir.interactionTimestamp = GameManager.Instance.unscaledTrialClock;
            ir.trialTimestamp = GameManager.Instance.gameTimestamp;
            ir.interactionType = "ReportIntervention(): " + interventionType;
            ir.interactionTag = interventionString;
            ir.agentName = agentName;
            ir.agentType = agentType;

            ed.trialResults[trialIndex].interactionLog.Add(ir);
        }
        else
        {
            Debug.LogError("ExperimentManager: trialIndex out of rage. Set trialIndex to 0 if you just want to test data logging on individual trials");
        }
    }

    //for interventions coming from buttons or other general places (that are not neccessarily agents)
    public void ReportIntervention(int trialIndex, float interactionTimestamp, string interventionString)
    {
        if (trialIndex > -1)
        {
            ed.trialResults[trialIndex].behaviourSummary.interventions++;

            BehaviourTallyLegacy(trialIndex, interventionString);


            InteractionRecord ir = new InteractionRecord();
            ir.interactionTimestamp = interactionTimestamp;
            ir.interactionType = "ReportIntervention(): " + interventionString;
            ed.trialResults[trialIndex].interactionLog.Add(ir);
        }
        else
        {
            Debug.LogError("ExperimentManager: trialIndex out of rage. Set trialIndex to 0 if you just want to test data logging on individual trials");
        }
    }

    public void ReportInteraction(int trialIndex, float interactionTimestamp, string interactionString)
    {
        if (trialIndex > -1)
        {
            InteractionRecord ir = new InteractionRecord();
            ir.interactionTimestamp = interactionTimestamp;
            ir.interactionType = "ReportInteraction(): " + interactionString;
            ed.trialResults[trialIndex].interactionLog.Add(ir);
        }
        else
        {   
            Debug.LogError("ExperimentManager: trialIndex out of rage. Set trialIndex to 0 if you just want to test data logging on individual trials");
        }
    }

    public void ReportAirspaceExit(int trialIndex)
    {
        if(trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.airspaceExit++;
    }

    public void ReportLanding(int trialIndex)
    {
        if (trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.landings++;
    }

    public void ReportSafeOverfly(int trialIndex)
    {
        if (trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.safeOverfly++;
    }

    public void ReportCollision(int trialIndex)
    {
        if (trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.collisions++;
    }

    public void ReportDZCrash(int trialIndex)
    {
        if (trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.dzCrashes++;
    }

    public void ReportDZSafeIncursion(int trialIndex)
    {
        if (trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.dzSafeIncursions++;
        
    }

    public void ReportDZDangerousIncursion(int trialIndex)
    {
        if (trialIndex > -1)
            ed.trialResults[trialIndex].behaviourSummary.dzDangerousIncursions++;
    }

    [ContextMenu("Post Data")]
    public void PostToDatabase()
    {
        if (post2Database)
        {
            GenerateFilename();

            if (!verboseEventLog)
                ed.eventLog.Clear();


            //edw = new ExperimentDataWrapper();
            //edw.experimentString = experimentString;
            //edw.ed = ed;
            //RestClient.Put(fullPostLink, edw);


            Debug.Log(fullPostLink);            
            RestClient.Put(fullPostLink, ed);
        }
        else
        {
            Debug.LogError("WARNING: FirebaseBridge: Not Posting to Database");
        }
    }

    public void PostWithEventLogCache()
    {
        if(verboseEventLog)
            ed.eventLog = new List<string>(eventLogCache);

        PostToDatabase();
    }

    public void CurrentTrialSync()
    {
        ed.trialResults[ExperimentManager.Instance.trialIndex].score = GameManager.Instance.score;
        ed.trialResults[ExperimentManager.Instance.trialIndex].losses = GameManager.Instance.losses;
    }
    
    public void IncrementTrainingFullRepeatCount()
    {
        ed.trainingFullRepeatCount++;
    }

}