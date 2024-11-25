using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScenarioFrame
{
    public List<C2Sim_Asset_Data> assetList;

    public ScenarioFrame()
    {
        assetList = new List<C2Sim_Asset_Data>();
    }
}
