using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapInfo : MonoBehaviour
{
    GameObject goal;
    int lapNr = 0;
    RankList myList;
    Transform[] trackPos;

    // Start is called before the first frame update
    void Start()
    {
        trackPos = FindObjectOfType<TrackTargets>().GetComponentsInChildren<Transform>();
        myList = FindObjectOfType<RankList>();
        goal = FindObjectOfType<RankList>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == goal)
        {
            //lapNr++;
            //if (lapNr == 3)
            //{
            //    RankList.rank++;
            //    myList.RegisterWin(this);
            //}

            if (gameObject.layer == 8 && TrackTargets.nxtTarget == trackPos[trackPos.Length - 1])
                print("win");
        }
        
    }


}
