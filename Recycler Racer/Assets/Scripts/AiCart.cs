using UnityEngine;
using System.Collections;
using System;

public class AiCart : MonoBehaviour
{
    public float obstacleDetection = 20;
    public float obstacleDetectSides = 3;

    private TrackTargets targets;
    public Transform[] trackPositions;
    private int trackPosNr = 1;
    
    public Transform previousDestination;
    public Transform currentDestination;

    private WheelCollider[] wheels;

    //Front wheels
    private WheelCollider wheel1; 
    private WheelCollider wheel2;

    public float speed = 60;
    public float minSpeed = 10;

    private void Start()
    {
        GameObject detector = Instantiate(new GameObject());
        detector.transform.position = transform.position;
        detector.transform.rotation = transform.rotation;
        detector.transform.parent = transform;
        detector.name = "Detector";
        detector.AddComponent<Detector>();

        targets = FindObjectOfType<TrackTargets>();
        wheels = GetComponentsInChildren<WheelCollider>();
        wheel1 = transform.Find("wheelF1").GetComponent<WheelCollider>();
        wheel2 = transform.Find("wheelF2").GetComponent<WheelCollider>();
        trackPositions = targets.GetComponentsInChildren<Transform>();
        currentDestination = trackPositions[1]; //trackPositions[0] is the parent holding the track objects. don't use it. 
    }

    

    

    // Update is called once per frame
    void Update()
    {
        FlipCart();
        ObstacleDetection();
        IsCartOnGround();
        NextPosition();
        
        //ForgetPickUpIfFar(currentDestination);
    }

    public void SetDestination(Transform destination)
    {
        previousDestination = currentDestination;
        currentDestination = destination;
    }

