using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay_C2Sim_Asset : MonoBehaviour
{
    public Text txt;
    public C2Sim_Asset myAsset;
    public Vector3 offset;
    public bool health = true;
    public bool value = true;
    public bool speed = true;
    public bool height = true;

    Quaternion startRotation;

    [ExecuteInEditMode]
    private void Start()
    {
        if (TextBillboardRotationRef.Instance != null)
            transform.rotation = TextBillboardRotationRef.Instance.transform.rotation;

        if (txt == null)
            txt = GetComponent<Text>();

        if(offset == Vector3.zero)
            offset = transform.position - myAsset.transform.position;

        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(myAsset != null && txt != null)
        {
            txt.text = "";

            if (myAsset.myData.isLanding)
            {
                txt.text = "<b>==LANDING==</b>" + "\n";

            }

            if(health)
                txt.text += "health: " + myAsset.myData.health + "\n";

            if(value)
                txt.text += "value: " + myAsset.myData.value + "\n";

            if (height)
                txt.text += "height: " + myAsset.transform.position.y.ToString("0") + "\n";

            if (speed)
                txt.text += "speed: " + myAsset.myData.forwardSpeed.ToString("0") + "\n";

            //txt.text += "heading: " + myAsset.transform.rotation.eulerAngles.y.ToString("0") + "\n";
            
            transform.position = myAsset.transform.position + offset;

            if (!myAsset.isActiveAndEnabled)
                gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
