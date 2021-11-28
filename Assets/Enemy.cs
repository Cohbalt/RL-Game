using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    public int health;
    public int luck;
    public int attack;
    public int defense;
    public int numAttacks;
    public GameObject enemType;

    public void generate(int thresh)
    {
        Random rnd = new Random();
        luck = rnd.Next(1, thresh);
        attack = rnd.Next(1, thresh);
        defense = rnd.Next(1, thresh);
        health = rnd.Next(1, thresh);
        numAttacks = rnd.Next(1, thresh);
    }
}
