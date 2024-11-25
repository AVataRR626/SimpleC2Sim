using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{

    public static List<DangerZone> Instances;

    public string dzName = "template";
    public float damageChance = 10;//per second chance percentage of being damaged while inside...
    public float damage = 10;
    public float speedThreshold = 30;


    public void Start()
    {
        if(Instances == null)
        {
            Instances = new List<DangerZone>();
        }

        Instances.Add(this);
    }
}
