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
    public int attack;
    public int defense;
    public void takeDamage(int damage)
    {
        Animator anim;
        anim = GetComponentInChildren<Animator>();
        anim.Play("Hurt");

        unitCurrentHealth = unitCurrentHealth - damage;
    }

}
