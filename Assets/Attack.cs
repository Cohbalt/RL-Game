using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{

    public int damage;
    public int cost;
    public int type;

    public Attack(int i = 1)
    {
        damage = i;
        type = i;
        cost = i;
    }

    public void randomize(int type, int quality)
    {
        damage = Random.Range(1, quality * type);
        cost = Random.Range(1, type);
        this.type = type;
    }
}
