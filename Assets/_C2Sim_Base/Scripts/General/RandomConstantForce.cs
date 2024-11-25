using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomConstantForce : MonoBehaviour
{
    public Vector3 randomRange = new Vector3(20, 0, 20);
    public ConstantForce myConstantForce;

    // Start is called before the first frame update
    void Start()
    {
        if (myConstantForce == null)
            myConstantForce = GetComponent<ConstantForce>();

        Vector3 forceVector = Vector3.zero;
        forceVector.x = Random.Range(-randomRange.x, randomRange.x);
        forceVector.y = Random.Range(-randomRange.y, randomRange.y);
        forceVector.z = Random.Range(-randomRange.z, randomRange.z);

        myConstantForce.force = forceVector;
    }
}
