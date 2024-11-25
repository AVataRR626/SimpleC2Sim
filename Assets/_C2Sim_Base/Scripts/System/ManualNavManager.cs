using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualNavManager : MonoBehaviour
{
    public static ManualNavManager Instance;
    public C2Sim_Asset subject;
    public NavNode navNodePrefab;
    public Camera myCam;
    public LayerMask layerMask;
    public int diversionCost = 1;

    [Header("Mouse Settings")]
    public GameObject divertMouseIcon;
    public Vector2 hotSpot;
    public CursorMode cursorMode = CursorMode.Auto;
    public bool cursorSetFlag = false;

    NavNode prevSpawnedNode;

    void Start()
    {
        Instance = this;

        if(myCam == null)
        {
            myCam = Camera.main;
        }
    }

    public void ClearMap()
    {

    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ClearSelections();

        if (Input.GetKeyDown(KeyCode.Space))
            ClearSelections();

        if (subject != null)
        {
            if (!cursorSetFlag)
            {
                divertMouseIcon.SetActive(true);
                cursorSetFlag = true;
            }
        }
        else
        {
            divertMouseIcon.SetActive(false);
            cursorSetFlag = false;
        }

        /*
         * keeping this here for future reference.
         * cursor switching code...
        if (subject != null)
        {
            if (!cursorSetFlag)
            {
                Cursor.SetCursor(divertMouseIcon, hotSpot, cursorMode);
                cursorSetFlag = true;
            }
        }
        else
        {
            Cursor.SetCursor(null, hotSpot, cursorMode);
            cursorSetFlag = false;
        }
        */
    }

    public void ClearSelections()
    {
        subject = null;
        prevSpawnedNode = null;
    }

    private void OnMouseDown()
    {
        ExperimentManager.Instance.ReportAction();

        if (subject != null)
        {
            ExperimentManager.Instance.ReportIntervention(subject.myData.interventionString);

            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            subject.InitHeading();

            if (subject.myData.isLanding)
                subject.AbortLanding();

            if (Physics.Raycast(ray, out hit, 10000, layerMask))
            {
                Debug.Log("ManualNavManager: clicked on: " + hit.point);

                if (subject.myData.navTarget != null)
                {
                    if (!subject.myData.navTarget.permanent)
                    {
                        FirebaseBridge.Instance.AddEvent("RDIR: " + subject.myData.unitId + " t:" + subject.myData.unitType);

                        //move current nav target to new clickpoint
                        subject.myData.navTarget.transform.position = hit.point;
                    }
                    else
                    {
                        NavNode nn = Instantiate(navNodePrefab, hit.point, Quaternion.identity);
                        FirebaseBridge.Instance.AddEvent("RDIR: " + subject.myData.unitId + " t:" + subject.myData.unitType);

                        nn.targetHeight = subject.myData.targetHeight;
                        if (!subject.myData.navTarget.areaExit)
                        {
                            nn.nextNode = subject.myData.navTarget;
                            subject.myData.diversionFlag = true;
                        }
                        else
                        {
                            nn.nextNode = subject.myData.navTarget.nextNode;
                        }
                        

                        subject.myData.navTarget = nn;
                        prevSpawnedNode = nn;
                    }
                }

                
                subject = null;
                GameManager.Instance.AddScore(-diversionCost);

            }
        }
    }
}
