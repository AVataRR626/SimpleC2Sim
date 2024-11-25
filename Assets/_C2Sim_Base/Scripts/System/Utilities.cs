using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Utilities
{
    //From:
    //https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net
    public static T DeepClone<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }

    public static void ShuffleArray(ref int [] arr, int startIndex)
    {
        long rSeed = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        Random.InitState((int)rSeed);

        for (int i = startIndex; i < arr.Length - 1; i++)
        {
            int swapIndex = Random.Range(i + 1, arr.Length);

            //Debug.Log("SwapIndex: " + swapIndex);

            int swapVal = arr[swapIndex];

            arr[swapIndex] = arr[i];
            arr[i] = swapVal;
        }
    }

    public static void ShuffleArray(ref int[] arr, int startIndex, int endIndex )
    {
        //long rSeed = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        //Random.InitState((int)rSeed);

        for (int i = startIndex; i < endIndex; i++)
        {
            int swapIndex = Random.Range(i, endIndex+1);

            //Debug.Log("SwapIndex: " + swapIndex);

            if (swapIndex != i)
            {
                int swapVal = arr[swapIndex];
                arr[swapIndex] = arr[i];
                arr[i] = swapVal;
            }
        }
    }

    public static void ShuffleBlocks(ref int[] arr, ref Block [] blocks)
    {
        //long rSeed = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        //Random.InitState((int)rSeed);

        for (int i = 0; i < blocks.Length - 1; i++)
        {
            int swapIndex = Random.Range(i, blocks.Length);

            if(swapIndex != i)
                SwapBlocks(ref arr, blocks[i], blocks[swapIndex]);
        }
    }

    public static void PartialBlockShuffle(ref int[] arr, ref Block[] blocks, int startIndex)
    {
        //long rSeed = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        //Random.InitState((int)rSeed);

        for (int i = startIndex; i < blocks.Length - 1; i++)
        {
            int swapIndex = Random.Range(i, blocks.Length);

            if (swapIndex != i)
                SwapBlocks(ref arr, blocks[i], blocks[swapIndex]);
        }
    }

    public static void SwapBlocks(ref int[] arr, Block b1, Block b2)
    {
        if(b1.BlockSize() == b2.BlockSize())
        {
            int[] temp = new int[b1.BlockSize()];

            //save items from block 1
            for(int i = b1.blockStartIndex; i < b1.blockEndIndex + 1; i++)
            {
                for (int j = 0; j < temp.Length; j++)
                {
                    //Debug.Log(" i:" + i + " j:" + j);
                    temp[j]= arr[i];
                    i++;
                }
            }

            //copy block 2 items into block 1
            for (int i = b1.blockStartIndex; i < b1.blockEndIndex + 1; i++)
            {
                for(int j = b2.blockStartIndex; j < b2.blockEndIndex + 1; j++)
                {
                    arr[i] = arr[j];
                    i++;
                }
            }

            //copy saved items back into block 2
            //copy block 2 items into block 1
            for (int i = 0; i < temp.Length + 1; i++)
            {
                for (int j = b2.blockStartIndex; j < b2.blockEndIndex + 1; j++)
                {
                    arr[j] = temp[i];
                    i++;
                }
            }

        }
        else
        {
            Debug.LogError("Utilities::SwapBlocks() - block size mismatch: " + b1.blockName + " and " + b2.blockName);
        }
    }
        
}
