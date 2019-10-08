using UnityEngine;
using UnityEngine.UI;

public class RankList : MonoBehaviour
{
    VerticalLayoutGroup uiRanks;
    int rank = 0;

    public void RegisterWin(LapInfo cart)
    {
        GameObject rankItem = Instantiate(new GameObject());
        rankItem.transform.parent = uiRanks.transform;
        rank++;
        rankItem.GetComponent<Text>().text = "Place nr" + rank + " " + cart.gameObject.name;
    }
}
