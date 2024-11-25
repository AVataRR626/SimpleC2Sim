using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Sim_Asset_CollisionAvoidance : MonoBehaviour
{
    [Header("Behaviour Parameters")]
    public float evasionDistance = 100;
    public float evasionHeight = 50;
    public float followThreshold = 0.1f;
    public bool evading = true;

    [Header("System Settings")]
    public C2Sim_Radar myRadar;
    public C2Sim_Asset myAsset;
    public CollisionIndicator collisionIndicatorPrefab;
    public NavNode navNodePrefab;
    public NavNode currentEvadeNode;

    private void Start()
    {
        myRadar.onEnter.AddListener(NewContactAlert);
    }

    public void NewContactAlert()
    {
        if(evading)
            EvaseiveAction(myRadar.contactList[0]);
    }

    public void EvaseiveAction(C2Sim_Asset hazard)
    {   

        Vector3 evasionPoint = transform.position;

        if(IsLeft(transform, hazard.transform))
        {
            evasionPoint += transform.right * evasionDistance;
        }
        else
        {
            evasionPoint -= transform.right * evasionDistance;
        }

        if (evasionPoint.x > transform.position.x)
            myAsset.myData.targetHeight += evasionHeight;
        else
            myAsset.myData.targetHeight -= evasionHeight;

        if (currentEvadeNode != null)        
            Destroy(currentEvadeNode.gameObject);

        currentEvadeNode = Instantiate(navNodePrefab, evasionPoint, Quaternion.identity);

        if (myAsset.myData.navTarget != null)
        {
            currentEvadeNode.nextNode = myAsset.myData.navTarget;
        }

        myAsset.myData.navTarget = currentEvadeNode;
    }

    public static bool IsLeft(Transform subject, Transform hazard)
    {
        Vector3 direction = hazard.position - subject.position;
        Vector3 cross = Vector3.Cross(subject.forward, direction);
        return cross.y < 0;
    }

}
