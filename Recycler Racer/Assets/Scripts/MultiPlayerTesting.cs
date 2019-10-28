using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class MultiPlayerTesting : MonoBehaviour
{

    public Text myText;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("JoyX") > 0)
            myText.text = "was pressed";
    }
}
