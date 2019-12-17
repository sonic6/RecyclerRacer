using UnityEngine;
using UnityEngine.UI;

public class PickUpCounter : MonoBehaviour
{
    public int pickedUp = 0;

    public string pickUpType;
    [SerializeField] Image uiPickup = null;
    [SerializeField] Text PickupAmount;
    [SerializeField] Sprite plastic;
    [SerializeField] Sprite glass;
    [SerializeField] Sprite compost;
    [SerializeField] Sprite paper;
    [SerializeField] Text description;

    private void Start()
    {
        if (pickUpType.Contains("plastic"))
        {
            uiPickup.sprite = plastic;
            description.text = "plastic";
        }
        else if (pickUpType.Contains("glass"))
        {
            uiPickup.sprite = glass;
            description.text = "glass";
        }
        else if (pickUpType.Contains("compost"))
        {
            uiPickup.sprite = compost;
            description.text = "compost";
        }
        else if (pickUpType.Contains("paper"))
        {
            uiPickup.sprite = paper;
            description.text = "metal";
        }
    }

    public void UpdateUi()
    {
        pickedUp++;
        PickupAmount.text = pickedUp.ToString();
    }
    
    
}
