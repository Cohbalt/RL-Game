using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public Tile[,] tiles = new Tile[5, 3];
    public bool[,] tileTrue = new bool[5, 3];
    public Queue<int> BFS;

    public void randomizeMap(int difficulty = 1)
    {
        int height = 1;
        for (int i = 0; i < 5; i++)
        {
            Tile temp = new Tile();
            temp.randomizeTile((i + 1) * 5);
            tiles[i, height] = temp;
            tileTrue[i, height] = true;
            height += Random.Range(-1, 2);
            if (height < 0)
            {
                height = 0;
            }
            if (height > 2)
            {
                height = 2;
            }
        }
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!tileTrue[i , j] && Random.Range(0,5) > i)
                {
                    Tile temp = new Tile();
                    temp.randomizeTile(i + 5);
                    tiles[i, j] = temp;
                    tileTrue[i, j] = true;

                }
                else if (Random.Range(0,2) == 1)
                {
                    Tile temp = new Tile();
                    temp.randomizeTile((i + 3) * i);
                    tiles[i, j] = temp;
                    tileTrue[i, j] = true;
                }
                else
                {
                    tileTrue[i, j] = false;
                }
            }
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