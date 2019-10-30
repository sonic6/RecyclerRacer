using UnityEngine;
using UnityEngine.UI;

public class RankList : MonoBehaviour
{
    [SerializeField] VerticalLayoutGroup uiRanks;
    [SerializeField] GameObject item;
    public GameObject goalLine1;
    public GameObject goalLine2;

    public static int rank = 0;

    private void Awake()
    {
        GoalTrigger g1 = goalLine1.AddComponent<GoalTrigger>();
        g1.index = 1;
        GoalTrigger g2 = goalLine2.AddComponent<GoalTrigger>();
        g2.index = 2;

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

public class GoalTrigger : MonoBehaviour
{
    public int index; //indicates the number of the gameobject. First goaltrigger or second goaltrigger
    bool myBool = false;
    RankList myRanklist;
    bool trick = false; //To make sure the player doesn't cheat

    private void Start()
    {
        myRanklist = FindObjectOfType<RankList>();
    }

    private void OnTriggerEnter(Collider other) //probably very broken and needs many fixes
    {
        if (index == 1)
            myBool = true;
        else if(index == 2)
        {
            bool g1 = myRanklist.goalLine1.GetComponent<GoalTrigger>().myBool;
            if (myRanklist.goalLine1.GetComponent<GoalTrigger>().myBool == true)
            {
                print("Lap");
                g1 = false;
                myBool = true;
            }
            else if (myBool == true)
                trick = true;
            if (trick == true)
                myBool = false;
        }
    }
}
