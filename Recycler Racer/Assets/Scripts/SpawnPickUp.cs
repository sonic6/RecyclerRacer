using UnityEngine;
using System.Collections;


public class SpawnPickUp : MonoBehaviour
{
    [SerializeField] GameObject[] pickUps;
    [SerializeField] float throwForce;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        
        GameObject nyObject = Instantiate(pickUps[Random.Range(0, pickUps.Length)], transform.position, transform.rotation);
        Rigidbody rb = nyObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        nyObject.AddComponent<OnTouchedFloor>();
        nyObject.GetComponent<BoxCollider>().isTrigger = false;
        nyObject.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(throwForce * 0.75f, throwForce * 5f));
        yield return new WaitForSeconds(10f);
        StartCoroutine(Spawn());
    }
    
}

public class OnTouchedFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            Destroy(GetComponent<Rigidbody>());
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
