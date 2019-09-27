using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    GameObject[] carts;
    GameObject myCompatibleCart;
    GameObject pivot;

    private void Start()
    {
        carts = GameObject.FindGameObjectsWithTag("cart");
        ExamineCarts();
    }

    void ExamineCarts() //Finds a cart that is compatible with this pickup and gives it this pickup as a destination
    {
        foreach(GameObject cart in carts)
        {
            if (cart.gameObject.name.Contains(gameObject.tag)) //if the cart is compatible with this pickup
            {
                myCompatibleCart = cart;
                cart.GetComponent<AiCart>().GetDestination(gameObject.transform); //Then make cart set it's destination to this pickup
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("cart"))
        {
            
            Destroy(gameObject);
        }
    }

    //if this pickup is taken by any cart or destroyed for any other reason. tell the compatible cart to set it's destination back on track
    private void OnDestroy()
    {
        myCompatibleCart.GetComponent<AiCart>().currentDestination = myCompatibleCart.GetComponent<AiCart>().previousDestination;
    }

    private void IsPickUpBehindCart()
    {
        
        if(pivot == null)
        {
            pivot = new GameObject();
            pivot.transform.position = gameObject.transform.position;
            pivot.transform.parent = gameObject.transform;
        }

        Vector3 distance = pivot.transform.localPosition;
        Ray ray = new Ray(new Vector3(distance.x - 100, distance.y, distance.z), new Vector3(distance.x + 100, distance.y, 0));
        RaycastHit hit;
        Debug.DrawRay(new Vector3(distance.x - 100, distance.y, distance.z), new Vector3(distance.x + 100, distance.y, 0), Color.green);
        if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == myCompatibleCart)
            myCompatibleCart.GetComponent<AiCart>().currentDestination = myCompatibleCart.GetComponent<AiCart>().previousDestination;
    }

    private void Update()
    {
        IsPickUpBehindCart();
    }
}
