using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;

public class AiCart : MonoBehaviour
{
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
        //myCart.SetDestination(currentDestination.position);
        Movement();
        NextPosition();
    }

    public void SetDestination(Transform destination)
    {
        previousDestination = currentDestination;
        currentDestination = destination;
    }

    void Movement()
    {
        foreach(WheelCollider wheel in wheels)
        {
            wheel.motorTorque = speed;
        }

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
}
