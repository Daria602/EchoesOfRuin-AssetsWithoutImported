using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;
using UnityEngine.UI;

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
    //private WaitCoroutine waitCoroutine = new WaitCoroutine();

    public const string AttackableLayer = "Attackable";
    public const string InteractableLayer = "Interactable";

    private PlayerMovement movement;

    // Distance for Move
    public TextMeshProUGUI distanceText;

    
    private PlayerCombat combat;

    public int XP = 0;
    public int gold = 0;


    public Slider XPSlider;
    public Slider HealthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI xpText;
    public PlayerStats stats;



    //public CombatManager combatManager;

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
        combat = GetComponent<PlayerCombat>();
        stats = GetComponent<PlayerStats>();
        XPSlider.minValue = 0;
        XPSlider.maxValue = currentThreshold;
        XPSlider.value = XP;
        HealthSlider.minValue = 0;
        HealthSlider.maxValue = GetComponent<PlayerHealth>().CurrentMaxHealth;
        HealthSlider.value = GetComponent<PlayerHealth>().currentHealth;
        xpText.text = "Level " + stats.characterLevel.ToString();

    }

    public int currentThreshold = 1000;

    public void CalculateNewThreshold()
    {
        currentThreshold = 500 * ((stats.characterLevel + 1) * (stats.characterLevel+1)) - (500 * (stats.characterLevel+1));
    }

    public bool PassedCurrentThreshold()
    {
        return XP >= currentThreshold;
    }

    public void AddXP(int XPValue)
    {
        
        XP += XPValue;
        

        if (PassedCurrentThreshold())
        {
            stats.characterLevel++;
            CalculateNewThreshold();
            XPSlider.minValue = XPValue;
            XPSlider.maxValue = currentThreshold;
        }
        xpText.text = "Level " + stats.characterLevel.ToString();
        XPSlider.value = XPValue;
    }

    void Update()
    {
        HealthSlider.value = GetComponent<PlayerHealth>().currentHealth;
        healthText.text = GetComponent<PlayerHealth>().currentHealth.ToString();
        if (!combat.IsInCombat)
        {
           
            if (LeftMouseClicked())
            {
                Interactable interactable;
                Vector3 point;
                ClickType clickType = GetClickType(out point, out interactable);
                switch (clickType)
                {
                    case ClickType.UI:
                        RemoveFocus();
                        Debug.Log("Clicked over UI");
                        break;
                    case ClickType.Move:
                        RemoveFocus();
                        movement.MovePlayer(point);
                        animator.SetBool("isRunning", true);
                        Debug.Log(point);
                        break;
                    case ClickType.Interact:
                        animator.SetBool("isRunning", true);
                        movement.MovePlayer(point);
                        RemoveFocus();
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

        
        

        //ShowDistance();
        
    }

    public void RemoveFocus(Interactable interactable = null)
    {
        if (focus != null && focus != interactable)
        {
            focus.IsFocus = false;
        }
    }

    public void StartFight()
    {
        // To toggle combat
        // if player is not in combat, running, but the combat is triggered mid run,
        // reset the animation
        if (!combat.IsInCombat)
        {
            animator.SetBool("isRunning", false);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30f);
            List<int> participants = new List<int>();
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Attackable")
                {
                    participants.Add(hitCollider.gameObject.GetComponent<CombatController>().characterId);
                }
            }
            if (participants.Count > 0)
            {
                // Player's Id is 0
                participants.Add(0);
                InitiateCombat(participants);
            }

        }
    }

    private void InitiateCombat(List<int> participantsIds)
    {
        
        CombatManager.GetInstance().StartCombat(participantsIds);
        CombatManager.GetInstance().isCombatGoing = true;
        //combatManager.InitiateCombat(participants);
        //foreach (var enemy in participants)
        //{

        //    enemy.GetComponent<CombatController>().IsInCombat = true;
        //    enemy.transform.LookAt(transform);
        //    Debug.Log("Enemy is in combat: " + enemy.GetComponent<CombatController>().IsInCombat);
        //}

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
