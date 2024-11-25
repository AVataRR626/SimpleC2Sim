using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionDisplay_C2Sim_DangerZone : MonoBehaviour
{
    public List<Transform> speedIndicators;
    public DangerZone myDangerZone;
    public Transform startPoint;
    public Transform endPoint;
    public Vector3 indicatorDirection;
    public float speed = 3;
    public float restartDistance;

    // Start is called before the first frame update
    void Start()
    {
        if (myDangerZone != null)
            speed = myDangerZone.speedThreshold;

        indicatorDirection = (endPoint.position - startPoint.position).normalized;        
        restartDistance = Vector3.Distance(endPoint.position, startPoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        indicatorDirection = endPoint.position - startPoint.position;
        speedIndicator.transform.position = indicatorDirection * myDangerZone.speedThreshold;
        */

        if (myDangerZone != null)
            speed = myDangerZone.speedThreshold;

        foreach (Transform si in speedIndicators)
        {
            si.transform.localPosition += (indicatorDirection * speed * Time.deltaTime);

            if (Vector3.Distance(si.transform.position, startPoint.position) >= restartDistance)
                si.transform.position = startPoint.position;
        }
    }
}
