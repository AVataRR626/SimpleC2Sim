using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DiversionZone : MonoBehaviour
{
    public Transform diversionPoint;
    public NavNode diversionPrefab;    
    public Vector3 spawnOffset = new Vector3(0, -10, 0);

    public List<C2Sim_Asset> acceptFilter;

    public NavNode NewDiversionPoint(string interventionString)
    {
        NavNode divertNode = Instantiate(diversionPrefab);
        divertNode.diversionSource = this;
        divertNode.transform.position = diversionPoint.position;
        divertNode.interventionString = interventionString;

        diversionPoint.transform.position += spawnOffset;        

        return divertNode;        
    }

    public void DecrementOffset()
    {
        diversionPoint.transform.position -= spawnOffset;
    }
}
