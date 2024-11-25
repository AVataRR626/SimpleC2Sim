using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C2Sim_isLanding_Action : MonoBehaviour
{
    public C2Sim_Asset myAsset;
    public UnityEvent onLanding;
    public bool actionFlag = false;

    void Update()
    {
        if(!actionFlag)
        {
            if(myAsset.myData.isLanding)
            {
                onLanding.Invoke();
                actionFlag = true;
            }
        }
    }
}
