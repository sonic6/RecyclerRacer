using UnityEngine;
using System.Collections;

public class CartController : MonoBehaviour
{


    WheelCollider[] myRig; //WheelCollider components on racing cart

    //[Tooltip("The position of the camera relative to the player's cart")]
    //[SerializeField] Vector3 camPos;
    [Tooltip("racing cart's speed")]
    public float speed;
    [Tooltip("racing cart's minimum speed")]
    public float minSpeed;
    //[Tooltip("racing cart's steering left and right speed")]
    //[SerializeField] float steer;

    public GameObject camPivot;
    
    //Front wheels
    private WheelCollider wheel1;
    private WheelCollider wheel2;

    float distanceFromGround = 1;

    Rigidbody rb;
    private void Awake()
    {
        Time.timeScale = 2.5f;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass.Set(rb.centerOfMass.x, rb.centerOfMass.y - 0.5f, rb.centerOfMass.z); //Lowering the center of mass is supposed to make the cart more stable
        myRig = GetComponentsInChildren<WheelCollider>();
        wheel1 = transform.Find("wheelF1").GetComponent<WheelCollider>();
        wheel2 = transform.Find("wheelF2").GetComponent<WheelCollider>();
    }
    
    void Update()
    {
        PositionCamera();
        IsCartOnGround();
        StartDriving();
        FlipCart();
    }

    void PositionCamera()
    {
        camPivot.transform.position = gameObject.transform.position/* + camPos*/;
        if (Camera.main.transform.parent == null)
            Camera.main.transform.parent = camPivot.transform;
        camPivot.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y,0);
    }

    void IsCartOnGround()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1) , -transform.up * distanceFromGround);
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

    

    private void FlipCart()
    {
        if (Input.GetKey(KeyCode.F))
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

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

    private void StartDriving()
    {
        bool isDriving = false;
        var cartVelocity = gameObject.GetComponent<Rigidbody>().velocity;


        if (Input.GetAxis("Vertical") > 0)
        {
            foreach (WheelCollider wheel in myRig)
            {
                wheel.motorTorque = speed;
            }

        }

        else if (Input.GetAxis("Vertical") < 0)
        {
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
            rb.AddRelativeForce(-transform.up * Time.deltaTime * (cartVelocity.magnitude/10)); //Adds downwards force on the car to pin it to th ground. 

            if (Input.GetAxis("Horizontal") > 0)
            {
                float newSteer = Mathf.LerpAngle(0, 75f, 1f /cartVelocity.magnitude ) * Input.GetAxis("Horizontal") ;
                wheel1.steerAngle = newSteer;
                wheel2.steerAngle = newSteer;
            }

            else if (Input.GetAxis("Horizontal") < 0)
            {
                float newSteer = Mathf.LerpAngle(0, 75f,1f / cartVelocity.magnitude)* Input.GetAxis("Horizontal") ;
                wheel1.steerAngle = newSteer;
                wheel2.steerAngle = newSteer;
            }

            else if (Input.GetAxis("Horizontal") == 0)
            {
                wheel1.steerAngle = 0;
                wheel2.steerAngle = 0;
            }

            //This is for drifting
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, Mathf.LerpAngle(0, 90f,Time.deltaTime * Input.GetAxis("Horizontal")), rb.transform.localEulerAngles.z);
            //    //rb.MoveRotation(new Quaternion(rb.transform.localEulerAngles.x, Mathf.LerpAngle(0, 90f, Input.GetAxis("Horizontal") / 360), rb.transform.localEulerAngles.z, rb.transform.localRotation.w));
            //}
        }
    }

    public IEnumerator DrainSpeed()
    {
        yield return new WaitForSeconds(10f);
        if(speed > minSpeed)
            speed = speed - 3;
    }
}
