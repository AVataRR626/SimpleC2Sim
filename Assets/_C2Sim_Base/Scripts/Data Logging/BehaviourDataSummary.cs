using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BehaviourDataSummary
{
    //event counters
    public int actions;
    public int interventions;

    //tracking agent behaviours
    public int landings;
    public int collisions;
    public int safeOverfly;
    public int dzCrashes;
    public int dzSafeIncursions;
    public int dzDangerousIncursions;
    public int airspaceExit;

    //tracking user behaviours
    public int divertCC;//good if higher; when someone diverts aircraft on a collision course; you should divert collision course assets   
    public int divertDZDI;//good if higher;  when someone diverts aircraft heading into a danger zone; you should divert dangerous incursions
    public int divertFCC;//good if lower; when someone diverts aircraft on a safe overfly course; you should not divert feigned collisions (they are safe)
    public int divertDZSI;//good if lower; when someone diverts aircraft heading into a safe danger zone incursion; you should not divert safe incursions    

    public int divertCancelDZDI;//good if lower; when someone cancels a correct danger zone diversion; you shouldn't cancel neccessary diversions
    public int divertCancelDZSI;//good if higher; when someone cancels a diversion for a safe danger zone incursion; you should cancel uneccessary diversions

    public int divertCancelFCCD;//good if higher; you should cancel an uneccessary diversion
    public int divertCancelDZUA;//good if higher; when someone cancels an incorrect danger zone diversoin; you should cancel uneccessary diversions (same as DZDI?)
    public int divertCancelDZNA;//good if lower; when someone cancels a correct danger zone diversion; you should not cancel these (same as DZSI?)
}
