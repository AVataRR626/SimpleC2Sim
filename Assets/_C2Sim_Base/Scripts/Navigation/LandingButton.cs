using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingButton : MonoBehaviour
{
    public LandingZone myLZ;

    private void OnMouseDown()
    {
        if (ManualNavManager.Instance.subject != null)
        {
            FirebaseBridge.Instance.AddEvent("CLND:" + ManualNavManager.Instance.subject.myData.unitId 
                + ";t:" + ManualNavManager.Instance.subject.myData.unitType);
            ManualNavManager.Instance.subject.InitiateLanding(myLZ);
            ManualNavManager.Instance.subject = null;
        }
    }
}
