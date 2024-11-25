using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance;
    public int trialIndex = -1;

    [Header("Trial Settings")]
    public bool shuffleAtStart = false;
    public int shuffleStartIndex = 2;//To avoid confusion, this should match the first block shuffle index
    //NOTE: the block shuffling will take precedence over this.
    //If block shuffling is enabled, this variable is cosmetic.
    //It is what the "X trial of Y trials" message is based on.

    public int[] trialOrder;//trial order not considering trialIndexStart
    //i.e. the trial order if trials start at BUILD INDEX 0

    [Header("Block Settings")]
    public bool shuffleBlocksAtStart = false;
    public int shuffleBlockStartIndex = 0;
    public Block[] blocks;
    public List<int> surveyIndicies;//list of trial indicies where surveys should appear
    public List<int> blockIntroIndicies;

    [Header("Experiment Settings")]
    public string preGameSureyScene;
    public string interTrialSurveyScene;
    public string experimentEndScreen;
    public string experimentString;

    [Header("Training Limits")]
    public int instructionRepeatLimit = 2;
    public int practiceRepeatLimit = 3;

    [Header("System Stuff - Usually Don't Touch")]
    public int trialIndexStart = 4;//the first BUILD INDEX where trials start
    public string prevScene;
    public bool debugMode = false;
    public KeyCode debugEndKey = KeyCode.End;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

            //sync experiment string
            FirebaseBridge.Instance.experimentString = experimentString;

            if (shuffleAtStart)
            {
                Shuffle();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (debugMode)
        {
            if (Input.GetKeyDown(debugEndKey))
            {
                TrialManager.Instance.EndTrial();
            }
        }
    }

    public void Shuffle()
    {
        if (blocks.Length > 0)
        {
            //shuffle trials in blocks
            for (int i = 0; i < blocks.Length; i++)
            {
                Utilities.ShuffleArray(ref trialOrder, blocks[i].blockStartIndex + blocks[i].shuffleStartOffset, blocks[i].blockEndIndex);
                surveyIndicies.Add(blocks[i].blockEndIndex);
                blockIntroIndicies.Add(blocks[i].blockStartIndex);
            }

            //then shuffle the blocks
            if (shuffleBlocksAtStart)
            {
                Utilities.PartialBlockShuffle(ref trialOrder, ref blocks, shuffleBlockStartIndex);
            }
        }
        else
        {
            Utilities.ShuffleArray(ref trialOrder, shuffleStartIndex);
        }

        for (int i = 0; i < trialOrder.Length; i++)
        {
            int trialIndex = trialIndexStart + trialOrder[i];
            string fullScenePath = SceneUtility.GetScenePathByBuildIndex(trialIndex);
            fullScenePath = fullScenePath.Substring(fullScenePath.Length - 32, 32);
            FirebaseBridge.Instance.ReportTrialSequence(trialIndex + " :" + fullScenePath);
        }
    }

    public bool EnoughInstructionRepeats()
    {
        return FirebaseBridge.Instance.ed.trainingFullRepeatCount < practiceRepeatLimit;
    }

    public void IncrementTrainingFullRepeatCount()
    {
        FirebaseBridge.Instance.IncrementTrainingFullRepeatCount();
    }

    public void TrainingFailFlag(string message)
    {
        FirebaseBridge.Instance.TrainingFailFlag(message);
    }

    public void SwapBlocks(int biA, int biB)
    {
        Utilities.SwapBlocks(ref trialOrder, blocks[biA], blocks[biB]);
    }

    [ContextMenu("SwalBlocksTest")]
    public void SwapBlocksTest()
    {
        SwapBlocks(0, 1);
    }

    public void LoadPreGameSurvey()
    {
        SceneManager.LoadScene(preGameSureyScene);
    }

    public void LoadSurveyScene()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(interTrialSurveyScene);
    }

    public void NextTrial()
    {
        prevScene = SceneManager.GetActiveScene().name;

        FirebaseBridge.Instance.PostToDatabase();
        trialIndex++;

        if (trialIndex < trialOrder.Length)
            SceneManager.LoadScene(trialIndexStart + trialOrder[trialIndex]);
        else
            EndExperiment();
        
    }

    public void EndExperiment()
    {
        FirebaseBridge.Instance.PostToDatabase();
        //FirebaseBridge.Instance.PostWithEventLogCache();
        SceneManager.LoadScene(experimentEndScreen);
    }

    public void ReportAction()
    {
        Debug.Log("ExperimentManager: ReportAction:" + trialIndex);
        FirebaseBridge.Instance.ReportAction(trialIndex);
    }

    public void ReportIntervention(string interventionString)
    {
        FirebaseBridge.Instance.ReportIntervention(trialIndex, interventionString);            
    }

    public void ReportIntervention(string interventionString, string interventionType)
    {
        FirebaseBridge.Instance.ReportIntervention(trialIndex, interventionString, interventionType);
    }

    public void ReportIntervention(string interventionString, string interventionType, string agentName, string agentType)
    {
        FirebaseBridge.Instance.ReportIntervention(trialIndex, interventionString, interventionType, agentName, agentType);
    }

    public void ReportIntervention(float timestamp, string interventionString)
    {
        FirebaseBridge.Instance.ReportIntervention(trialIndex, timestamp, interventionString);
    }

    public void ReportInteraction(float timestamp, string interactionString)
    {
        FirebaseBridge.Instance.ReportInteraction(trialIndex, timestamp, interactionString);
    }

    public void ReportAirspaceExit()
    {
        FirebaseBridge.Instance.ReportAirspaceExit(trialIndex);
    }

    public void ReportLanding()
    {
        FirebaseBridge.Instance.ReportLanding(trialIndex);
    }

    public void ReportSafeOverfly()
    {
        FirebaseBridge.Instance.ReportSafeOverfly(trialIndex);
    }

    public void ReportCollision()
    {
        FirebaseBridge.Instance.ReportCollision(trialIndex);
    }

    public void ReportDZCrash()
    {
        FirebaseBridge.Instance.ReportDZCrash(trialIndex);
    }

    public void ReportDZDangerousIncursion()
    {
        FirebaseBridge.Instance.ReportDZDangerousIncursion(trialIndex);
    }

    public void ReportDZSafeIncursion()
    {
        FirebaseBridge.Instance.ReportDZSafeIncursion(trialIndex);
    }

}
