using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public float maxTime = 10;
    public float landingHeight = 25;
    public Color[] heighColours;
    public float[] heightThresholds;
    public UnityEvent onGameEnd;

    [Header("Performance Tracking")]
    public int score;
    public int losses;
    public Timer timer;
    public string scorePrefix = "score: ";
    public string lossCountPrefix = "losses: ";
    public Text txtScore;
    public int tutorialError;

    [Header("System Stuff")]
    public bool lockFlag = false;
    public Text heightLegend;
    public string heightLegendHeader = "height colour legend:";
    public float unscaledTrialClock = 0;
    public float gameTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        if(TrialManager.Instance.trialTime > 0)
            maxTime = TrialManager.Instance.trialTime;

        gameTimestamp = Time.time;
    }

    private void Update()
    {
        txtScore.text = scorePrefix + score + "\n" + lossCountPrefix + losses;

        if (!lockFlag)
        {
            if (timer.time >= maxTime)
            {
                timer.timing = false;
                onGameEnd.Invoke();
                Time.timeScale = 0;
                lockFlag = true;
            }
        }

        if(heightLegend != null)
        {
            heightLegend.text = heightLegendHeader+"\n";

            string heightString = "";

            for (int i = heighColours.Length - 1; i >= 0; i--)
            {
                if (i == 0)
                    heightString = "(low) 0 to " + (heightThresholds[i]-1);
                else if (i < heightThresholds.Length)
                    heightString = heightThresholds[i - 1] + " to " + (heightThresholds[i] - 1);
                else
                    heightString = "(high) " + heightThresholds[heightThresholds.Length - 1] + " & above";

                heightLegend.text += "<color=#" + ColorUtility.ToHtmlStringRGB(heighColours[i]) + ">" + heightString + "</color>\n";
            }
        }

        TrialClockTick();
    }

    private void TrialClockTick()
    {
        unscaledTrialClock += Time.unscaledDeltaTime;        
    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }

    public void AddLoss(int lostScore)
    {
        losses += lostScore;
    }

    public void Reset()
    {
        score = 0;
        losses = 0;
        tutorialError = 0;
        lockFlag = false;
        timer.timing = true;
    }

    public void Reset(float newTime)
    {
        timer.SetTimer(newTime);
        Reset();
    }

    public void EndTrial()
    {
        TrialManager.Instance.EndTrial();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    [ContextMenu("Unpause")]
    public void UnPause()
    {
        Debug.Log("GameManager:Unpause()");
        Time.timeScale = 1;
    }

    public void ReloadLevel()
    {
        UnPause();
        Invoke("InstantReload", 0.2f);
    }

    public void ReloadLevel(bool trainingRepeat)
    {
        if (trainingRepeat)        
            ExperimentManager.Instance.IncrementTrainingFullRepeatCount();

        ReloadLevel();
    }

    public void InstantReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReportIntervention(string interventionString)
    {
        TrialManager.Instance.ReportIntervention(unscaledTrialClock, interventionString);
    }

    public void ReportInteraction(string interactionString)
    {
        TrialManager.Instance.ReportInteraction(unscaledTrialClock, interactionString);
    }

    public void ResetUnscaledTrialClock()
    {
        unscaledTrialClock = 0;
    }
}
