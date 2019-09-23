using UnityEngine.AI;
using UnityEngine;

public class AiCart : MonoBehaviour
{
    public NavMeshAgent myCart;
    public Transform triggerBox;

    // Update is called once per frame
    void Update()
    {
        myCart.SetDestination(triggerBox.position);
    }
}
