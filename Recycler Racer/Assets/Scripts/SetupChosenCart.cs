using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupChosenCart : MonoBehaviour
{
    AiCart[] carts;

    private void Awake()
    {
        string chosenCart = PlayerPrefs.GetString("chosenCart");
        carts = FindObjectsOfType<AiCart>();
        foreach(AiCart cart in carts)
        {
            if(cart.gameObject.name.Contains(chosenCart))
            {
                CartController player = cart.gameObject.AddComponent<CartController>();
                player.camPivot = GameObject.Find("CamPivot");
                player.speed = cart.speed;
                player.minSpeed = cart.minSpeed;
                Destroy(cart); //Destroys the component not the gameobject
            }
        }
    }
}
