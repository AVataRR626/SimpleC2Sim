using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlighterUtil : MonoBehaviour
{
    public Renderer myRenderer;
    public bool highlighting = false;
    public string highlightKey = "_OutlineEnabled";
    public float highlightValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        //don't share materials at runtime! highlight each object individually
        //myRenderer.material.CopyPropertiesFromMaterial(myRenderer.material);
    }

    // Update is called once per frame
    void Update()
    {
        if(myRenderer != null)
        {
            if (highlighting)
                highlightValue = 1.0f;
            else
                highlightValue = 0.0f;

            
            myRenderer.sharedMaterial.SetFloat(highlightKey, highlightValue);
        }
    }
}
