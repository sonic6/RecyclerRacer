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
        currentDestination = trackPositions[0];
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleDetection();
        //myCart.SetDestination(currentDestination.position);
        //Movement();
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

        //Vector3 direction = currentDestination.position - transform.position;
        //Quaternion rotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.5f * Time.deltaTime);

        Vector3 relativeVector = transform.InverseTransformPoint(currentDestination.position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * 75f;
        wheel1.steerAngle = newSteer;
        wheel2.steerAngle = newSteer;

    }

    private void NextPosition()
    {
        Vector3 me = gameObject.transform.position;
        Vector3 they = currentDestination.transform.position;
        if (Mathf.Abs(me.x - they.x) < 10f && Mathf.Abs(me.z - they.z) < 10f)
        {
            if (trackPosNr + 1 == trackPositions.Length)
                trackPosNr = 0;

            currentDestination = trackPositions[trackPosNr + 1];

            if (trackPosNr + 1 < trackPositions.Length)
                trackPosNr++;
        }
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
        Ray ray = new Ray(gameObject.transform.localPosition, transform.forward * obstacleDetection);
        RaycastHit hit;
        Debug.DrawRay(gameObject.transform.localPosition, transform.forward * obstacleDetection, Color.green);

        Ray ray1 = new Ray(gameObject.transform.localPosition, new Vector3(0,0,obstacleDetectSides) + transform.forward * obstacleDetection);
        RaycastHit hit1;
        Debug.DrawRay(gameObject.transform.localPosition, new Vector3(0, 0, obstacleDetectSides) + transform.forward * obstacleDetection, Color.green);

        Ray ray2 = new Ray(gameObject.transform.localPosition, new Vector3(0, 0, -obstacleDetectSides) + transform.forward * obstacleDetection);
        RaycastHit hit2;
        Debug.DrawRay(gameObject.transform.localPosition, new Vector3(0, 0, -obstacleDetectSides) + transform.forward * obstacleDetection, Color.green);
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag != "floor")
        {
            ApplyWheelSpeed(-speed);

            if (Physics.Raycast(ray1, out hit1) && hit1.collider.gameObject.tag != "floor")
            {
                Vector3 relativeVector = transform.InverseTransformPoint(hit1.transform.position);
                float newSteer = (relativeVector.x / relativeVector.magnitude) * -75f;
                wheel1.steerAngle = newSteer;
                wheel2.steerAngle = newSteer;
                //ApplyWheelSpeed(speed / 2);
            }

            else if (Physics.Raycast(ray2, out hit2) && hit2.collider.gameObject.tag != "floor")
            {
                Vector3 relativeVector = transform.InverseTransformPoint(hit2.transform.position);
                float newSteer = (relativeVector.x / relativeVector.magnitude) * 75f;
                wheel1.steerAngle = newSteer;
                wheel2.steerAngle = newSteer;
                //ApplyWheelSpeed(speed / 2);
            }
        }
        else Movement();

        

    }
}
