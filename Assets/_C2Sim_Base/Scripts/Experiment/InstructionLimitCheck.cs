using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionLimitCheck: MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.SetActive(ExperimentManager.Instance.EnoughInstructionRepeats());
    }
}
