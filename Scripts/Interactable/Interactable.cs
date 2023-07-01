using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    bool m_isFocus = false;
    protected Transform player;
    bool hasInteracted = false;
    public OutlineController outline;

    public bool IsFocus { get => m_isFocus; set => IsFocusChanged(value); }

    //Implement this for interactable objects
    public virtual void Interact()
    {
        //Debug.Log("Interacting with " + transform.name);
    }

    private void Start()
    {
        outline = GetComponent<OutlineController>();
    }

    private void Update()
    {
        
        if (m_isFocus && !hasInteracted)
        {
            
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                //Debug.Log("Got here");
                Interact();
                hasInteracted = true;
                IsFocus = false;

            }
        }
    }

    public void OnFocused(Transform playerTransform)
    {
        IsFocus = true;
        player = playerTransform;
        hasInteracted = false;

    }
    public void OnDefocused()
    {
        IsFocus = false;
        player = null;
        hasInteracted = false;
        
    }

    private void IsFocusChanged(bool value)
    {
        if (m_isFocus != value)
        {
            m_isFocus = value;

            if (m_isFocus)
            {
                if (outline != null)
                {
                    outline.IsEnabled = true;
                    // 1 for yellow
                    outline.ChangeColor(1);
                }
                
            }
            else
            {
                if (outline != null)
                {
                    outline.IsEnabled = false;
                    outline.ChangeColor(0);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
