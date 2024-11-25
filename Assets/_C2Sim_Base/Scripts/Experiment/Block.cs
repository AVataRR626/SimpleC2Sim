using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Block
{
    public string blockName;
    public int blockStartIndex = 0;//starting index
    public int blockEndIndex = 0;
    public int shuffleStartOffset = 0;

    public int BlockSize()
    {
        return (blockEndIndex - blockStartIndex) + 1;
    }
}
