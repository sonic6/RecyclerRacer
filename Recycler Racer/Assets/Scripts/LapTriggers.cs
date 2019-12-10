using UnityEngine;

public class LapTriggers : MonoBehaviour
{
    public GameObject[] triggerBoxes;

    private void Awake()
    {
        //Give the trigger boxes which are children for this object a new component script "RaceTrackTriggers"

        foreach(GameObject box in triggerBoxes)
        {
            box.AddComponent<RaceTrackTriggers>();
        }

        for (int i = 0; i < triggerBoxes.Length; i++)
        {
            triggerBoxes[i].GetComponent<RaceTrackTriggers>().myLapNr = i + 1;
        }
    }
}

public class RaceTrackTriggers : MonoBehaviour
{
    public int myLapNr; //The number of this trigger box in the list of lap trigger boxes
    


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<LapInfo>())
        {
            LapInfo cart = other.gameObject.GetComponent<LapInfo>();
            if (myLapNr == cart.lastLapTriggerPassed + 1)
                cart.lastLapTriggerPassed++;
        }
    }
}
