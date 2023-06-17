using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : BaseMovement
{
    public Vector3[] positionsToMove = null;
    private int posToMoveIndex = -1;


    // To set the positions
    public Camera sceneCamera = null;

    private CombatController combat;

    private void Start()
    {
        combat = GetComponent<CombatController>();

        // TODO: remove this
        combat.IsInCombat = false;
    }

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
        if (!combat.IsInCombat)
        {
            if (IsAllowedToMove)
            {
                if (positionsToMove.Length == 0) return;
                MoveStatic();
            }
            else
            {
                agent.ResetPath();
            }
        }
        
    }

    public void MoveNPC(Vector3 point)
    {
        if (CanGetToDestination(point))
        {
            SetPath();
        }
    }

    private void MoveStatic()
    {
        if (HasArrived())
        {
            gameObject.GetComponent<Animator>().SetBool("isRunning", false);
            posToMoveIndex =
                (posToMoveIndex + 1 == positionsToMove.Length) ? 0 : posToMoveIndex + 1;
            if (CanGetToDestination(positionsToMove[posToMoveIndex]))
            {
                gameObject.GetComponent<Animator>().SetBool("isRunning", true);
                SetPath();

            }

        }
    }

}
