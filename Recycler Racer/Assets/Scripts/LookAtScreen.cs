using UnityEngine;

public class LookAtScreen : MonoBehaviour
{
    
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
