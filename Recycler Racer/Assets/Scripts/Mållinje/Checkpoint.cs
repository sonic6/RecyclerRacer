using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool checkPoint;
    // Start is called before the first frame update
    void Start()
    {
        checkPoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cart1") || other.CompareTag("Cart2") || other.CompareTag("Cart3") || other.CompareTag("Cart4")){
            checkPoint = true;
        }
    }
}
