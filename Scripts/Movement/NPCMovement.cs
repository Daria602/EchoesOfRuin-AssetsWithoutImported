using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : BaseMovement
{
    public Vector3[] positionsToMove = null;
    private int posToMoveIndex = -1;


    // To set the positions
    public Camera sceneCamera = null;

    // Update is called once per frame
    void Update()
    {
        //To set positions
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log(hit.point);
        //    }
        //}
        if (IsAllowedToMove)
        {
            if (positionsToMove.Length == 0) return;
            Move();
        } 
        else
        {
            agent.ResetPath();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            
            IsAllowedToMove = !IsAllowedToMove;
            Debug.Log(IsAllowedToMove);
        }
    }

    private void Move()
    {
        if (HasArrived())
        {

            posToMoveIndex =
                (posToMoveIndex + 1 == positionsToMove.Length) ? 0 : posToMoveIndex + 1;
            if (CanGetToDestination(positionsToMove[posToMoveIndex]))
            {
                SetPath();

            }

        }
    }

}
