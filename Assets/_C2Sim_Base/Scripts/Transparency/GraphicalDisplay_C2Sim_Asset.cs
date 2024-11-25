using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicalDisplay_C2Sim_Asset : MonoBehaviour
{
    [Header("Graphics Settings")]
    public C2Sim_Asset assetRef;
    public LineRenderer flightVector;
    public LineRenderer [] partialVectors;
    public LineRenderer dzSpeedLimitIndicator;
    public float lengthFactor = 7;
    public float descentThreshold = 0.1f;
    public GameObject ascendingIcon;
    public GameObject descendingIcon;
    public float flashModulus = 10;
    public GameObject landingIcon;
    public Image healthDisplay;
    public Image valueDisplay;

    [Header("System")]
    public Vector3 prevPos;
    public float heightDelta;
    public Vector3 offset;
    public float speedLimit = 0;
    public DangerZone closestDZ;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.localPosition;

        transform.parent = null;

        if (DiageticRotationRef.Instance != null)
            transform.rotation = DiageticRotationRef.Instance.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (assetRef != null)
        {

            if (!assetRef.isActiveAndEnabled)
                gameObject.SetActive(false);

            TrackSubject();
            HandleAscendingDescendingIcon();
            HandleFlightVector();
            HandleLandingIcon();
            HandleHealthImage();
            HandleValueImage();

            prevPos = transform.position;
        }
        else
            Destroy(gameObject);
    }

    public void HandleHealthImage()
    {
        float healthRatio = assetRef.myData.health / assetRef.myData.maxHealth;
        healthDisplay.fillAmount = healthRatio;
    }

    public void HandleValueImage()
    {
        float fillRatio = (float)assetRef.myData.value / (float)assetRef.myData.maxValue;
        valueDisplay.fillAmount = fillRatio;
    }

    void HandleAscendingDescendingIcon()
    {
        if(Mathf.Abs(assetRef.myData.verticalSpeed) > descentThreshold)
        {
            bool flashFlag =
                ((int)assetRef.transform.position.y % (int)flashModulus)
                == 0;

            if (assetRef.myData.verticalSpeed > 0)
            {
                ascendingIcon.SetActive(true && flashFlag);
                descendingIcon.SetActive(false);
            }
            else
            {
                ascendingIcon.SetActive(false);
                descendingIcon.SetActive(true && flashFlag);
            } 
        }
        else
        {
            ascendingIcon.SetActive(false);
            descendingIcon.SetActive(false);
        }

    }

    void TrackSubject()
    {
        if (assetRef == null)
            Destroy(gameObject);
        else
            transform.position = assetRef.transform.position + offset;
    }

    void HandleLandingIcon()
    {
        landingIcon.SetActive(assetRef.myData.isLanding);
    }

    void HandleFlightVector()
    {
        /*
        flightVector.SetPosition(1,
            (lengthFactor * assetRef.myData.forwardSpeed * assetRef.transform.forward)
            );

        flightVector.material.color = GameManager.Instance.heighColours[HeightIndex()];
        */

        if (partialVectors != null)
        {
            if (partialVectors.Length > 0)
            {
                for (int i = 0; i < partialVectors.Length; i++)
                {
                    float partialFactor = (i+1)/(partialVectors.Length*1.0f);
                    //Debug.Log("PF: " + partialFactor);
                    if (partialVectors[i] != null)
                    {
                        partialVectors[i].SetPosition(1,
                            (lengthFactor * assetRef.myData.forwardSpeed * assetRef.transform.forward * partialFactor)
                            );
                        partialVectors[i].material.color = GameManager.Instance.heighColours[HeightIndex()];
                    }
                }
            }
        }

        HandleSpeedLimitLine();
    }

    public void HandleSpeedLimitLine()
    {
        if (dzSpeedLimitIndicator != null)
        {

            //look for the closest danger zone
            speedLimit = 0;
            float closest = 1000;
            for(int i = 0; i < DangerZone.Instances.Count; i++)
            {
                if (DangerZone.Instances[i] != null)
                {
                    float dist = Vector3.Distance(assetRef.transform.position, DangerZone.Instances[i].transform.position);

                    if (dist < closest)
                    {
                        closest = dist;
                        closestDZ = DangerZone.Instances[i];
                        speedLimit = closestDZ.speedThreshold;
                        //closest one is your length
                    }
                }
            }

            //dzSpeedLimitIndicator.transform.position = assetRef.myData.forwardSpeed * assetRef.transform.forward * lengthFactor;            
            dzSpeedLimitIndicator.SetPosition(1, lengthFactor * speedLimit * assetRef.transform.forward);
        }
    }

    public int HeightIndex()
    {

        if (assetRef.transform.position.y < GameManager.Instance.heightThresholds[0])
            return 0;

        for (int i = 0; i < GameManager.Instance.heightThresholds.Length - 1; i++)
        {
            if (assetRef.transform.position.y >= GameManager.Instance.heightThresholds[i] &&
                assetRef.transform.position.y < GameManager.Instance.heightThresholds[i + 1])
            {
                return i + 1;
            }


        }

        return GameManager.Instance.heightThresholds.Length;
    }
}
