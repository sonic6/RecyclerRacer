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

        //SteerWhileMovingOnly(myCart.velocity.magnitude);
    }

    public void SetDestination(Transform destination)
    {
        previousDestination = currentDestination;
        currentDestination = destination;
    }

    //private void SteerWhileMovingOnly(float magnitude)
    //{
    //    if (magnitude < 50)
    //    {
    //        myCart.angularSpeed = 0;
    //        previousDestination = currentDestination;
    //        myCart.SetDestination(transform.forward);
    //    }
    //    else
    //    {
    //        myCart.angularSpeed = 120;
    //        myCart.SetDestination(previousDestination.position);
    //    }
    //}
}
