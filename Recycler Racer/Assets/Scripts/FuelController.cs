using UnityEngine;

public class FuelController : MonoBehaviour
{

    [SerializeField] GameObject needleTransform;
    [SerializeField] GameObject needle2;
    private float maxSpeedAngle = -72;
    private float zeroSpeedAngle = 72;

    private float speedMax;
    private float speed;

    private static float updatableValue = 0;

    private void Start()
    {
        needleTransform.transform.eulerAngles = new Vector3(0, 0, zeroSpeedAngle - 9);
    }

    public void SetNeedleAngle(float value)
    {
        updatableValue = ((maxSpeedAngle-zeroSpeedAngle) / value-1); //because the game starts of with 1 chunk of 3 in speed, not 0 chunk of 3;
        needleTransform.transform.eulerAngles = new Vector3(0, 0, needleTransform.transform.eulerAngles.z + updatableValue);
    }

    private void Update()
    {
        needle2.transform.eulerAngles = needleTransform.transform.eulerAngles * 2;
    }
    
}
