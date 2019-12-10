using UnityEngine;
using UnityEngine.UI;

public class LapEnd : MonoBehaviour
{
    [SerializeField] Text playerLapUi;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<LapInfo>() && other.GetComponent<LapInfo>().LapsDone == 2)
        {
            
        }

        if(other.GetComponent<LapInfo>() && other.GetComponent<LapInfo>().lastLapTriggerPassed == 4)
        {
            other.GetComponent<LapInfo>().LapsDone++;
            other.GetComponent<LapInfo>().lastLapTriggerPassed = 0;
            if (other.GetComponent<CartController>())
                playerLapUi.text = other.GetComponent<LapInfo>().LapsDone.ToString();
        }
    }
}
