using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Sim_Transparency_Manager : MonoBehaviour
{
    public GameObject[] transparencyLayers;
    public int[] activeLayers;

    public bool resetFlag = true;

    private void Update()
    {
        if(TrialManager.Instance != null)
        {
            if(TrialManager.Instance.activeLayers != null)
            {

                activeLayers = TrialManager.Instance.activeLayers;
            }
        }

        if (resetFlag)
        {
            for (int i = 0; i < transparencyLayers.Length; i++)
                transparencyLayers[i].SetActive(false);

            resetFlag = false;
        }

        foreach (int i in activeLayers)
            transparencyLayers[i].SetActive(true);
    }
}
