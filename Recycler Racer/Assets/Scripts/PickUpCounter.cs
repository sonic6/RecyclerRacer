using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpCounter : MonoBehaviour
{
    int pickedUp = 0;

    public static string pickUpType;
    [SerializeField] Image uiPickup = null;
    Text PickupAmount;
    [SerializeField] Sprite plastic;
    [SerializeField] Sprite glass;
    [SerializeField] Sprite compost;
    [SerializeField] Sprite paper;

    private void Start()
    {
        if (pickUpType.Contains("plastic"))
            uiPickup.sprite = plastic;
        else if (pickUpType.Contains("glass"))
            uiPickup.sprite = glass;
        else if (pickUpType.Contains("compost"))
            uiPickup.sprite = compost;
        else if (pickUpType.Contains("paper"))
            uiPickup.sprite = paper;

        PickupAmount = uiPickup.GetComponentInChildren<Text>();
    }

    public void UpdateUi()
    {
        pickedUp++;
        PickupAmount.text = pickedUp.ToString();
    }
    
    
}
