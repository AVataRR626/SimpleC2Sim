using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C2Sim_Asset : MonoBehaviour
{
    public bool ghostMode = false;
    [Header("Data")]
    public C2Sim_Asset_Data myData;

    [Header("UI and Subsystems")]
    public HighlighterUtil highlighter;
    public GameObject explosionPrefab;
    public GameObject successFXPrefab;
    public Transform successFXSpawnPoint;
    public Rigidbody myRbody;//only for collision detection
    public GameObject graphicsRoot;
    public LineConnector destinationIndicator;
    public bool selectable = true;
    public bool maxConcurrentException = false;
    public string onClickHideFilter = "pretrial";
    public bool onClickDefaultTimescale = true;

    [Header("One Click Mode")]
    public bool oneClickMode = false;
    public NavNode oneClickNavPrefab;
    public Transform oneClickEvasionPoint;
    public int oneClickCost = 1;
    public bool continuePreviousCourse = true;
    public bool collisionReportFlag;
    public string collisionPair;
    public float defaultTimeScale = 1;

    public static int COUNT;

    public static float ConvertAngleToHeading(float a)
    {

        while (a < 0)
        {
            a += 360;
        }

        return a;
    }

    public void EnableOneClickMode()
    {
        oneClickMode = true;
    }

    public void ReportSafeOverfly()
    {
        Debug.Log("--------------C2SimAsset: Safe Overfly: " + name);
        ExperimentManager.Instance.ReportSafeOverfly();
    }

    public float HeadingToNavNode()
    {
        if (myData.navTarget != null)
        {
            myData.mapFrom = new Vector2(transform.position.x,
                transform.position.z);

            myData.mapTo = new Vector2(myData.navTarget.transform.position.x,
                myData.navTarget.transform.position.z);

            float rawAngle = AngleInDeg(myData.mapFrom, myData.mapTo) - 90;
            return (ConvertAngleToHeading(rawAngle));

        }
        else
            return -1111;//error!
    }

    public void HandleDiversionZone(DiversionZone dz)
    {
        if (!myData.diversionFlag)
        {
            if (dz.acceptFilter.Contains(this) || dz.acceptFilter.Count == 0)
            {

                NavNode divertNode = dz.NewDiversionPoint(myData.interventionString);
                //divertNode.interventionString = myData.interventionString;
                myData.prevNavTarget = myData.navTarget;


                if (myData.navTarget != null)
                    divertNode.nextNode = myData.navTarget;

                myData.navTarget = divertNode;
                myData.diversionFlag = true;
            }
        }
    }

    public void InitiateLanding(LandingZone lz)
    {
        if (!myData.isLanding)
        {   
            FirebaseBridge.Instance.AddEvent("ILND:" + myData.unitId + ";t:" + myData.unitType);            

            //cancel any present nav targets
            DestroyNavTarget();

            //and go land straight away!
            myData.navTarget = lz.landingStart;
            myData.isLanding = true;
            myData.preLandingHeight = myData.targetHeight;            
        }
    }

    public void AbortLanding()
    {
        myData.isLanding = false;
        myData.targetHeight = myData.preLandingHeight;
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

    // Start is called before the first frame update
    void Start()
    {
        InitHeading();
        InitAsset();

        COUNT++;

        Limit2MaxConcurrent();
    }

    public void Limit2MaxConcurrent()
    {
        
        if (!maxConcurrentException && TrialManager.Instance.maxConcurrent != 0)
        {
            if (COUNT > TrialManager.Instance.maxConcurrent)
            {
                Debug.Log("**** COUNT > MaxConcurrent! MaxConcurrent is: " + TrialManager.Instance.maxConcurrent);
                Destroy(gameObject);
            }
        }
    }

    private void InitAsset()
    {
        if (myRbody == null)
            myRbody = GetComponent<Rigidbody>();

        if (successFXSpawnPoint == null)
            successFXSpawnPoint = transform;

        myData.transformData = transform;
        myData.headingRef.parent = null;

        myData.unitId = C2Sim_Asset_Data.NextId;
        C2Sim_Asset_Data.NextId++;
        FirebaseBridge.Instance.AddEvent("SPN:" + myData.unitId + ";t:" + myData.unitType + ";p:" + transform.position);

        
    }

    private void Update()
    {
        if (highlighter != null && ManualNavManager.Instance != null)
        {
            highlighter.highlighting = (ManualNavManager.Instance.subject == this);
        }

        myData.lifeTime += Time.deltaTime;
    }

    public void SuccessAndExit()
    {
        //Debug.Log("---SUCCESS & EXIT-----");
        Instantiate(successFXPrefab, successFXSpawnPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }


    public void DestroyNavTarget()
    {
        if (myData.navTarget != null)
        {
            if (!myData.navTarget.permanent)
            {
                Destroy(myData.navTarget.gameObject);                
            }
        }

        myData.navTarget = null;
    }

    public void NextNavTarget()
    {
        //chained navigation...
        NavNode prevNode = myData.navTarget;
        myData.navTarget = myData.navTarget.nextNode;
        InitHeading();

        if (!prevNode.permanent)
            Destroy(prevNode.gameObject);
    }
    
    private void FixedUpdate()
    {
        EnforceLimits();
        FrameTick();
    }

    public void EnforceLimits()
    {
        myData.targetHeight = Mathf.Clamp(myData.targetHeight, myData.minHeight, myData.maxHeight);
        myData.forwardSpeed = Mathf.Clamp(myData.forwardSpeed, myData.minSpeed, myData.maxSpeed);
    }

    public void FrameTick()
    {
        HandleNavNodes();
        //transform.Translate(transform.forward * forwardSpeed, Space.World);
        HandlePosition();
        HandleHeadingQuaternions();
        
        
        EnforceHealth();

    }

    void EnforceHealth()
    {
        if (myData.health <= 0)
        {
            if (collisionReportFlag)
            {
                FirebaseBridge.Instance.AddEvent("COL: " + name + ";othr: " + collisionPair + ";pos: " + transform.position);
                Debug.Log(Time.time + ": " + name + " Collided with: " + collisionPair);
                ExperimentManager.Instance.ReportCollision();
            }

            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            GameManager.Instance.AddLoss(myData.value);
            DestroyNavTarget();
            Destroy(gameObject);
        }
    }

    void HandleNavNodes()
    {   
        myData.followNode.targetHeight = myData.targetHeight;

        if (myData.navTarget != null)
        {
            myData.targetHeading = HeadingToNavNode();

            //follow height instruction if specified
            if (myData.navTarget.targetHeight > -1)
                myData.targetHeight = myData.navTarget.targetHeight;

            //follow speed instruction if specified
            if (myData.navTarget.targetForwardSpeed > -1)
                myData.targetForwardSpeed = myData.navTarget.targetForwardSpeed;

            if (destinationIndicator != null)
            {

                myData.isExitingArea = myData.navTarget.areaExit;

                if (!myData.isExitingArea)
                {
                    destinationIndicator.destination = myData.navTarget.transform;
                }
                else
                {
                    destinationIndicator.destination = transform;
                }

            }

            //when you arrive at your navnode
            if (Vector2.Distance(myData.mapFrom, myData.mapTo) <= myData.arrivalDistance + (myData.forwardSpeed/2))
            {
                myData.diversionFlag = false;

                if (myData.navTarget.nextNode == null)
                {
                    if (myData.navTarget.killAssetOnArrival)
                    {
                        //landing on runway
                        if (myData.isLanding)
                        {
                            ExperimentManager.Instance.ReportLanding();
                            FirebaseBridge.Instance.AddEvent("LND: " + myData.unitId + " p:" + transform.position + " t:" + myData.unitType);
                            Instantiate(successFXPrefab, successFXSpawnPoint.position, Quaternion.identity);
                            GameManager.Instance.AddScore(myData.value);

                        }

                        //exiting airspace
                        if(myData.navTarget.scoreOnExit)
                        {

                            ExperimentManager.Instance.ReportLanding();
                            FirebaseBridge.Instance.AddEvent("SXT: " + myData.unitId + " p:" + transform.position + " t:" + myData.unitType);
                            Instantiate(successFXPrefab, successFXSpawnPoint.position, Quaternion.identity);
                            GameManager.Instance.AddScore(myData.value);
                        }
                        

                        Destroy(gameObject);
                    }

                    Debug.Log("C2Sim_Asset: " + name + " POO ");
                    DestroyNavTarget();
                }
                else
                {
                    NextNavTarget();
                }

            }
        }
        else
        {
            //revert to previous nav target if current nav target was destroyed
            //(most likely just finished a diversion and just needs to return to original destination)
            if (myData.prevNavTarget != null)
            {
                myData.navTarget = myData.prevNavTarget;
            }
            else
            {
                myData.targetHeading = myData.prevNavHeading;//revert to previous heading if there wasn't an original nav target
            }

            InitHeading();
            destinationIndicator.destination = transform;
        }

        //following aircraft must (try) to match speed
        if (myData.followNode != null)
        {
            myData.followNode.targetForwardSpeed = myData.targetForwardSpeed;
        }
    }

    void HandlePosition()
    {
        if (myData.isLanding && myData.isInLandingZone)
            myData.targetHeight = GameManager.Instance.landingHeight;

        Vector3 horizontal = myData.HorizontalVelocity(transform);//transform.forward * myData.forwardSpeed;
        Vector3 vertical = myData.VerticalVelocity(transform);

        //actually move the object...
        if (!ghostMode)
        {
            myRbody.MovePosition(transform.position +
                (horizontal)
                 + (vertical));
        }
        else
        {
            transform.position += horizontal + vertical;
            //myRbody.MovePosition(transform.position + (horizontal) + (vertical));
            
            if (ManualCollisionCheck())
            {
                Debug.Log("!!!!!!!!!!!!!! GHOST MODE COLLIDE!!!");
                GhostPrediction.Instance.collisionLog.Add("COLLISION PREDICTED: " + name);
            }
        }

        myData.HorizontalAccelerate();
        myData.VerticalAccelerate(transform);
        myData.position = transform.position;

    }

    public bool ManualCollisionCheck()
    {
        /*
         * Doesn't work when moving objects within 1 frame...
        //https://docs.unity3d.com/ScriptReference/Rigidbody.SweepTest.html
        RaycastHit hit;
        if (myRbody.SweepTest(transform.forward, out hit, 0.1f))
        {
            return true;
        }
        return false;
        */

        //Works between frames, but detects hits with itself... GRRRR
        //https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html        
        /*
        if (Physics.CheckSphere(transform.position, 5))
        {
            return true;
        }
        */

        //based on:
        //okay running this crashes the game--
        /*
        //https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, myData.collisionRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            //if you hit something that's not yourself...
            if(hitColliders[i].transform != transform)
            {
                return true;
            }
        }
        */

        return false;
    }

    void HandleHeadingQuaternions()
    {
        if (transform.rotation.eulerAngles.y != myData.targetHeading)
        {
            myData.headingRef.rotation = Quaternion.Euler(0, myData.targetHeading, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, myData.headingRef.rotation, myData.turnClock * myData.headingChangeRate);

            myData.turnClock += Time.fixedDeltaTime;
        }
        else
        {
            InitHeading();
        }
    }

    void HandleHeadingAngles()
    {
        if (transform.rotation.eulerAngles.y != myData.targetHeading)
        {
            Vector3 eulerRot = transform.rotation.eulerAngles;
            float lerpValue = (myData.headingChangeRate * myData.turnClock);

            eulerRot.y = Mathf.LerpAngle(myData.lastHeading, myData.targetHeading, lerpValue);

            //Debug.Log(" hd:" + eulerRot.y + " lh:" + lastHeading + " tc:" + turnClock + " lv:" + lerpValue + " tre:" + transform.rotation.eulerAngles);

            
            Quaternion newRot = new Quaternion();
            newRot.eulerAngles = eulerRot;
            transform.rotation = newRot;
            myData.turnClock += Time.fixedDeltaTime;
        }
        else
        {
            InitHeading();
        }
    }

    public void GhostMode()
    {
        ghostMode = true;
        gameObject.layer = GhostPrediction.ghostAssetLayer;
        graphicsRoot.layer = GhostPrediction.ghostAssetLayer;
        explosionPrefab = null;
        //GetComponent<Collider>().enabled = false;
    }

    public void InitHeading()
    {
        myData.lastHeading = transform.rotation.eulerAngles.y;
        myData.turnClock = 0;

        if (myData.targetHeading <= -1)
            myData.targetHeading = myData.lastHeading;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        C2Sim_Asset otherAsset = collision.gameObject.GetComponent<C2Sim_Asset>();

        if (otherAsset != null && otherAsset != this)
        {
            //loss and data logging handled in health enforcement function
            //collision is instant death
            myData.health = 0;
            collisionReportFlag = true;
            collisionPair = otherAsset.name;

            
        }

    }

    private void OnTriggerEnter(Collider other)
    {   
        //Debug.Log(Time.time + ": " + name + " entered trigger: " + other.name);

        C2Sim_InterventionStringSetter iss = other.GetComponent<C2Sim_InterventionStringSetter>();
        if (iss != null)
        {
            myData.interventionString = iss.interventionString;
        }


        //handle Danger Zones
        DangerZone newDZ = other.GetComponent<DangerZone>();
        if(newDZ != null)
        {
            if (newDZ != myData.currentDgrZ)
            {   
                myData.currentDgrZ = newDZ;

                FirebaseBridge.Instance.AddEvent("DZEN: " + myData.unitId + ";t:" + myData.unitType + ";dz: " + myData.currentDgrZ.dzName);

                if (myData.currentDgrZ.speedThreshold <= myData.forwardSpeed)
                    ExperimentManager.Instance.ReportDZSafeIncursion();
                else
                    ExperimentManager.Instance.ReportDZDangerousIncursion();
            }
        }

        //handle Landing Zones
        myData.currentLZ = other.GetComponentInParent<LandingZone>();
        if(myData.currentLZ != null)
        {
            myData.isInLandingZone = true;
            FirebaseBridge.Instance.AddEvent("LZEN: " + myData.unitId + ";lz:" + myData.currentLZ.name);

            if (myData.currentLZ.autoLand && myData.autoLand)
            {   
                //only land if runway isn't oversubscribed
                if (myData.currentLZ.congestionClock < myData.currentLZ.congestionMargin)
                {

                    InitiateLanding(myData.currentLZ);
                    //myData.currentLZ.congestionClock += myData.currentLZ.congestionMargin;
                }
                
            }
        }

        //handle Diversion Zones
        myData.currentDivZ = other.GetComponent<DiversionZone>();
        if(myData.currentDivZ != null)
        {
            FirebaseBridge.Instance.AddEvent("DVEN: "+ myData.unitId + ";t:" + myData.unitType + ";dz: " + myData.currentDivZ.name);
            HandleDiversionZone(myData.currentDivZ);
        }

        //handle Radar contacts
        C2Sim_Radar rd = other.GetComponent<C2Sim_Radar>();
        if(rd != null)
        {   
            rd.AddContact(this);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {

        C2Sim_InterventionStringSetter iss = other.GetComponent<C2Sim_InterventionStringSetter>();
        if (iss != null)
        {
            myData.interventionString = iss.interventionString;
        }

        if (myData.currentDgrZ != null)
        {
            myData.dangerClock += Time.fixedDeltaTime;

            if(myData.dangerClock >= 1)
            {
                if (myData.currentDgrZ.speedThreshold > myData.forwardSpeed)
                {
                    if (Random.Range(0, 100) <= myData.currentDgrZ.damageChance)
                    {
                        Debug.Log("DZONE: " + myData.currentDgrZ.speedThreshold + " | " + myData.forwardSpeed);

                        FirebaseBridge.Instance.AddEvent("DZDG: " + myData.unitId + ";t:" + myData.unitType + ";dz: " + myData.currentDgrZ.dzName);
                        myData.health -= myData.currentDgrZ.damage;

                    }

                    if(myData.health <= 0)
                    {
                        ExperimentManager.Instance.ReportDZCrash();
                    }
                }

                myData.dangerClock = 0;
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(Time.time + ": " + name + " exitted trigger: " + other.name);

        if (myData.currentDgrZ != null)
        {
            FirebaseBridge.Instance.AddEvent("DZEX: " + myData.unitId + ";t:" + myData.unitType + ";dz:" + myData.currentDgrZ.dzName);
            myData.currentDgrZ = null;
        }

        
        if(myData.currentLZ != null)
        {
            myData.isInLandingZone = false;
            FirebaseBridge.Instance.AddEvent("LZEX: " + myData.unitId + ";t:" + myData.unitType);
            myData.currentLZ = null;
        }

        C2Sim_Radar rd = other.GetComponent<C2Sim_Radar>();
        if (rd != null)
        {
            rd.RemoveContact(this);
        }
    }

    private void OnMouseDown()
    {   
        MouseDown();
    }

    public void MouseDown()
    {
        ExperimentManager.Instance.ReportAction();
        Debug.Log("C2Sim_Asset:OnMouseDown");

        if (onClickHideFilter.Length > 0)
            FilteredHideShow.Hide(onClickHideFilter);

        if(onClickDefaultTimescale)
            Time.timeScale = defaultTimeScale;

        if (selectable)
        {
            if (ManualNavManager.Instance.subject != null)
            {
                if (myData.followable)
                {
                    if (myData.followNode != null)
                        if (myData.followNode != ManualNavManager.Instance.subject.myData.followNode)
                            ManualNavManager.Instance.subject.myData.navTarget = myData.followNode;

                    ManualNavManager.Instance.subject = null;
                }
                else
                {
                    SelectSelf();
                }
            }
            else
            {
                SelectSelf();
            }
        }

        if (oneClickMode)
        {
            ExperimentManager.Instance.ReportIntervention(myData.interventionString, "Asset:OneClick", name, myData.unitType);

            if (oneClickNavPrefab != null)
            {
                NavNode oneClickNav = Instantiate(oneClickNavPrefab, oneClickEvasionPoint.position, Quaternion.identity);

                if (continuePreviousCourse)
                {

                    

                    oneClickNav.nextNode = myData.navTarget;
                    myData.prevNavHeading = myData.targetHeading;

                    //Debug.Log("----CONTINUE PREVIOUS COURSE----" + myData.targetHeading);
                }

                myData.navTarget = oneClickNav;

                GameManager.Instance.AddScore(-oneClickCost);
            }
        }
    }

    public void MakeSelectable()
    {
        selectable = true;
    }

    public void SelectSelf()
    {
        FirebaseBridge.Instance.AddEvent("SLCT: " + myData.unitId + " t:" + myData.unitType);
        ManualNavManager.Instance.subject = this;
    }

    private void OnDestroy()
    {
        COUNT--;

        if(myData.headingRef != null)
            Destroy(myData.headingRef.gameObject);
    }

    public void SelfDestruct()
    {
        if (myData.navTarget != null)
        {
            if(!myData.navTarget.permanent)
                Destroy(myData.navTarget.gameObject);
        }

        Destroy(gameObject);
    }
}
