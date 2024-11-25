using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialErrorReporter : MonoBehaviour
{

    public bool reportOnStart = false;
    public bool reportOnEnable = true;
    public bool reportOnClick = false;
    public bool decrementOnClick = false;

    // Start is called before the first frame update
    void Start()
    {
        if(reportOnStart)
            ReportTutorialError();
    }

    private void OnEnable()
    {
        if (reportOnEnable)
            ReportTutorialError();
    }

    public void ReportTutorialError()
    {
        Debug.Log("TutorialErrorReporter.ReportTutorialError()");
        GameManager.Instance.tutorialError++;
    }

    public void OnMouseDown()
    {
        if (reportOnClick)
            GameManager.Instance.tutorialError++;

        if (decrementOnClick)
            GameManager.Instance.tutorialError--;
    }
}
