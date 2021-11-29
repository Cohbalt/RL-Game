using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Map
{
    public List<Tile> tiles = new List<Tile>();

    public Map()
    {
        for (int i = 0; i < 5; i++)
        {
            Tile temp = new Tile(i + 1);
            tiles.Add(temp);
        }
    }

    public class Tile
    {
        public int background, thresh;
        public List<Enemy> enemies;
        public List<int> enemyTrue;

        public Tile(int thresh)
        {
            this.thresh = thresh;
            Random rnd = new Random();
            background = rnd.Next(0, 1);

            /*for (int i = 0; i < enemies.Count; i++)
            {
                if (i != 1)
                {
                    enemyTrue[i] = rnd.Next(0, 1);
                }
                else
                {
                    enemyTrue[i] = 1;
                }

                if (enemyTrue[i] == 1)
                {
                    enemies[i].En
                }
            }*/
        }

    }

    public class Enemy
    {
        int health;
        int max;
        int luck;
        int attack;
        int defense;
        int numAttacks;
        //GameObject enemType;
        Enemy(int i)
        {
            max = i;
            Random rnd = new Random();
            health = rnd.Next(1, max);
            luck = rnd.Next(1, max);
            attack = rnd.Next(1, max);
            defense = rnd.Next(1, max);
            numAttacks = rnd.Next(1, max);

        }
    }
}
