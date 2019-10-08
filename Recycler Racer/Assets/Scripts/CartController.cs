﻿using UnityEngine;
using System;
using System.Collections;

public class CartController : MonoBehaviour
{
    WheelCollider[] myRig; //WheelCollider components on racing cart

    [Tooltip("The position of the camera relative to the player's cart")]
    [SerializeField] Vector3 camPos;
    [Tooltip("racing cart's speed")]
    public float speed;
    [Tooltip("racing cart's steering left and right speed")]
    [SerializeField] float steer;

    public GameObject camPivot;
    
    //Front wheels
    private WheelCollider wheel1;
    private WheelCollider wheel2;

    float distanceFromGround = 1;

    void Start()
    {
        myRig = GetComponentsInChildren<WheelCollider>();
        wheel1 = transform.Find("wheelF1").GetComponent<WheelCollider>();
        wheel2 = transform.Find("wheelF2").GetComponent<WheelCollider>();
    }
    
    void Update()
    {
        PositionCamera();
        IsCartOnGround();
        StartDriving();
    }

    void PositionCamera()
    {
        camPivot.transform.position = gameObject.transform.position/* + camPos*/;
        if (Camera.main.transform.parent == null)
            Camera.main.transform.parent = camPivot.transform;
        camPivot.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y,0);
        //Camera.main.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
    }

    void IsCartOnGround()
    {
        Ray ray = new Ray(transform.position, -transform.up * distanceFromGround);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "floor")
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

    IEnumerator StabalizeCart() //Might want to rase this. not being used
    {
        yield return new WaitForSeconds(5);
        transform.eulerAngles = new Vector3(0, transform.rotation.y, 0);
        GetComponent<Rigidbody>().AddForce(transform.up * 10);
    }

    

    private void StartDriving()
    {
        bool isDriving = false;
        var cartVelocity = gameObject.GetComponent<Rigidbody>().velocity;


        if (Input.GetAxis("Vertical") > 0)
        {
            //myRig.AddRelativeForce(new Vector3(0, 0, speed) * Time.deltaTime, ForceMode.Impulse);
            foreach (WheelCollider wheel in myRig)
            {
                wheel.motorTorque = speed;
            }

        }

        else if (Input.GetAxis("Vertical") < 0)
        {
            //myRig.AddRelativeForce(new Vector3(0, 0, -speed) * Time.deltaTime, ForceMode.Impulse);
            foreach (WheelCollider wheel in myRig)
            {
                wheel.motorTorque = -speed;
            }
        }

        else
        {
            foreach (WheelCollider wheel in myRig)
            {
                wheel.motorTorque = 0;
            }
        }

        //if cart's velocity on x or z is not 0 then the cart is marked as driving
        if (cartVelocity.x != 0 || cartVelocity.z != 0)
            isDriving = true;
        else isDriving = false;

        //If the cart is driving (velocity doesn't equal 0) then you can steer left and right
        if (isDriving == true)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + steer, transform.rotation.z), Space.Self);
                //Vector3 relativeVector = transform.InverseTransformPoint(currentDestination.position);
                float newSteer = /*(relativeVector.x / relativeVector.magnitude) **/ 75f;
                wheel1.steerAngle = newSteer;
                wheel2.steerAngle = newSteer;
            }

            else if (Input.GetAxis("Horizontal") < 0)
            {
                //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y - steer, transform.rotation.z), Space.Self);
                float newSteer = /*(relativeVector.x / relativeVector.magnitude) **/ 75f;
                wheel1.steerAngle = -newSteer;
                wheel2.steerAngle = -newSteer;
            }
            else
            {
                wheel1.steerAngle = 0;
                wheel2.steerAngle = 0;
            }
        }
    }
}
