using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;

public class AiCart : MonoBehaviour
{
    public float obstacleDetection = 20;
    public float obstacleDetectSides = 3;

    private TrackTargets targets;
    private Transform[] trackPositions;
    private int trackPosNr = 0;
    
    public Transform previousDestination;
    public Transform currentDestination;

    private WheelCollider[] wheels;

    //Front wheels
    private WheelCollider wheel1; 
    private WheelCollider wheel2;

    public float speed = 60;

    private void Start()
    {
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
        ObstacleDetection();
        IsCartOnGround();
        NextPosition();
        
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

    private void NextPosition()
    {
        Vector3 me = gameObject.transform.position;
        Vector3 they = currentDestination.transform.position;
        if (Mathf.Abs(me.x - they.x) < 10f && Mathf.Abs(me.z - they.z) < 10f)
        {
            trackPosNr++;
            if (trackPosNr > trackPositions.Length)
                trackPosNr = 1;
            currentDestination = trackPositions[trackPosNr];

            //print(trackPositions[trackPosNr].name);
            

        }
    }

    void ApplyWheelSpeed(float wheelSpeed)
    {
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = wheelSpeed;
        }
    }

    public bool RayDistance(RaycastHit hit, float length)
    {
        float myX = Mathf.Abs(transform.position.x - hit.point.x);
        float myZ = Mathf.Abs(transform.position.z - hit.point.z);
        if (myX <= length && myZ <= length)
            return true;
        else return false;

    }

    private void ObstacleDetection()
    {
        Ray ray = new Ray(gameObject.transform.localPosition, (transform.forward * obstacleDetection)/Mathf.Infinity);
        RaycastHit hit;
        Debug.DrawRay(gameObject.transform.localPosition, transform.forward * obstacleDetection, Color.green);

        Ray ray1 = new Ray(gameObject.transform.localPosition, (new Vector3(0,0,obstacleDetectSides) + transform.forward * obstacleDetection) / Mathf.Infinity);
        RaycastHit hit1;
        Debug.DrawRay(gameObject.transform.localPosition, new Vector3(0, 0, obstacleDetectSides) + transform.forward * obstacleDetection, Color.green);

        Ray ray2 = new Ray(gameObject.transform.localPosition, (new Vector3(0, 0, -obstacleDetectSides) + transform.forward * obstacleDetection) / Mathf.Infinity);
        RaycastHit hit2;
        Debug.DrawRay(gameObject.transform.localPosition, new Vector3(0, 0, -obstacleDetectSides) + transform.forward * obstacleDetection, Color.green);
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag != "floor" && RayDistance(hit, obstacleDetection) == true)
        {
            ApplyWheelSpeed(-speed);
            print("i see "+ hit.collider.name);
            
        }

        else if (Physics.Raycast(ray1, out hit1) && hit1.collider.gameObject.tag != "floor" && RayDistance(hit1, obstacleDetection) == true)
        {
            ApplyWheelSpeed(-speed);
            Vector3 relativeVector = transform.InverseTransformPoint(hit1.transform.position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * -75f;
            wheel1.steerAngle = newSteer;
            wheel2.steerAngle = newSteer;
            //ApplyWheelSpeed(speed / 2);

            print("i see " + hit1.collider.name);
        }

        else if (Physics.Raycast(ray2, out hit2) && hit2.collider.gameObject.tag != "floor" && RayDistance(hit2, obstacleDetection) == true)
        {
            ApplyWheelSpeed(-speed);
            Vector3 relativeVector = transform.InverseTransformPoint(hit2.transform.position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * 75f;
            wheel1.steerAngle = newSteer;
            wheel2.steerAngle = newSteer;
            //ApplyWheelSpeed(speed / 2);

            print("i see " + hit2.collider.name);
        }
        else Movement();

        

    }
}
