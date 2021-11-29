using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public List<Tile> tiles = new List<Tile>();

    public void randomizeMap(int difficulty = 1)
    {
        for (int i = 0; i < 5; i++)
        {
            Tile temp = new Tile();
            temp.randomizeTile((i + 1) * 5);
            tiles.Add(temp);
        }
    }

    public class Tile
    {
        public int background, thresh;
        public List<Enemy> enemies;
        public List<int> enemyTrue;

        public void randomizeTile(int thresh)
        {
            enemyTrue = new List<int>();
            enemies = new List<Enemy>();
            background = Random.Range(0, 2);

            for (int i = 0; i < 3; i++)
            {

                if (i != 1)
                {
                    enemyTrue.Add(Random.Range(0, 2));
                }
                else
                {
                    enemyTrue.Add(1);
                }

                Enemy temp = new Enemy();
                temp.randomizeEnemy(thresh);
                enemies.Add(temp);
            }
        }   
        public class Enemy
        {
            int health;
            int luck;
            int attack;
            int defense;
            int numAttacks;
            public int enemyType;
            public List<int> stats;
            
            public void randomizeEnemy(int thresh)
            {
                stats = new List<int>();

                health = Random.Range(1, thresh + 1);
                luck = Random.Range(1, thresh + 1);
                attack = Random.Range(1, thresh + 1);
                defense = Random.Range(1, thresh + 1);
                numAttacks = Random.Range(1, thresh + 1);
                enemyType = Random.Range(0, 2);

                stats.Add(health);
                stats.Add(luck);
                stats.Add(attack);
                stats.Add(defense);
            }

        }


    }

}