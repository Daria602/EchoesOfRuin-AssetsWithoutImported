using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMovement : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    private bool m_isAllowedToMove = true;
    protected NavMeshPath path = null;

    public bool IsAllowedToMove { get => m_isAllowedToMove; set => m_isAllowedToMove = value; }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
 
    public void SetPath()
    {
        agent.SetPath(path);

    }

    public bool HasArrived()
    {
        // Check if arrived at destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Arrived
                    return true;
                }
            }
        }
        return false;
    }

    protected bool CanGetToDestination(Vector3 destination)
    {
        path = new NavMeshPath();
        // Check if path can be made
        if (agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
        {

            return true;
        }
        return false;
    }
}
