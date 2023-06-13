using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement
{
    private void Update()
    {
        if (!IsAllowedToMove)
        {
            agent.ResetPath();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {

            IsAllowedToMove = !IsAllowedToMove;
            Debug.Log(IsAllowedToMove);
        }

    }
    public void MovePlayer(Vector3 point)
    {
        if (CanGetToDestination(point))
        {
            SetPath();
        }
    }
}
