using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIndicator_Line : CollisionIndicator
{
    public LineConnector myConnector;

    private void Start()
    {
        if (myConnector == null)
            myConnector = GetComponent<LineConnector>();
    }
    
    public override void SetAssets(C2Sim_Asset a, C2Sim_Asset b)
    {
        asset_a = a;
        asset_b = b;
        myConnector.source = a.transform;
        myConnector.destination = b.transform;
    }
}
