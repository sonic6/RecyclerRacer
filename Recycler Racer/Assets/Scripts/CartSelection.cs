using UnityEngine;
using TMPro;

public class CartSelection : MonoBehaviour
{
    [SerializeField] GameObject[] Carts;
    private GameObject currentCart;
    public Transform previewPos;
    int currentIndex = 0;
    public Transform emptyPosition;
    public TextMeshProUGUI cartName;

    private void Start()
    {
        currentCart = Carts[currentIndex];
        PreviewCart();
    }

    public void RightButton()
    {
        HideCart();

        currentIndex++;
        if (currentIndex < Carts.Length)
            currentCart = Carts[currentIndex];
        else
        {
            currentIndex = 0;
            currentCart = Carts[currentIndex];
        }
        PreviewCart();
    }

    public void LeftButton()
    {
        HideCart();

        currentIndex--;
        if (currentIndex >= 0)
            currentCart = Carts[currentIndex];
        else
        {
            currentIndex = Carts.Length - 1;
            currentCart = Carts[currentIndex];
        }
        PreviewCart();
    }

    private void HideCart()
    {
        currentCart.transform.position = emptyPosition.position;
    }
    
    private void PreviewCart()
    {
        currentCart.transform.position = previewPos.position;
        cartName.text = currentCart.name.Remove(0, 8);
        if (cartName.text == "paper") cartName.text = "metal";
        PlayerPrefs.SetString("chosenCart", cartName.text);
    }
}
