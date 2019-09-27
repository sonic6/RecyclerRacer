using UnityEngine.AI;
using UnityEngine;

public class AiCart : MonoBehaviour
{
    public NavMeshAgent myCart;
    public Transform previousDestination;
    public Transform currentDestination;

    // Update is called once per frame
    void Update()
    {
        myCart.SetDestination(currentDestination.position);
    }

    public void GetDestination(Transform destination)
    {
        previousDestination = currentDestination;
        currentDestination = destination;
    }
}
