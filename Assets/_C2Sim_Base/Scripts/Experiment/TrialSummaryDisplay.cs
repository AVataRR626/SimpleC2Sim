using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialSummaryDisplay : MonoBehaviour
{
    public Text text;
    public string toplineMessage = "Good Luck!";
    public string displayString = "";

    // Start is called before the first frame update
    void Start()
    {

        int trialNumber = ExperimentManager.Instance.trialIndex - ExperimentManager.Instance.shuffleStartIndex + 1;
        int trialCountMax = ExperimentManager.Instance.trialOrder.Length - ExperimentManager.Instance.shuffleStartIndex;

        string original = text.text;

        displayString = "";
        //displayString += "<b>TRIAL CODE: </b>" + TrialManager.Instance.trialCode;        
        displayString += "\n";
        displayString += "\n";
        displayString += "<b>TRIAL #: </b>" + trialNumber + " of " + trialCountMax;
        //displayString += "<b>DISPLAY  MODE: </b>" + TrialManager.Instance.conditionString;
        //displayString += "<b>AGENT ID: </b>" + TrialManager.Instance.agentString;
        displayString += "\n";
        displayString += "\n";

        Debug.Log("TrialSummaryDisplay: " + displayString);

        text.text = original + displayString;
    }
}
