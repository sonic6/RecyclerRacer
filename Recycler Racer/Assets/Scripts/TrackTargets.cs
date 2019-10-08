using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTargets : MonoBehaviour
{
    public Color color;
    public List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Transform[] targets = GetComponentsInChildren<Transform>();
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
}
