using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalDisplay_C2Sim_DangerZone : MonoBehaviour
{
    public DangerZone myDangerZone;
    public LineRenderer safeSpeedIndicator;
    public float lengthFactor = 7;
    public Vector3 lineForward = new Vector3(1,0,0);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleSafeSpeedIndicator();
    }

    void HandleSafeSpeedIndicator()
    {
        safeSpeedIndicator.SetPosition(1,
            (lengthFactor * myDangerZone.speedThreshold * lineForward)
            );
    }
}
