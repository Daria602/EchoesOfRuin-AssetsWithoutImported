using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{

    public int maxHP = 100; 
    private int currentHP; 

    void Start()
    {
        currentHP = maxHP; 
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        Debug.Log("Evil box HP is " + currentHP);
        if (currentHP <= 0)
        {
            currentHP = 0;
            Destroy(gameObject); 
        }
    }
}
