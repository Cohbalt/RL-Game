using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public string unitName;
    public int unitMaxHealth;
    public int unitCurrentHealth;
    public int unitLuck;
    public int unitAttack;
    public int unitDefense;
    
    public void takeDamage(int damage)
    {
        Animator anim;
        anim = GetComponentInChildren<Animator>();
        anim.Play("Hurt");

        if (damage >= unitCurrentHealth)
        {
            unitCurrentHealth = 0;
        }

        else
        {
            unitCurrentHealth -= damage;
        }

    }

    public void setStats(List<int> stats)
    {
        unitMaxHealth = stats[0];
        unitCurrentHealth = stats[0];
        unitLuck = stats[1];
        unitAttack = stats[2];
        unitDefense = stats[3];
    }

}
