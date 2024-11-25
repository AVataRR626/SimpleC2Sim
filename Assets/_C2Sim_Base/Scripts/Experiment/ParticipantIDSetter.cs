using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantIDSetter : MonoBehaviour
{
    public InputField participantID;

    public void SetParticipantID()
    {
        FirebaseBridge.Instance.participantID = participantID.text;
        FirebaseBridge.Instance.ed.participantID = participantID.text;
    }
}
