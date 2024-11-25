using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyQuestion : MonoBehaviour
{
    public enum AnswerMode { Dropdown, Slider};

    [Header("General Settings")]
    public AnswerMode answerMode = AnswerMode.Dropdown;    
    public int answerIndex;
    public int questionIndex;    
    public Text question;    
    public List<string> options;
    public SurveyManager myManager;
    [Header("Dropdown Mode Specific")]
    public Dropdown answers;
    [Header("Slider Mode Specific")]
    public Slider slider;
    public Text answer;

    public void SyncAnswerIndex()
    {

        if (answerMode == AnswerMode.Dropdown)
            answerIndex = answers.value;
        else
            answerIndex = GetSliderAnswerIndex();

        myManager.CheckCompletion();
        SliderAnswerDisplay();
    }

    public int GetSliderAnswerIndex()
    {

        return (int)slider.value;
    }

    public void SyncQuestion(string qString, int qI)
    {
        questionIndex = qI;
        question.text = qString;

        if (myManager.defaultAnswerText.Length > 0)
            if (answer != null)
                answer.text = myManager.defaultAnswerText;
    }

    public void SliderAnswerDisplay()
    {
        if (answer != null)
        {
            if(answerIndex > 0)
                answer.text = options[answerIndex];
            else
                answer.text = myManager.defaultAnswerText;
        }
    }

    public void SyncAnswers(string [] answerText)
    {
        //prepare a list datastructure to contain all the question answers
        options = new List<string>();
        //first one is always blank, so we can tell if someone's done it or not
        options.Add("");
        //sync from survey settings..
        for (int i = 0; i < answerText.Length; i++)
        {
            string finalText = answerText[i];
            options.Add(finalText);
        }

        if (answerMode == AnswerMode.Dropdown)
        {
            //clear out the dropdown menu
            answers.ClearOptions();
            //now add them to the dropdown
            answers.AddOptions(options);
        }

        if(answerMode == AnswerMode.Slider)
        {
            slider.wholeNumbers = true;
            slider.minValue = 0;
            slider.maxValue = answerText.Length;
        }
    }
}
