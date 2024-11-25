using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    [Header("Navigation Settings")]
    public NavNode nextNode;
    public bool permanent = false;
    public bool killAssetOnArrival = false;
    public float targetHeight = -1;
    public float targetForwardSpeed = -1;
    public bool areaExit = false;
    public bool scoreOnExit = false;

    [Header("Click Settings")]
    public bool killOnClick;
    public string onClickHideFilter = "pretrial";
    public bool onClickDefaultTimescale = true;
    public float defaultTimeScale = 1;

    [Header("Experiment Settings")]
    public string interventionString = "";
    public int diversionCost = -1;
    public GameObject diversionIndicator;
    public Vector3 indicatorSpawnOffset;
    

    [Header("System Connections")]
    public GameObject permanentIndicator;
    public LineConnector destinationIndicator;
    public DiversionZone diversionSource;

    private void Update()
    {
        if (permanentIndicator != null)
            permanentIndicator.SetActive(permanent);

        if(nextNode != null && destinationIndicator != null)
        {
            destinationIndicator.gameObject.SetActive(true);
            destinationIndicator.source = transform;
            destinationIndicator.destination = nextNode.transform;
        }
        else if(destinationIndicator !=  null)
        {
            destinationIndicator.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        NavNodeClick();
    }

    public void NavNodeClick()
    {
        ExperimentManager.Instance.ReportAction();
        FirebaseBridge.Instance.AddEvent("NavNodeClick" );

        Debug.Log("--------NavNode:OnMouseDown");

        if (onClickHideFilter.Length > 0)
            FilteredHideShow.Hide(onClickHideFilter);

        if (onClickDefaultTimescale)
            Time.timeScale = defaultTimeScale;

        if (killOnClick)
        {
            if (!permanent)
            {
                ExperimentManager.Instance.ReportIntervention(interventionString);

                if (diversionSource != null)
                {
                    diversionSource.DecrementOffset();

                    /*
                    //cancelling an incorrectly diverted asset
                    if (interventionString == "dzsi")
                        ExperimentManager.Instance.ReportIntervention("dzsi_dc");

                    //cancelling a correctly diverted asset
                    if (interventionString == "dzdi")
                        ExperimentManager.Instance.ReportIntervention("dzdi_dc");
                    */
                }

                if (diversionIndicator != null)
                    Instantiate(diversionIndicator, transform.position + indicatorSpawnOffset, Quaternion.identity);

                //this is to handle auto-diversions or pre spawned auto-diversions
                GameManager.Instance.AddScore(diversionCost);
                Destroy(gameObject);

            }
        }
        else
        {
            //This is for when you divert an aircraft by clicking on the aircraft,
            //and a new nav node is spawned
            if (ManualNavManager.Instance.subject != null)
            {
                ManualNavManager.Instance.subject.DestroyNavTarget();
                ManualNavManager.Instance.subject.myData.navTarget = this;
                ManualNavManager.Instance.subject.myData.isLanding = false;
                ManualNavManager.Instance.subject.myData.isExitingArea = areaExit;

                if (diversionIndicator != null)
                    Instantiate(diversionIndicator,transform.position + indicatorSpawnOffset, Quaternion.identity);

                GameManager.Instance.AddScore(diversionCost);

                ExperimentManager.Instance.ReportIntervention(
                    ManualNavManager.Instance.subject.myData.interventionString);






                //--FINAL STEP---
                //and then prep for next selection as the final step
                ManualNavManager.Instance.subject = null;
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log("NavNode: Deleted");
    }
}
