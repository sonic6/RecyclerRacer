﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    GameObject[] carts;
    GameObject myCompatibleCart;

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
        Vector3 distance = new Vector3(0,0,0);
        Ray ray = new Ray(distance, rayMulti * (transform.position));
        RaycastHit hit;
        Debug.DrawRay(distance, rayMulti*(transform.position));
        if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == myCompatibleCart)
            myCompatibleCart.GetComponent<AiCart>().currentDestination = myCompatibleCart.GetComponent<AiCart>().previousDestination;
    }

    private void Update()
    {
        IsPickUpBehindCart();
    }
}
