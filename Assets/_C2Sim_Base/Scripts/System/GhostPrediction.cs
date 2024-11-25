using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPrediction : MonoBehaviour
{
    public static int regularAssetLayer = 10;
    public static int ghostAssetLayer = 9;
    public static GhostPrediction Instance;

    public CollisionIndicator collisionIndicatorPrefab;
    public bool pauseOnCollide = false;
    public int frameProjection = 100;
    public C2Sim_Asset[] ghostClones;
    public C2Sim_Asset[] referenceAssets;

    public List<string> collisionLog;

    public bool pauseFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        collisionLog = new List<string>();

        //CloneGhosts();
    }

    public void GhostPredictSequence()
    {
        CloneGhosts();
        GhostProject();
    }

    private void FixedUpdate()
    {
        if (!pauseFlag)
        {
            if (collisionLog.Count == 0)
                GhostProject();
            else
            {
                if (pauseOnCollide)
                {
                    Time.timeScale = 0;
                    pauseFlag = true;
                }
            }
        }
    }

    [ContextMenu("GhostProject")]
    public void GhostProject()
    {
        //Debug.Log("Projecting " + frameProjection + " frames into the future");

        for (int i = 0; i < frameProjection; i++)
        {
            //stop once a collision is detected
            if(collisionLog.Count > 0)
            {
                Debug.Log("GHOST PREDICTION: Collisions detected at frame #" + i);

                foreach(string c in collisionLog)
                {
                    Debug.Log("----> " + c);
                }                
                break;
            }

            GhostFrame();
        }
    }

    public bool DistanceCollisionCheck(C2Sim_Asset a, C2Sim_Asset b)
    {
        float xDelta = Mathf.Abs(a.transform.position.x - b.transform.position.x);
        float yDelta = Mathf.Abs(a.transform.position.y - b.transform.position.y);
        float zDelta = Mathf.Abs(a.transform.position.z - b.transform.position.z);

        //Debug.Log("Manual Distance Check x:" + xDelta + " y:" + yDelta + " z:" + zDelta + " | " + a.myData.unitId + " & " + b.myData.unitId);

        if (xDelta <= a.myData.collisionSquare && 
            yDelta <= a.myData.collisionSquare &&
            zDelta <= a.myData.collisionSquare)
            return true;

        if (xDelta <= b.myData.collisionSquare &&
            yDelta <= b.myData.collisionSquare &&
            zDelta <= b.myData.collisionSquare)
            return true;

        return false;
    }

    [ContextMenu("GhostFrame")]
    public void GhostFrame()
    {
        //update all ghosts...
        for (int i = 0; i < ghostClones.Length; i++)
        {
            if(ghostClones[i] != null)
                ghostClones[i].FrameTick();
        }

        //pairwise collision detection:
        for(int i = 0; i < ghostClones.Length; i++)
        {
            for(int j = i; j < ghostClones.Length; j++)
            {
                if (i != j && ghostClones[i] != null && ghostClones[j] != null)
                {
                    if (DistanceCollisionCheck(ghostClones[i], ghostClones[j]))
                    {
                        collisionLog.Add("GHOST COLLISION: " + ghostClones[i].myData.unitId + " AND " + ghostClones[j].myData.unitId + " ("+i+","+j+")");

                        CollisionIndicator ci = Instantiate(collisionIndicatorPrefab);
                        ci.SetAssets(referenceAssets[i], referenceAssets[j]);
                    }
                }
            }
        }
    }
    
    [ContextMenu("GhostClone")]
    public void CloneGhosts()
    {
        collisionLog = new List<string>();

        referenceAssets = GameObject.FindObjectsOfType<C2Sim_Asset>();
        ghostClones = new C2Sim_Asset[referenceAssets.Length];

        for (int i = 0; i < ghostClones.Length; i++)
        {
            ghostClones[i] = Instantiate(referenceAssets[i]);
            ghostClones[i].GhostMode();
            ghostClones[i].FrameTick();
        }
    }

    public void DestroyClones()
    {
        foreach(C2Sim_Asset g in ghostClones)
        {
            Destroy(g.gameObject);
        }
    }
}
