using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIndicator : MonoBehaviour
{
    public C2Sim_Asset asset_a;
    public C2Sim_Asset asset_b;

    public virtual void SetAssets(C2Sim_Asset a, C2Sim_Asset b)
    {
        asset_a = a;
        asset_b = b;
    }
}
