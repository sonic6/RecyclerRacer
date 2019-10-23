using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelController : MonoBehaviour
{

    private GameObject needleTransform;
    private const float maxSpeedAngle = -72;
    private const float zeroSpeedAngle = 74;

    private float speedMax;
    private float speed;


    private void Awake()
    {
        needleTransform = GameObject.FindGameObjectWithTag("Needle");

        speed = 100f;
        speedMax = 100f;
    }

    private float getSpeedRotation()
    {
        float totalAngleSize = zeroSpeedAngle - maxSpeedAngle;

        float speedNormalized = speed / speedMax;

        return zeroSpeedAngle - speedNormalized * totalAngleSize;
    }

    private void Update()
    {
        speed -= 20f * Time.deltaTime;

        speed = Mathf.Clamp(speed, 0, speedMax);
        //if (speed > speedMax) speed = speedMax;

        needleTransform.transform.eulerAngles = new Vector3(0, 0, getSpeedRotation());
    }
}
