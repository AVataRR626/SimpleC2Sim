using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionSystem : MonoBehaviour
{

    public List<ScenarioFrame> scenarioFrames;

    public C2Sim_Asset [] referenceAssets;

    public GameObject[] transformScratchList;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PredictionSystem: Time.FixedDeltaTime => " + Time.fixedDeltaTime);

        Init();
    }

    public void Init()
    {
        RegisterMAHVSAsses();
    }    

    public void RegisterMAHVSAsses()
    {
        referenceAssets = GameObject.FindObjectsOfType<C2Sim_Asset>();

        //generate scratch transform objects
        transformScratchList = new GameObject[referenceAssets.Length];
        for (int i = 0; i < transformScratchList.Length; i++)
        {
            transformScratchList[i] = new GameObject();
            transformScratchList[i].transform.position = referenceAssets[i].transform.position;
            transformScratchList[i].transform.rotation = referenceAssets[i].transform.rotation;
        }

        //copy asset data to first frame
        scenarioFrames = new List<ScenarioFrame>();
        ScenarioFrame newFrame = new ScenarioFrame();        
        for(int i = 0; i < referenceAssets.Length; i++)
        {
            Debug.Log("copying frame data: " + i);
            C2Sim_Asset_Data newData = referenceAssets[i].myData.CopySelf();
            newFrame.assetList.Add(newData);
        }
        scenarioFrames.Add(newFrame);
    }

    
    [ContextMenu("CalculateFrame")]
    public void CalculateFrameTest()
    {
        CalculateFrame(scenarioFrames.Count-1);        
        
    }
    public void CalculateFrame(int referenceFrameIndex)
    {
        ScenarioFrame newFrame = new ScenarioFrame();

        //copy data from reference frame
        for(int i = 0; i < scenarioFrames[referenceFrameIndex].assetList.Count; i++)
        {
            newFrame.assetList.Add
                (scenarioFrames[referenceFrameIndex].assetList[i].CopySelf());
        }

        //calculate "next frame" information
        for(int i = 0; i < newFrame.assetList.Count; i++)
        {
            //position calculations
            Vector3 horizontal = newFrame.assetList[i].HorizontalVelocity(transformScratchList[i].transform);//transform.forward * myData.forwardSpeed;
            Vector3 vertical = newFrame.assetList[i].VerticalVelocity(transformScratchList[i].transform);


            transformScratchList[i].transform.position =
                transformScratchList[i].transform.position + 
                    (horizontal)
                    + (vertical);

            newFrame.assetList[i].position = transformScratchList[i].transform.position;

            newFrame.assetList[i].HorizontalAccelerate();
            newFrame.assetList[i].VerticalAccelerate(transformScratchList[i].transform);

            //heading calculations
        }

        scenarioFrames.Add(newFrame);
    }
    
}
