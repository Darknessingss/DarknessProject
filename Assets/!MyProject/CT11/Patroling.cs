using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patroling : MonoBehaviour
{
    public List<Transform> waypoints;
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
            GoToRandomWaypoint();
    }

    void Update()
    {

        if (agent.remainingDistance < 0.5f)
        {
            GoToRandomWaypoint();
        }
    }

    void GoToRandomWaypoint()
    {
        int randomIndex = Random.Range(0, waypoints.Count);
        agent.SetDestination(waypoints[randomIndex].position);
    }
}