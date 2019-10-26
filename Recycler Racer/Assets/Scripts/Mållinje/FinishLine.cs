using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    public GameObject LapCounter;
    public int LapsDone;

    public GameObject checkpoint;
    public GameObject finishedLap;

    private void Start()
    {
        finishedLap.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        //Counting laps
        if (other.CompareTag("Cart1"))
        {
            LapsDone += 1;
            LapCounter.GetComponent<Text>().text = "" + LapsDone;

        }
        finishedLap.SetActive(false);
        checkpoint.SetActive(true);
    }
}
