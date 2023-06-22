using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMovement : MonoBehaviour
{
    protected NavMeshAgent agent;
    private bool m_isAllowedToMove = true;
    protected NavMeshPath path = null;
    public float maximumTravelDistance;

    public bool IsAllowedToMove { get => m_isAllowedToMove; set => MovementStateIsChanged(value); }

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        maximumTravelDistance = agent.stoppingDistance * 10;
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

    public bool CanGetToDestination(Vector3 destination)
    {
        path = new NavMeshPath();
        // Check if path can be made
        if (agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
        {

            return true;
        }
        return false;
    }

    public bool GetDistance(Vector3 point, out float distance)
    {
        distance = -1;
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            float length = 0f;
            for (int i = 1; i < path.corners.Length; i++)
            {
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            distance = length;
            return true;

        }
        return false;
    }

    void MovementStateIsChanged(bool value)
    {
        if (agent != null)
        {
            agent.ResetPath();
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
            m_isAllowedToMove = value;
        }
        
    }


}
