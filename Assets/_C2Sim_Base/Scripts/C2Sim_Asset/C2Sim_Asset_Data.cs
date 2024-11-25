using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;

[System.Serializable]
public class C2Sim_Asset_Data
{
    [Header("Gameplay")]    
    public int value = 2;
    public int maxValue = 5;
    public float health = 100;
    public float maxHealth = 100;
    public float maxSpeed = 40;
    public float minSpeed = 10;
    public float maxHeight = 280;
    public float minHeight = 20;
    public float collisionSquare = 5;

    [Header("Navigation Settings")]
    public NavNode navTarget;    
    public float targetHeading;
    public float targetForwardSpeed = 1;
    public float targetHeight = 100;    
    public float arrivalDistance = 2;
    public bool autoLand = true;
    public Transform transformData;

    [Header("Navigation System")]
    public float forwardSpeed;
    public float headingChangeRate = 0.1f;
    public float forwardAcceleration = 0.3f;
    public float verticalSpeed;
    public float heightChangeRate = 0.1f;
    public bool isLanding = false;
    public Transform headingRef;
    public Vector3 position;
    public NavNode followNode;
    public NavNode prevNavTarget;
    public float prevNavHeading;
    public bool followable = false;
    public bool isExitingArea = false;

    [Header("Data Logging")]
    public int unitId;
    public string unitType;
    public string interventionString;
    public float lifeTime;
    public static int NextId;

    [Header("Calculations")]
    public LandingZone currentLZ;
    public DangerZone currentDgrZ;
    public DiversionZone currentDivZ;
    public float dangerClock;
    public float turnClock = 0;
    public float lastHeading;
    public float lastHeight;
    public float preLandingHeight;
    public Vector2 mapFrom;
    public Vector2 mapTo;
    public bool isInLandingZone = false;
    public bool diversionFlag = false;

    public static float ConvertAngleToHeading(float a)
    {

        while (a < 0)
        {
            a += 360;
        }

        return a;
    }

    //This returns the angle in radians
    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec1.x - vec2.x);
    }

    //This returns the angle in degrees
    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }

    public void InitiateLanding(LandingZone lz)
    {
        navTarget = lz.landingStart;
        isLanding = true;
        preLandingHeight = targetHeight;

        if (isInLandingZone)
            targetHeight = 5;

    }

    public void InitHeading()
    {
        lastHeading = transformData.rotation.eulerAngles.y;
        turnClock = 0;
    }

    public Vector3 HorizontalVelocity(Transform refTansform)
    {
        return refTansform.forward * forwardSpeed * Time.fixedDeltaTime;
    }

    public void HorizontalAccelerate()
    {
        if (targetForwardSpeed != forwardSpeed)
        {
            float deltaSpeed = 0;

            if (targetForwardSpeed > forwardSpeed)
                deltaSpeed = forwardAcceleration;
            else
                deltaSpeed = -forwardAcceleration;

            forwardSpeed = Mathf.Clamp(forwardSpeed + deltaSpeed * Time.fixedDeltaTime, minSpeed, maxSpeed);
        }
    }

    public Vector3 VerticalVelocity(Transform refTransform)
    {
        return refTransform.up * verticalSpeed * Time.fixedDeltaTime;
    }

    public void VerticalAccelerate(Transform refTransform)
    {
        if (refTransform.position.y != targetHeight)
        {
            //Debug.Log("------VerticalAccelerate");

            if (refTransform.position.y < targetHeight)
                verticalSpeed = Mathf.Min(heightChangeRate, targetHeight - refTransform.position.y);

            if (refTransform.position.y > targetHeight)
                verticalSpeed = Mathf.Max(-heightChangeRate, targetHeight - refTransform.position.y);

            
            if (Mathf.Abs(targetHeight - refTransform.position.y) <= heightChangeRate)
            {
                //verticalSpeed = 0;
                
                Vector3 desiredPosition = refTransform.position;
                desiredPosition.y = targetHeight;
                refTransform.position = desiredPosition;                
                verticalSpeed = 0;

                //Debug.Log("LOCKTIME " + desiredPosition.y);
            }           
            
        }
        else
        {
            //Debug.Log("Equil: " + refTransform.position.y);
            lastHeight = refTransform.position.y;
            verticalSpeed = 0;
        }
    }

    public C2Sim_Asset_Data CopySelf()
    {
        //can't do it this way, TRANSFORMS NOT SERAILZABLE!!
        //MAHVS_Asset_Data newCopy = Utilities.DeepClone<MAHVS_Asset_Data>(this);

        C2Sim_Asset_Data newCopy = new C2Sim_Asset_Data();

        newCopy.value = value;
        newCopy.health = health;
        newCopy.maxSpeed = maxSpeed;

        newCopy.transformData = transformData;
        newCopy.targetHeading = targetHeading;
        newCopy.targetForwardSpeed = targetForwardSpeed;
        newCopy.targetHeight = targetHeight;
        newCopy.navTarget = navTarget;
        newCopy.arrivalDistance = arrivalDistance;

        newCopy.forwardSpeed = forwardSpeed;
        newCopy.headingChangeRate = headingChangeRate;
        newCopy.forwardAcceleration = forwardAcceleration;
        newCopy.verticalSpeed = verticalSpeed;
        newCopy.heightChangeRate = heightChangeRate;
        newCopy.isLanding = isLanding;
        newCopy.headingRef = headingRef;
        newCopy.position = transformData.position;

        newCopy.unitId = unitId;
        newCopy.unitType = unitType;

        newCopy.currentLZ = currentLZ;
        newCopy.currentDgrZ = currentDgrZ;
        newCopy.dangerClock = dangerClock;
        newCopy.turnClock = turnClock;
        newCopy.lastHeading = lastHeading;
        newCopy.lastHeight = lastHeight;
        newCopy.preLandingHeight = preLandingHeight;
        newCopy.mapFrom = mapFrom;
        newCopy.mapTo = mapTo;
        newCopy.isInLandingZone = isInLandingZone;

        return newCopy;
    }
}
