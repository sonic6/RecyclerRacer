using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpoint;
    public GameObject FinishedLap;

    private void OnTriggerEnter(Collider other)
    {
        FinishedLap.SetActive(true);
        checkpoint.SetActive(false);
    }
}
