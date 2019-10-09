using UnityEngine;
using UnityEngine.AI;

public class PickUps : MonoBehaviour
{
    GameObject[] carts;
    GameObject myCompatibleCart;
    float aiBoost = 10; //speed boost to be given to ai cart
    float playerBoost = 10; //speed boost to be given to player cart

    //The amount the ray length will be multiplied by when casting it. This will help make a longer ray that goes through the pickup object
    public static float rayMulti = 2;

    private void Start()
    {
        gameObject.layer = 2; // layer nr 2 for Ignore raycast
        carts = GameObject.FindGameObjectsWithTag("cart");
        ExamineCarts();
    }

    void ExamineCarts() //Finds a cart that is compatible with this pickup and gives it this pickup as a destination
    {
        foreach(GameObject cart in carts)
        {
            var cartX = cart.transform.position.x;
            var cartZ = cart.transform.position.z;

            if (cart.gameObject.name.Contains(gameObject.tag)/* && Mathf.Abs(cartX - transform.position.x) < 10 && Mathf.Abs(cartZ - transform.position.z) < 10*/) //if the cart is compatible with this pickup
            {
                myCompatibleCart = cart;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("cart"))
        {
            myCompatibleCart.GetComponent<AiCart>().PrevDestinationOnPickUp();
            Destroy(gameObject);
        }
        if (other.gameObject == myCompatibleCart)
        {
            if (other.GetComponent<AiCart>())
                other.GetComponent<AiCart>().speed += aiBoost;
            else if (other.GetComponent<CartController>())
                other.GetComponent<CartController>().speed += playerBoost;
        }
    }

    private void IsPickUpBehindCart() //casts a ray from the center point of world coordinates to this pickup item and through it. If cart hits the ray while driving, then it moves on and doesn't try to take the pickup
    {
        Vector3 distance = new Vector3(0,0,0);
        Ray ray = new Ray(distance, rayMulti * (transform.position));
        RaycastHit hit;
        Debug.DrawRay(distance, rayMulti * (transform.position));
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == myCompatibleCart)
            myCompatibleCart.GetComponent<AiCart>().currentDestination = myCompatibleCart.GetComponent<AiCart>().previousDestination;
    }

    private void Update()
    {
        IsPickUpBehindCart();
    }
}
