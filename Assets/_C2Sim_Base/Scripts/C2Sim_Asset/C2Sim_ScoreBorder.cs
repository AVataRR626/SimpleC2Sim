using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Sim_ScoreBorder : MonoBehaviour
{

    public Collider myCollider;
    public float minimumLifetime = 0;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("SCOREBORDER: " + other.name);

        C2Sim_Asset asset = other.GetComponent<C2Sim_Asset>();

        if(asset != null)
        {
            if(asset.myData.lifeTime >= minimumLifetime)
            {
                GameManager.Instance.AddScore(asset.myData.value);
                ExperimentManager.Instance.ReportAirspaceExit();
                asset.SuccessAndExit();
            }
        }
    }
}
