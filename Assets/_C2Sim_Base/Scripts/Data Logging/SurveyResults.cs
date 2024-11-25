using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SurveyResults
{
    public string name;
    public string surveyStatement;
    public int[] answers;
    public string prevTrial;

    public SurveyResults(string newName, string newStatement, int size, string newPrevScene)
    {
        name = newName;
        surveyStatement = newStatement;
        answers = new int[size];

        prevTrial = newPrevScene;
    }
}
