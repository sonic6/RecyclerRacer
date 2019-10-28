using System.Collections.Generic;
using UnityEngine;

public class TrackTargets : MonoBehaviour
{
    public Color color;
    public List<Transform> nodes = new List<Transform>();
    Transform[] targets;
    [Tooltip("insert here a Gameobject that will be used to identify the center position of the race track")]
    public Transform trackCenter;
    public static Transform nxtTarget;
    int nxtInt = 1;

    private void Start()
    {
        targets = GetComponentsInChildren<Transform>();
        nxtTarget = targets[1];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        targets = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != transform)
                nodes.Add(targets[i]);
        }

        for(int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero;

            if (i > 0)
                previousNode = nodes[i - 1].position;
            else if (i == 0 && nodes.Count > 1)
                previousNode = nodes[nodes.Count - 1].position;

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.5f);
        }
    }

    public void RayThroughTargets()
    {
        
        foreach(Transform target in targets)
        {
            Vector3 rayStart = new Vector3(trackCenter.position.x, target.position.y, trackCenter.position.z);
            Vector3 rayFinish = new Vector3(target.position.x, target.localPosition.y, target.position.z);
            Ray ray = new Ray(rayStart, rayFinish);
            RaycastHit hit;
            Debug.DrawRay(rayStart, rayFinish, Color.green);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer == 8 /*Layer nr 8 is the player layer*/ && targets[nxtInt] == target.transform)
            {
                if (nxtInt < targets.Length - 1)
                    nxtInt++;
                else
                    nxtInt = 1;
                nxtTarget = targets[nxtInt];
            }
        }
        
    }

    private void Update()
    {
        RayThroughTargets();
    }
}
