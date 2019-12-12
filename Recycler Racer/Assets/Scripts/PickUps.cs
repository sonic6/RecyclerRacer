using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour
{
    PickUpCounter Ui;

    GameObject[] carts;
    GameObject myCompatibleCart;
    float aiBoost = 3; //speed boost to be given to ai cart
    float playerBoost = 3; //speed boost to be given to player cart
    
    [Tooltip("The amount the ray length will be multiplied by when casting it. This will help make a longer ray that goes through the pickup object")]
    public static float rayMulti = 2;

    [Tooltip("How long in seconds this pickUp will stay on the game field before dissapearing")]
    [SerializeField] float lifeSpan = 40;

    private void Awake()
    {
        Ui = FindObjectOfType<PickUpCounter>();
    }

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

            if (cart.gameObject.name.Contains(gameObject.tag)) //if the cart is compatible with this pickup
            {
                myCompatibleCart = cart;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("cart"))
        {
            if(myCompatibleCart.GetComponent<AiCart>() && myCompatibleCart.GetComponent<AiCart>().currentDestination == gameObject.transform && myCompatibleCart.GetComponent<AiCart>().previousDestination != null)
                myCompatibleCart.GetComponent<AiCart>().PrevDestinationOnPickUp();
            

            if (other.gameObject == myCompatibleCart) //If the correct cart takes the pickup increase the cart's speed by playerBoost or aiBoost accordingly
            {
                if (other.GetComponent<AiCart>() && other.GetComponent<AiCart>().speed < 15)
                    other.GetComponent<AiCart>().speed += aiBoost;
                else if (other.GetComponent<CartController>() && other.GetComponent<CartController>().speed < 15)
                    other.GetComponent<CartController>().speed += playerBoost;

                if(other.GetComponent<CartController>()) //This communicates with PickUpCounter script to tell it what type of pickup was picked up by player
                    Ui.UpdateUi();
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
    

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeSpan);
        if(myCompatibleCart.GetComponent<AiCart>() && myCompatibleCart.GetComponent<AiCart>().previousDestination != null && myCompatibleCart.GetComponent<AiCart>().currentDestination == gameObject.transform)
            myCompatibleCart.GetComponent<AiCart>().PrevDestinationOnPickUp();
        Destroy(gameObject);
    }
}
