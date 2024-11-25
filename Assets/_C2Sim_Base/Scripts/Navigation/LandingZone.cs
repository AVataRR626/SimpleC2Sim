using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingZone : MonoBehaviour
{
    public NavNode landingStart;
    public bool autoLand = false;
    public float congestionMargin = 4;
    public float congestionClock;
    public C2Sim_Radar myRadar;

    public int CongestionCount()
    {
        return myRadar.contactList.Count;
    }

    public void EnableAutoLand()
    {
        autoLand = true;
    }

    private void Update()
    {

        if (congestionClock > 0)
            congestionClock -= Time.deltaTime;
        else
            congestionClock = 0;
    }

    public void CongestionClockIncrement()
    {
        congestionClock += congestionMargin;
    }
}
