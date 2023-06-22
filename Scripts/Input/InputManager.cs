using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    // Singleton package
    private static InputManager instance;
    public static InputManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many InputManager instances");
        }
        instance = this;
    }

    public bool LeftMouseClicked()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool RightMouseClicked()
    {
        return Input.GetMouseButtonDown(1);
    }

    public int[] CameraMoveInput()
    {

        int[] moveDirection = { 0, 0, 0, 0}; // 0 -> W, 1 -> S, 2 -> A, 3 -> D
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection[1] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection[3] = 1;
        }
        return moveDirection;
    }

    public int[] CameraRotateInput()
    {
        int[] rotateDirection = { 0, 0 };
        if (Input.GetKey(KeyCode.Q))
        {
            rotateDirection[0] = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateDirection[1] = 1;
        }
        return rotateDirection;
    }

    public bool ToggleInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            return true;
        }
        return false;
    }

    public bool ToggleJournalInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            return true;
        }
        return false;
    }

    public Constants.ClickType GetClickType(out Vector3 point, out Interactable interactable)
    {
        interactable = null;
        point = Vector3.zero;

        // clicked over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return Constants.ClickType.UI;
        }
        else
        {
            Ray ray = CameraManager.GetInstance().GetSceneCamera().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if hit interactable
            // else it hit groung (maybe not walkable ground, but still ground)
            if (Physics.Raycast(ray, out hit))
            {
                point = hit.point;
                bool possibleInteractable = hit.collider.GetComponent<Interactable>() != null;
                if (possibleInteractable)
                {
                    interactable = hit.collider.GetComponent<Interactable>();
                    return Constants.ClickType.Interact;
                }
                return Constants.ClickType.Move;
            }
        }
        return Constants.ClickType.NONE;
    }

    public bool EscapeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }
        return false;
    }
}
