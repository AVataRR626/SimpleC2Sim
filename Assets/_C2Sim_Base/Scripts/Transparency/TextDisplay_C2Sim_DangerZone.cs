using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay_C2Sim_DangerZone : MonoBehaviour
{
    public Text txt;
    public DangerZone myDangerZone;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (TextBillboardRotationRef.Instance != null)
            transform.rotation = TextBillboardRotationRef.Instance.transform.rotation;

        if (txt == null)
            txt = GetComponent<Text>();

        if (offset == Vector3.zero)
            offset = transform.position - myDangerZone.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (myDangerZone != null && txt != null)
        {
            txt.text = "min safe\nspeed: " + myDangerZone.speedThreshold;
        }
    }
}
