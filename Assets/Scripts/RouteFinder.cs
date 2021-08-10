using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RouteFinder : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    public Vector3 next;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector3(0, 1.45f, 42.5f);
        agent.SetDestination(destination);
        //agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
