using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapInfo : MonoBehaviour
{
    GameObject goal;
    int lapNr = 0;
    RankList myList;

    // Start is called before the first frame update
    void Start()
    {
        myList = FindObjectOfType<RankList>();
        goal = RankList.goalLine;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == goal)
        {
            lapNr++;
            if (lapNr == 3)
            {
                RankList.rank++;
                myList.RegisterWin(this);
            }
        }
        
    }
}
