using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : BaseMovement
{
    public Vector3[] positionsToMove = null;
    private int posToMoveIndex = -1;
    private enum NPCState
    {
        ChoosingDirection,
        Walking,
        Staying
    }
    private NPCState state = NPCState.ChoosingDirection;


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
                state = NPCState.ChoosingDirection;
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
        switch (state)
        {
            case NPCState.ChoosingDirection:
                // choose position to move to
                posToMoveIndex =
                (posToMoveIndex + 1 == positionsToMove.Length) ? 0 : posToMoveIndex + 1;
                if (CanGetToDestination(positionsToMove[posToMoveIndex]))
                {
                    SetPath();
                    state = NPCState.Walking;
                    gameObject.GetComponent<Animator>().SetBool("isRunning", true);

                }
                break;
            case NPCState.Walking:
                if (HasArrived())
                {
                    state = NPCState.Staying;
                    gameObject.GetComponent<Animator>().SetBool("isRunning", false);
                }
                // walk untill there and check if arrived
                break;
            case NPCState.Staying:
                // wait for few seconds
                StartCoroutine(WaitBeforeLeaving(3f));
                break;
        }

    }
    IEnumerator WaitBeforeLeaving(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = NPCState.ChoosingDirection;
    }
}
