using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LossChecker : MonoBehaviour
{
    public bool tutorialErrorMode = true;
    public int loseLimit = 3;
    public UnityEvent noLosses;
    public UnityEvent yesLosses;    

    public bool winFlag = false;
    public int loseCount;

    public GameObject practiceButton;
    public GameObject continueButton;
    //Repeat Button managed in InstructionLimitCheck.cs

    private void Start()
    {
        if (ExperimentManager.Instance.practiceRepeatLimit > 0)
            loseLimit = ExperimentManager.Instance.practiceRepeatLimit;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("LossChecker.OnEnable() ----- ");
        Check();
    }

    public void Check()
    {
        if (GameManager.Instance != null)
        {
            if (!tutorialErrorMode)
                winFlag = CheckNoLosses();
            else
               winFlag = (CheckNoTutorialError() && CheckNoLosses());

            Debug.Log("LossChecker:Check() " + winFlag);


            if(loseCount >= loseLimit)
            {
                winFlag = true;
                ExperimentManager.Instance.TrainingFailFlag("training losses: " + loseCount);
            }


            if (winFlag)
            {
                noLosses.Invoke();
            }
            else
            {
                yesLosses.Invoke();
                loseCount++;
            }

            //disallow practice when try limit reached.
            if(loseCount >= loseLimit)
            {
                practiceButton.SetActive(false);
                //prevent infinite loop, simply allow people to continue.
                continueButton.SetActive(true);
            }
        }
    }

    public bool CheckNoLosses()
    {
        Debug.Log("LossChecker:CheckLosses() " + (GameManager.Instance.losses <= 0));
        return GameManager.Instance.losses <= 0;
    }

    public bool CheckNoTutorialError()
    {
        Debug.Log("LossChecker:CheckTutorialError() " + (GameManager.Instance.tutorialError <= 0));
        return GameManager.Instance.tutorialError <= 0;
    }

    public void CheckLossesEvent()
    {
        if (CheckNoLosses())
        {
            noLosses.Invoke();
            Debug.Log("LossChecker: ---- No Losses");
        }
        else
        {
            yesLosses.Invoke();
            Debug.Log("LossChecker: ---- Yes Losses");
        }

    }

    public void CheckTutorialErrorEvent()
    {
        if (CheckNoTutorialError())
        {
            noLosses.Invoke();
            Debug.Log("LossChecker: ---- No Losses");
        }
        else
        {
            yesLosses.Invoke();
            Debug.Log("LossChecker: ---- Yes Losses");
        }
    }
}
