using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultScreenScene : MonoBehaviour
{
    PickUpCounter pc;

    [SerializeField] Text collected;
    [SerializeField] Text type;
    [SerializeField] Text result;
    [SerializeField] Text finalText;
    string cartType;

    private void Awake()
    {
        pc = FindObjectOfType<PickUpCounter>();
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "ResultsScene")
        {
            collected.text = PlayerPrefs.GetInt("collected").ToString();
            string playerCart;
            playerCart = PlayerPrefs.GetString("chosenCart");
            if (playerCart.Contains("plastic"))
            {
                cartType = "plastic";
                result.text = (PlayerPrefs.GetInt("collected") * 0.3) + " kWh";
            }
            else if (playerCart.Contains("glass"))
            {
                cartType = "glass";
                result.text = (PlayerPrefs.GetInt("collected") * 0.0832) + " kWh";
            }
            else if (playerCart.Contains("compost"))
            {
                cartType = "compost";
                result.text = (PlayerPrefs.GetInt("collected") * 0.0009) + " kWh";
            }
            else if (playerCart.Contains("metal"))
            {
                cartType = "metal";
                result.text = (PlayerPrefs.GetInt("collected") * 0.630) + " kWh";
            }

            type.text = cartType;

            string rad1 = "You picked up " + collected.text + " " + type.text;
            string rad2 = "Recycling each of these objects saves the following amount of energy:";
            string rad3 = "an apple equals 0.0009 kWh";
            string rad4 = "a metal tin can equals 0.630 kWh";
            string rad5 = "a plastic bottle equals 0.3 kWh";
            string rad6 = "a glass jar equals 0.0832 kWh";
            string rad7 = "For context, a standard TV uses 0.128 kWh per hour";
            string rad8 = "you saved " + result.text + " during the race";

            finalText.text = rad1 + "@" + rad2 + "@" + rad3 + "@" + rad4 + "@" + rad5 + "@" + rad6 + "@" + "@" + rad7 + "@" + rad8;
            finalText.text = finalText.text.Replace("@", System.Environment.NewLine); 
        }
    }

    public void PrepareResults()
    {
        PlayerPrefs.SetInt("collected", pc.pickedUp);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
