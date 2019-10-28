using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSelection : MonoBehaviour
{
    [SerializeField] GameObject[] Carts;
    private GameObject currentCart;
    private Transform previewPos;
    int currentIndex = 0;

    private void Start()
    {
        currentCart = Carts[currentIndex];
        previewPos = currentCart.transform;
    }

    public void RightButton()
    {
        currentIndex++;
        if (currentIndex < Carts.Length)
            currentCart = Carts[currentIndex];
        else
        {
            currentIndex = 0;
            currentCart = Carts[currentIndex];
        }
    }

    public void LeftButton()
    {
        currentIndex--;
        if (currentIndex >= 0)
            currentCart = Carts[currentIndex];
        else
        {
            currentIndex = Carts.Length - 1;
            currentCart = Carts[currentIndex];
        }
    }

    //This function is unfinished. Will be continued
    private void PreviewCart()
    {
        currentCart.transform.position = previewPos.position;
    }
}
