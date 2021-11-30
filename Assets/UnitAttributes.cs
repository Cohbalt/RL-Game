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
    public bool isAlive;
    
    public bool takeDamage(int damage)
    {
        if (unitCurrentHealth == 0)
        {
            return false;
        }

        Animator anim;
        anim = GetComponentInChildren<Animator>();
        

        if (damage >= unitCurrentHealth)
        {
            unitCurrentHealth = 0;
            anim.Play("Death");
            return true;
        }

        else
        {
            unitCurrentHealth -= damage;
            anim.Play("Hurt");
            return true;
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
