using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Camera sceneCamera;
    private Animator animator;
    private bool isInCombat = false;
    public float attackRange = 10f;
    //public GameObject outOfRangePanel;
    //public LineRenderer attackRangeCircle;
    public int circleSegments = 360;
    public Interactable focus;
    private WaitCoroutine waitCoroutine = new WaitCoroutine();

    public const string AttackableLayer = "Attackable";
    public const string InteractableLayer = "Interactable";

    private PlayerMovement movement;

    // Distance for Move
    public TextMeshProUGUI distanceText;

    public GameObject distance;
    private CombatController combat;

    public enum ClickType
    {
        NONE,
        UI,
        Move,
        Interact
    }
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        //distanceText.gameObject.SetActive(false);
        combat = GetComponent<CombatController>();

    }

    void Update()
    {
        if (!combat.IsInCombat)
        {
            SetCombatMode();

            if (isInCombat)
            {
                //attackRangeCircle.enabled = true;
                //CreateAttackRangeCircle();
            }

            if (!isInCombat)
            {
                //attackRangeCircle.enabled = false;
            }

            if (!isInCombat && Input.GetMouseButtonDown(1))
            {
                RemoveFocus();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                CombatManager.GetInstance().InitiateCombat();
            }

            if (isInCombat && Input.GetMouseButtonDown(1))
            {
                AttackTarget();
            }
            //else if (!isInCombat && Input.GetMouseButtonDown(0))
            if (LeftMouseClicked())
            {
                Interactable interactable;
                Vector3 point;
                ClickType clickType = GetClickType(out point, out interactable);
                switch (clickType)
                {
                    case ClickType.UI:
                        Debug.Log("Clicked over UI");
                        break;
                    case ClickType.Move:
                        movement.MovePlayer(point);
                        animator.SetBool("isRunning", true);
                        break;
                    case ClickType.Interact:
                        movement.MovePlayer(point);
                        SetFocus(interactable);
                        Debug.Log("Clicked to interract");
                        break;
                }


            }
            if (movement.HasArrived())
            {
                animator.SetBool("isRunning", false);
            }
        }

        
        // T for test
        if (Input.GetKeyDown(KeyCode.T))
        {
            // To toggle combat
            // if player is not in combat, running, but the combat is triggered mid run,
            // reset the animation
            if (!combat.IsInCombat)
            {
                animator.SetBool("isRunning", false);
            }
            combat.IsInCombat = !combat.IsInCombat;
        }

        //ShowDistance();


        
        
    }

    private void ShowDistance()
    {
        Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            NavMeshPath path = new NavMeshPath();
            if (movement.agent.CalculatePath(hit.point, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                float lng = 0f;
                for (int i = 1; i < path.corners.Length; i++)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                distanceText.text = lng.ToString("F1");
                distanceText.rectTransform.localPosition = Input.mousePosition - new Vector3(960f, 0f) - new Vector3(0f, 540f);


            }
        }
    }

    private void InCombat()
    {
        
    }

    public void Move()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                
            }

            NavMeshPath navMeshPath = new NavMeshPath();
            //if (agent.CalculatePath(hit.point, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            //{
            //    agent.SetPath(navMeshPath);
            //    animator.SetBool("isRunning", true);                
            //}           
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
    }

    void AttackTarget()
    {
        Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject target = hit.collider.gameObject;

            // Check if the clicked object has the desired layer or tag
            if (target.layer == LayerMask.NameToLayer(AttackableLayer))
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                // Perform actions on the selected target

                if (distanceToTarget > attackRange)
                {
                    //ShowOutOfRangeWarning();
                    Debug.Log("Out of range");
                    //StartCoroutine(waitCoroutine.DisablePanel(outOfRangePanel, 2f));
                }
                else
                {
                    //HideOutOfRangeWarning();
                    Debug.Log("Attacking!");
                    ObjectDamage attackable = target.GetComponent<ObjectDamage>();
                    if (attackable != null)
                    {
                        attackable.TakeDamage(10);
                    }
                }
            }
            else
            {
                Move();
            }
        }
    }

    void SetCombatMode()
    {
        // Initially, when entering combat mode, all movements are locked until initiative is decided and the character's turn comes
        // Refer to the AttackTarget() method for the above
        if (Input.GetKeyDown(KeyCode.C))
        {
            isInCombat = !isInCombat;
            Debug.Log("Combat mode is set to " + isInCombat);
        }
    }

    private bool LeftMouseClicked()
    {
        return Input.GetMouseButtonDown(0);
    }

    public ClickType GetClickType(out Vector3 point, out Interactable interactable)
    {
        interactable = null;
        point = Vector3.zero;


        // clicked over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return ClickType.UI;
        }
        else
        {
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
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
                    return ClickType.Interact;
                }
                return ClickType.Move;

                
            }

        }
        return ClickType.NONE;
    }

}
