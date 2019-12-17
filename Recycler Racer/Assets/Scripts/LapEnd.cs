using UnityEngine;
using UnityEngine.UI;

public class LapEnd : MonoBehaviour
{
    [SerializeField] Text playerLapUi;
    [SerializeField] Text[] positions;
    int n = 0;
    [SerializeField] GameObject positionsParent;

    private void OnTriggerEnter(Collider other)
    {

        if(other.GetComponent<LapInfo>() && other.GetComponent<LapInfo>().lastLapTriggerPassed == 4)
        {
            other.GetComponent<LapInfo>().LapsDone++;
            other.GetComponent<LapInfo>().lastLapTriggerPassed = 0;
            if (other.GetComponent<CartController>())
                playerLapUi.text = other.GetComponent<LapInfo>().LapsDone.ToString();
        }

        if (other.GetComponent<LapInfo>() && other.GetComponent<LapInfo>().LapsDone == 3)
        {
            if (other.GetComponent<CartController>())
            {
                other.GetComponent<CartController>().enabled = false;
                if (n == 0)
                    positionsParent.SetActive(true);
            }

            if ( n==0 ) FirstPlace(other);
            else if (n == 1) SecondPlace(other);
            else if (n == 2) FinalPlace(other);
        }
    }

    private string ObtainName(Collider cart)
    {
        if (cart.gameObject.name.Contains("glass"))
            return "glass";
        else if (cart.gameObject.name.Contains("plastic"))
            return "plastic";
        else if (cart.gameObject.name.Contains("paper"))
            return "metal";
        else if (cart.gameObject.name.Contains("compost"))
            return "compost";
        else return null;
    }

    private void FirstPlace(Collider cart)
    {
        positions[n].text = "1st place: " + ObtainName(cart) + " recycler cart";
        n++;
    }
    private void SecondPlace(Collider cart)
    {
        positions[n].text = "2nd place: " + ObtainName(cart) + " recycler cart";
        n++;
    }
    private void FinalPlace(Collider cart)
    {
        positions[n].text = "3rd place: " + ObtainName(cart) + " recycler cart";
        n++;
        FindLastCart();
        positionsParent.SetActive(true);
    }

    private void FindLastCart()
    {
        GameObject[] cart = GameObject.FindGameObjectsWithTag("cart");
        GameObject LastCart = null;

        for(int x = 0; x < cart.Length; x++)
        {
            if (cart[x].GetComponent<LapInfo>().LapsDone < 3)
                LastCart = cart[x]; 
        }

        positions[n].text = "4th place: " + ObtainName(LastCart.GetComponent<Collider>()) + " recycler cart";

        if (LastCart.GetComponent<CartController>())
            LastCart.GetComponent<CartController>().enabled = false;
    }
}
