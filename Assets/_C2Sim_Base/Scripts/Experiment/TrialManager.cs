using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : MonoBehaviour
{
    public static TrialManager Instance;

    [Header("Trial Settings")]
    public string trialCode;
    public string agentString;
    public string conditionString;
    public int maxConcurrent = 12;
    public float trialTime;
    public bool loadSurveyScene = true;
    public bool toggleable = false;    
    public KeyCode toggleKey = KeyCode.T;
    
    public int[] activeLayers;    

    [Header("System Stuff")]    
    public DelayedActivator delayedActivator;
    public int droneCount;

    void Start()
    {
        Instance = this;

        if (delayedActivator == null)
            delayedActivator = GetComponent<DelayedActivator>();

        droneCount = GetDroneCount();

        conditionString = "";

        for (int i = 0; i < activeLayers.Length; i++)
        {
            conditionString += "L[" + i + "]:";
            conditionString += activeLayers[i].ToString();

            if (activeLayers[i] == 0)
                conditionString += " T";
            else
                conditionString += " G";

            conditionString += " | ";
        }

        conditionString += " MAXD:" + droneCount;

        //GameManager.Instance.maxTime = trialTime;
        //Debug.Log("==========ADD TRIAL===========");
        FirebaseBridge.Instance.AddTrial();
    }

    private void Update()
    {
        if (toggleable)
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (activeLayers[0] == 0)
                    activeLayers[0] = 1;
                else
                    activeLayers[0] = 0;

                ResetTransparencyLayers();
            }
        }


    }

    [ContextMenu("Reset Layers")]
    public void ResetTransparencyLayers()
    {
        C2Sim_Transparency_Manager[] tmgrs = FindObjectsOfType<C2Sim_Transparency_Manager>();

        foreach (C2Sim_Transparency_Manager tmgr in tmgrs)
            tmgr.resetFlag = true;
    }

    public void EndTrial()
    {
        FirebaseBridge.Instance.CurrentTrialSync();
        FirebaseBridge.Instance.PostToDatabase();

        if (loadSurveyScene)
        {
            if (ExperimentManager.Instance.blocks.Length == 0)
            {
                //when in regular old, no blocks mode, just load the survey scene...
                LoadSurveyScene();//
            }
            else
            {
                //but if blocks exist, make sure to only load surveys after every block - not every trial
                if(ExperimentManager.Instance.surveyIndicies.Contains(ExperimentManager.Instance.trialIndex))
                    LoadSurveyScene();
                else
                    LoadNextTrial();
            }
        }
        else
        {
            LoadNextTrial();
        }
    }

    public void LoadSurveyScene()
    {
        Time.timeScale = 1;
        ExperimentManager.Instance.LoadSurveyScene();
    }

    public void LoadNextTrial()
    {
        Time.timeScale = 1;
        ExperimentManager.Instance.NextTrial();
    }

    public int GetDroneCount()
    {
        int result = 0;

        //C2Sim_Asset[] drones = FindObjectsOfType<C2Sim_Asset>();
        foreach (DelayedActivationTime t in delayedActivator.subjects)
        {
            if (t != null)
            {
                C2Sim_Asset c2a = t.GetComponent<C2Sim_Asset>();
                if (c2a != null)
                    result++;
            }
        }

        return result;
    }

    public void ReportIntervention(string interventionString)
    {
        ExperimentManager.Instance.ReportIntervention(interventionString);
    }

    public void ReportIntervention(float timestamp, string interventionString)
    {
        ExperimentManager.Instance.ReportIntervention(timestamp, interventionString);
    }

    public void ReportInteraction(float timestamp, string interactionString)
    {
        ExperimentManager.Instance.ReportInteraction(timestamp, interactionString);
    }
}
