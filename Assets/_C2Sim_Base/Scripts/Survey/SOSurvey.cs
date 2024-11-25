using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object for survey..
//https://docs.unity3d.com/Manual/class-ScriptableObject.html
//
// Enter possible responses in response array i.e.
//      [0] Strongly Disagree
//      [1] Disagree... etc.
//
// Enter actual questions questionsarray
//
//

[CreateAssetMenu(fileName = "Survey", menuName = "Survey", order = 1)]
public class SOSurvey : ScriptableObject
{
    public string surveyStatement;
    public string answerPrompt;

    public string [] responses;
    public string [] questions;
}
