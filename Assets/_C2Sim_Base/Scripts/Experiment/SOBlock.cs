using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOBlock : ScriptableObject
{
    public int blockStartIndex = 0;//starting index
    public int blockEndIndex = 0;
    public int [] trialOrder;

    public void ShuffleOrder()
    {
        Utilities.ShuffleArray(ref trialOrder, blockStartIndex, blockEndIndex);
    }
}
