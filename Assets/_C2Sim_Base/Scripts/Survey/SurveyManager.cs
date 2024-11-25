using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurveyManager : MonoBehaviour
{
    [Header("Survey Parameters")]
    public SOSurvey mySurvey;
    public SurveyQuestion questionPrefab;
    public string gameScene = "EMnt";

    [Header("UI Settings")]
    public string defaultAnswerText;
    public bool proceedOnCompletion = true;
    public Text txtTitle;
    public GameObject proceedButton;
    public GameObject pleaseCompleteSign;
    public Transform questionRoot;
    public Vector3 spawnOffset = new Vector3(0, 5, 0);
    public float baseHeight = 100;
    public float questionHeightFactor = 20;
    public RectTransform rectTransform;

    [Header("Runtime")]
    public SurveyResults results;
    public List<SurveyQuestion> questionList;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        txtTitle.text = mySurvey.surveyStatement;

        if(mySurvey.answerPrompt.Length > 0)
            defaultAnswerText = mySurvey.answerPrompt;

        GenerateQuestions();
        CheckCompletion();
        AdjustScrollHeight();
    }

    [ContextMenu("Adjust Height")]
    public void AdjustScrollHeight()
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = baseHeight + questionHeightFactor * questionList.Count;
        rectTransform.sizeDelta = sizeDelta;
    }

    public bool IsComplete()
    {
        for(int i = 0; i < questionList.Count; i++)
        {
            //hasn't been answered, so....
            if (questionList[i].answerIndex == 0)
                return false;
        }

        return true;
    }

    public void CheckCompletion()
    {
        if (proceedOnCompletion)
        {
            proceedButton.SetActive(IsComplete());
            pleaseCompleteSign.SetActive(!IsComplete());
        }
    }

    public void GenerateQuestions()
    {
        Vector3 spawnPos = Vector3.zero;
        for(int i = 0; i < mySurvey.questions.Length; i++)
        {
            SurveyQuestion newQuestion = Instantiate(questionPrefab, questionRoot.position + spawnPos, Quaternion.identity);
            newQuestion.myManager = this;
            newQuestion.SyncQuestion(mySurvey.questions[i], i);
            newQuestion.SyncAnswers(mySurvey.responses);
            newQuestion.transform.parent = questionRoot;
            newQuestion.transform.localScale = new Vector3(1, 1, 1);
            questionList.Add(newQuestion);
            spawnPos += spawnOffset;
        }

        //push proceed button downwards
        Vector3 buttonPos = questionRoot.position + spawnPos + spawnOffset;
        buttonPos.x = proceedButton.transform.position.x;
        proceedButton.transform.position = buttonPos;
        pleaseCompleteSign.transform.position = buttonPos;
    }

    [ContextMenu("GatherAnswers")]
    public void GatherAnswers()
    {
        results = new SurveyResults(mySurvey.name, mySurvey.surveyStatement, questionList.Count, ExperimentManager.Instance.prevScene);

        results.name = mySurvey.name;

        for(int i = 0; i < questionList.Count; i++)
        {
            questionList[i].SyncAnswerIndex();
            results.answers[i] = questionList[i].answerIndex;
        }
        
        FirebaseBridge.Instance.AddSurvey(results);
    }

    public void Proceed()
    {
        GatherAnswers();
        
        if (gameScene.Length > 0)
        {
            if (gameScene == "EMnt")
            {
                if (ExperimentManager.Instance != null)
                {
                    ExperimentManager.Instance.NextTrial();
                }
            }
            else
                SceneManager.LoadScene(gameScene);
        }
    }
}
