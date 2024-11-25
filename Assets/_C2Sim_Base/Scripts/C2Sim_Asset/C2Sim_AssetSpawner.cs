using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Sim_AssetSpawner : MonoBehaviour
{
    public bool isSpawning = true;
    public float spawnClock;
    public float baseSpawnInterval;
    public float randSpawnInterval;    
    public C2Sim_Asset [] assetPrefabs;
    public Transform [] spawnPoints;
    public NavNode [] navNodes;

    public int spawnType;
    public int spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isSpawning)
        {
            if (spawnClock > 0)
            {
                spawnClock -= Time.fixedDeltaTime;
            }
            else
            {
                spawnClock = baseSpawnInterval + Random.Range(0.0f, randSpawnInterval);
                Spawn();
            }
        }
    }

    public void Spawn() 
    {
        if (isSpawning)
        {
            spawnType = Random.Range(0, assetPrefabs.Length - 1);
            spawnPoint = Random.Range(0, spawnPoints.Length - 1);

            C2Sim_Asset newAsset = Instantiate(assetPrefabs[spawnType], spawnPoints[spawnPoint].position, Quaternion.identity);
            newAsset.myData.targetHeading = spawnPoints[spawnPoint].rotation.eulerAngles.y;

            if (navNodes[spawnPoint] != null)
                newAsset.myData.navTarget = navNodes[spawnPoint];

            //newAsset.transform.Rotate(0, newAsset.targetHeading, 0);
        }
    }

}
