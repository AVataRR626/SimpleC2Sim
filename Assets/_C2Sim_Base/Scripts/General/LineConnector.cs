using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineConnector : MonoBehaviour
{
    public Transform source;
    public Transform destination;
    public LineRenderer myLineRenderer;
    public bool yMatch = false;

    // Start is called before the first frame update
    void Start()
    {
        if (myLineRenderer == null)
            myLineRenderer = GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(source != null && destination != null)
        {
            myLineRenderer.SetPosition(0, source.position);

            Vector3 destPos = destination.position;

            if (yMatch)
                destPos.y = source.position.y;

            myLineRenderer.SetPosition(1, destPos);
        }
        else
        {
            myLineRenderer.SetPosition(0,transform.position);
            myLineRenderer.SetPosition(1, transform.position);
        }
    }
}
