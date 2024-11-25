using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Sim_Asset_GroupController : MonoBehaviour
{
    public float globalMaxSpeed = 100;
    public float globalSpeed = 100;
    public float globalDZThreshold = 90;
    public float scoreBorderMinLifetime = 10;
    public C2Sim_Asset[] allAssets;
    public DangerZone[] allDangerZones;
    public C2Sim_ScoreBorder[] allScoreBorders;

    // Start is called before the first frame update
    void Start()
    {   
        allAssets = FindObjectsOfType<C2Sim_Asset>();
        
        allDangerZones = FindObjectsOfType<DangerZone>();

        allScoreBorders = FindObjectsOfType<C2Sim_ScoreBorder>();
    }

    // Update is called once per frame
    void Update()
    {
        SyncValues();
    }

    public void SyncValues()
    {
        foreach(C2Sim_Asset a in allAssets)
        {
            a.myData.maxSpeed = globalMaxSpeed;
            a.myData.forwardSpeed = globalSpeed;
        }

        if (allDangerZones != null)
        {
            if (allDangerZones.Length > 0)
            {
                foreach(DangerZone dz in allDangerZones)
                {
                    dz.speedThreshold = globalDZThreshold;
                }
            }
        }

        foreach(C2Sim_ScoreBorder sb in allScoreBorders)
        {
            sb.minimumLifetime = scoreBorderMinLifetime;
        }

    }
}
