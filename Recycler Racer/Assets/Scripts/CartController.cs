using UnityEngine;

public class CartController : MonoBehaviour
{
    Rigidbody myRig; //Component on racing carts

    [Tooltip("racing cart's speed")]
    [SerializeField] float speed;

    [SerializeField] float steer;

    void Start()
    {
        myRig = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (Input.GetAxis("Vertical") > 0)
            myRig.AddRelativeForce(new Vector3(0, 0, speed) * Time.deltaTime, ForceMode.Impulse);
        
        else if(Input.GetAxis("Vertical") < 0)
            myRig.AddRelativeForce(new Vector3(0, 0, -speed) * Time.deltaTime, ForceMode.Impulse);

        if (Input.GetAxis("Horizontal") > 0)
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y + steer, transform.rotation.z), Space.Self);

        else if (Input.GetAxis("Horizontal") < 0)
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y - steer, transform.rotation.z), Space.Self);
    }
}
