using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Tile : MonoBehaviour
{
    public int background;
    public List<Enemy> enemies;
    public List<int> enemyTrue;

    public void generate(int thresh)
    {
        Random rnd = new Random();
        background = rnd.Next(0, 1);

        for (int i = 0; i < enemies.Count; i++)
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
                enemies[i].generate(thresh);
            }
        }
    }
}
