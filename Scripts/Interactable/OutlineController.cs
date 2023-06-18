using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Outline[] outlineScripts;
    private bool m_isEnabled = true;

    
    
    public bool IsEnabled { get => m_isEnabled; set => IsEnabledChanged(value); }

    private void Start()
    {
        outlineScripts = GetComponentsInChildren<Outline>();
        IsEnabledChanged(false);
    }

    

    private void IsEnabledChanged(bool value)
    {
        if (m_isEnabled != value) 
        {
            for (int i = 0; i < outlineScripts.Length; i++)
            {
                outlineScripts[i].enabled = value;
            }
            m_isEnabled = value;
        }
        
    }

    // yellow for hover, stay yellow while coming to it 
    // red if is in combat and it's his turn
    public void ChangeColor(int color)
    {
        for (int i = 0;i < outlineScripts.Length;i++)
        {
            outlineScripts[i].color = color;
        }
    }



}
