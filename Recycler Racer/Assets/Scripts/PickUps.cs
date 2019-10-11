using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour
{
    GameObject[] carts;
    GameObject myCompatibleCart;
    float aiBoost = 10; //speed boost to be given to ai cart
    float playerBoost = 10; //speed boost to be given to player cart
    
    [Tooltip("The amount the ray length will be multiplied by when casting it. This will help make a longer ray that goes through the pickup object")]
    public static float rayMulti = 2;

    [Tooltip("How long in seconds this pickUp will stay on the game field before dissapearing")]
    [SerializeField] float lifeSpan = 15;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
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
            if(myCompatibleCart.GetComponent<AiCart>() && myCompatibleCart.GetComponent<AiCart>().previousDestination != null)
                myCompatibleCart.GetComponent<AiCart>().PrevDestinationOnPickUp();
            

            if (other.gameObject == myCompatibleCart) //If the correct cart takes the pickup increase the cart's speed by playerBoost or aiBoost accordingly
            {
                if (other.GetComponent<AiCart>())
                    other.GetComponent<AiCart>().speed += aiBoost;
                else if (other.GetComponent<CartController>())
                    other.GetComponent<CartController>().speed += playerBoost;
            }
            else if (other.gameObject != myCompatibleCart) //If the wrong cart takes the pickup decrease the cart's speed by playerBoost or aiBoost accordingly
            {
                if (other.GetComponent<AiCart>() && other.GetComponent<AiCart>().speed > other.GetComponent<AiCart>().minSpeed)
                    other.GetComponent<AiCart>().speed -= aiBoost;
                else if (other.GetComponent<CartController>() && other.GetComponent<CartController>().speed > other.GetComponent<CartController>().minSpeed)
                    other.GetComponent<CartController>().speed -= playerBoost;
            }
            else
                return;

            Destroy(gameObject);
        }
        
    }

    private void IsPickUpBehindCart() //casts a ray from the center point of world coordinates to this pickup item and through it. If cart hits the ray while driving, then it moves on and doesn't try to take the pickup
    {
        Vector3 distance = new Vector3(0, 0, 0);
        Ray ray = new Ray(distance, rayMulti * (transform.position));
        RaycastHit hit;
        Debug.DrawRay(distance, rayMulti * (transform.position));
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == myCompatibleCart)
        {
            if (myCompatibleCart.GetComponent<AiCart>() && myCompatibleCart.GetComponent<AiCart>().currentDestination == gameObject && myCompatibleCart.GetComponent<AiCart>().previousDestination != null)
                myCompatibleCart.GetComponent<AiCart>().PrevDestinationOnPickUp();
        }
            
    }

    private void Update()
    {
        IsPickUpBehindCart();
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeSpan);
        if(myCompatibleCart.GetComponent<AiCart>() && myCompatibleCart.GetComponent<AiCart>().previousDestination != null)
            myCompatibleCart.GetComponent<AiCart>().PrevDestinationOnPickUp();
        Destroy(gameObject);
    }
}
