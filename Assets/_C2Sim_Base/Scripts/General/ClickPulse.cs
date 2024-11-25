using UnityEngine;
using System.Collections;

public class ClickPulse : MonoBehaviour
{
    public bool hoverPulse = true;
    public GameObject subject;

    private void Start()
    {
        if (subject == null)
            subject = gameObject;
    }

    void OnMouseDown()
    {
        subject.SendMessage("PulseUp");
    }

    void OnMouseUp()
    {
        subject.SendMessage("PulseDown");
    }

    private void OnMouseEnter()
    {
        subject.SendMessage("PulseUp");
    }

    private void OnMouseExit()
    {
        subject.SendMessage("PulseDown");
    }
}
