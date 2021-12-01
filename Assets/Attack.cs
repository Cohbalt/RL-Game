using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attack class that holds variables so we can easily make and retrieve variables for a weapon
public class Attack
{
    public int damage;
    public int cost;
    public int type;

    //Attack constructor, sets the defaults to 1
    public Attack(int i = 1)
    {
        damage = i;
        type = i;
        cost = i;
    }

    //Randomization function that takes a type and quality in order to randomize the weapons stats
    public void randomize(int type, int quality)
    {
        damage = Random.Range(1, quality * type);
        cost = Random.Range(1, type);
        this.type = type;
    }
}
