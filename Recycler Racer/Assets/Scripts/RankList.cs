using UnityEngine;
using UnityEngine.UI;

public class RankList : MonoBehaviour
{
    [SerializeField] VerticalLayoutGroup uiRanks;
    [SerializeField] GameObject item;
    public static GameObject goalLine;

    public static int rank = 0;

    private void Awake()
    {
        goalLine = GameObject.Find("mål");
        GameObject[] carts = GameObject.FindGameObjectsWithTag("cart");

        foreach(GameObject cart in carts)
        {
            cart.AddComponent<LapInfo>();
        }
    }

    public void RegisterWin(LapInfo cart)
    {
        GameObject rankItem = Instantiate(item);
        rankItem.transform.parent = uiRanks.transform;
        rankItem.GetComponent<Text>().text = "Place nr" + rank + " " + cart.gameObject.name;
    }
}