    void Movement()
    {
        ApplyWheelSpeed(speed);
        
        Vector3 relativeVector = transform.InverseTransformPoint(currentDestination.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 75f;
        wheel1.steerAngle = newSteer;
        wheel2.steerAngle = newSteer;

    }

    void IsCartOnGround()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "floor")
        {
            if (transform.position.y - hit.point.y < 5)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }

            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
            }
        }

    }

    void FlipCart()
    {
        if (transform.eulerAngles.x >= 90 || transform.eulerAngles.z >= 90)
        {
            StartCoroutine(CheckRotation());
        }
    }

    IEnumerator CheckRotation()
    {
        yield return new WaitForSeconds(5);
        if (transform.eulerAngles.x >= 90 || transform.eulerAngles.z >= 90)
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void NextPosition()
    {
        Vector3 me = gameObject.transform.position;
        Vector3 they = currentDestination.transform.position;
        //If current destination is close and isn't a pickup
        if (Mathf.Abs(me.x - they.x) < 20f && Mathf.Abs(me.z - they.z) < 20f && currentDestination.name.Contains("pickup") == false)
        {
            trackPosNr++;
            if (trackPosNr > trackPositions.Length - 1)
                trackPosNr = 1;
            currentDestination = trackPositions[trackPosNr];
        }
        //if current destination is a pickup
        else if(currentDestination.name.Contains("pickup"))
        {
            they = previousDestination.transform.position;
            if(Mathf.Abs(me.x - they.x) < 20f && Mathf.Abs(me.z - they.z) < 20f)
            {
                trackPosNr++;
                if (trackPosNr > trackPositions.Length - 1)
                    trackPosNr = 1;
                previousDestination = trackPositions[trackPosNr];
            }
        }
    }

    public void PrevDestinationOnPickUp()
    {
        if(previousDestination != null)
            currentDestination = previousDestination;
    }

    void ApplyWheelSpeed(float wheelSpeed)
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = wheelSpeed;
        }
    }
    

    private void ObstacleDetection()
    {
        Ray ray = new Ray(gameObject.transform.localPosition, (transform.forward / obstacleDetection));
        RaycastHit hit;
        Debug.DrawRay(gameObject.transform.localPosition, transform.forward * obstacleDetection, Color.green);

        Ray ray1 = new Ray(gameObject.transform.localPosition, (new Vector3(0,0,obstacleDetectSides) + transform.forward / obstacleDetection));
        RaycastHit hit1;
        Debug.DrawRay(gameObject.transform.localPosition, new Vector3(0, 0, obstacleDetectSides) + transform.forward * obstacleDetection, Color.green);

        Ray ray2 = new Ray(gameObject.transform.localPosition, (new Vector3(0, 0, -obstacleDetectSides) + transform.forward / obstacleDetection));
        RaycastHit hit2;
        Debug.DrawRay(gameObject.transform.localPosition, new Vector3(0, 0, -obstacleDetectSides) + transform.forward * obstacleDetection, Color.green);
        if (Physics.Raycast(ray, out hit) && ObstacleDistance(hit, obstacleDetection) == true && hit.collider.gameObject.tag != "floor")
        {

            if (hit.collider.gameObject.tag == "wall")
            {
                Vector3 relativeVector = transform.InverseTransformPoint(currentDestination.position);
                float newSteer = (relativeVector.x / relativeVector.magnitude) * -75f;
                wheel1.steerAngle = newSteer;
                wheel2.steerAngle = newSteer;
                ApplyWheelSpeed(-speed * 4);
            }
            else
            {
                ApplyWheelSpeed(speed / 2);
            }
        }

        else if (Physics.Raycast(ray1, out hit1) && ObstacleDistance(hit1, obstacleDetection) == true && hit1.collider.gameObject.tag != "floor")
        {
           
            Vector3 relativeVector = transform.InverseTransformPoint(hit1.transform.position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * -75f;
            wheel1.steerAngle = newSteer;
            wheel2.steerAngle = newSteer;
            ApplyWheelSpeed(speed / 2);
        }

        else if (Physics.Raycast(ray2, out hit2) && ObstacleDistance(hit2, obstacleDetection) == true && hit2.collider.gameObject.tag != "floor")
        {
            
            Vector3 relativeVector = transform.InverseTransformPoint(hit2.transform.position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * 75f;
            wheel1.steerAngle = newSteer;
            wheel2.steerAngle = newSteer;
            ApplyWheelSpeed(speed / 2);
        }
        else Movement();

        

    }

    private bool ObstacleDistance(RaycastHit hit, float distance)
    {
        float myX = Mathf.Abs(hit.point.x - transform.position.x);
        float myZ = Mathf.Abs(hit.point.z - transform.position.z);
        if (myX <= distance && myZ <= distance)
            return true;
        else return false;
    }
    

}

public class Detector : MonoBehaviour
{
    private void Start()
    {
        SetUpPickupDetector();
    }

    void SetUpPickupDetector()
    {
        BoxCollider bx = gameObject.AddComponent<BoxCollider>();
        bx.isTrigger = true;
        bx.center = new Vector3(0,0,60);
        bx.size = new Vector3(80, 20, 5);
    }

    private bool IsPickUpOnTrack(GameObject myPickup) //This function checks if the detected pickup is on track between current destination point and next destination point
    {
        Transform currentDest;
        Transform nxtDest;

        if (transform.parent.GetComponent<AiCart>().currentDestination.name.Contains("pickup") == false)
        {
            currentDest = transform.parent.GetComponent<AiCart>().currentDestination;
            string crtDstName = transform.parent.GetComponent<AiCart>().currentDestination.name;

            //These three lines are to seperate the name of the gameobject element (the ones in the race track) into string and number to later parse the number into an Int
            String[] spearator = { "GameObject (", ")" };
            Int32 count = 2;
            String[] strlist = crtDstName.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);
            
            if(int.Parse(strlist[0]) < transform.parent.GetComponent<AiCart>().trackPositions.Length - 1)
                nxtDest = transform.parent.GetComponent<AiCart>().trackPositions[int.Parse(strlist[0]) + 1];
            else
                nxtDest = transform.parent.GetComponent<AiCart>().trackPositions[1];

            float currentAndNxtX = Mathf.Abs(nxtDest.transform.position.x - currentDest.transform.position.x);
            float currentAndPickupX = Mathf.Abs(myPickup.transform.position.x - currentDest.transform.position.x);

            float currentAndNxtZ = Mathf.Abs(nxtDest.transform.position.z - currentDest.transform.position.z);
            float currentAndPickupZ = Mathf.Abs(myPickup.transform.position.z - currentDest.transform.position.z);
            if ((currentAndPickupX < currentAndNxtX) && (currentAndPickupZ < currentAndNxtZ))
                return true;
            else return false;
        }

        else
        {
            currentDest = transform.parent.GetComponent<AiCart>().previousDestination;
            string crtDstName = transform.parent.GetComponent<AiCart>().previousDestination.name;
            String[] spearator = { "GameObject (", ")" };
            Int32 count = 2;
            String[] strlist = crtDstName.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);

            nxtDest = transform.parent.GetComponent<AiCart>().trackPositions[int.Parse(strlist[0]) + 1];

            float currentAndNxtX = Mathf.Abs(nxtDest.transform.position.x - currentDest.transform.position.x);
            float currentAndPickupX = Mathf.Abs(myPickup.transform.position.x - currentDest.transform.position.x);

            float currentAndNxtZ = Mathf.Abs(nxtDest.transform.position.z - currentDest.transform.position.z);
            float currentAndPickupZ = Mathf.Abs(myPickup.transform.position.z - currentDest.transform.position.z);
            if ((currentAndPickupX < currentAndNxtX) && (currentAndPickupZ < currentAndNxtZ))
                return true;
            else return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.GetComponent<AiCart>().speed < 15 && transform.parent.name.Contains(other.tag))
        {
            if((other.gameObject.name != transform.parent.GetComponent<AiCart>().currentDestination.name) && IsPickUpOnTrack(other.gameObject) == true)
            {
                transform.parent.GetComponent<AiCart>().previousDestination = transform.parent.GetComponent<AiCart>().currentDestination;
                transform.parent.GetComponent<AiCart>().currentDestination = other.transform;
            }
            
        }
    }
}
