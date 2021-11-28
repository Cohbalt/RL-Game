using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttributes : MonoBehaviour
{
    public string unitName;
    public int unitMaxHealth;
    public int unitCurrentHealth;
    public int unitLuck;
    public int unitLevel;
    public int numAttacks;
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

    public void setStats(int health, int luck, int level, int attack, int defense, int numAttacks)
    {
        unitMaxHealth = health;
        unitCurrentHealth = health;
        unitLuck = luck;
        unitLevel = level;
        unitAttack = attack;
        unitDefense = defense;
    }

}
