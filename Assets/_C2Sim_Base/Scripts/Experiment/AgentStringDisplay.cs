using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentStringDisplay : MonoBehaviour
{
    public Text text;
    public string prefix = "agent id: ";

    void Start()
    {
        text.text = prefix + TrialManager.Instance.agentString;
    }
}
