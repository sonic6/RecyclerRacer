using UnityEngine;

public class CartController : MonoBehaviour
{
    Rigidbody myRig; //Component on racing carts

    [Tooltip("racing cart's speed")]
    [SerializeField] float speed;
    [Tooltip("racing cart's steering left and right speed")]
    [SerializeField] float steer;



    bool onGround;

    void Start()
    {
        myRig = GetComponent<Rigidbody>();

    }
    
    void Update()
    {
        StartDriving();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "floor")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "floor")
        {
            onGround = false;
        }
    }

    private void StartDriving()
    {
        bool isDriving = false;
        var cartVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        //if the cart is on the ground then it's allowed to drive forward or backwards
        if (onGround)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                myRig.AddRelativeForce(new Vector3(0, 0, speed) * Time.deltaTime, ForceMode.Impulse);

                
            }

            else if (Input.GetAxis("Vertical") < 0)
            {
                myRig.AddRelativeForce(new Vector3(0, 0, -speed) * Time.deltaTime, ForceMode.Impulse);


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
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + steer, transform.rotation.z), Space.Self);

            else if (Input.GetAxis("Horizontal") < 0)
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y - steer, transform.rotation.z), Space.Self);
        }
        
    }

}
