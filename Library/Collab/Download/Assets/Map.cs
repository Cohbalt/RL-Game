using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Globals : MonoBehaviour
{
    public List<GameObject> backgrounds;
    public List<GameObject> enemies;
    public GameObject scene1, scene2;
    public GameObject enemy1, enemy2;
    public Map map;

    public Globals()
    {
        backgrounds = new List<GameObject>();
        backgrounds.Add(scene1);
        backgrounds.Add(scene2);
        enemies = new List<GameObject>();
        enemies.Add(enemy1);
        enemies.Add(enemy2);
        map = new();
    }
}

public class Scenes : MonoBehaviour
{
    public List<GameObject> backgrounds;
    public GameObject background1, background2;
}

public class enemyType : MonoBehaviour
{
    
}

public class Map : MonoBehaviour
{
    public List<Tile> tiles;
    void addTiles()
    {
        tiles = new List<Tile>();
        for (int i = 0; i < 5; i++)
        {
            Tile temp = new();
            temp.generate(10 * i);
        }



    }
}

public class Tile : MonoBehaviour
{
    GameObject background;
    List<Enemy> enemies;
    public void generate(int thresh)
    {
        Random rnd = new Random();
       //background = rnd.Next(0, 1);
        for (int i = 0; i < 3; i++)
        {
            Enemy enemyStats = new();
            enemies.Add(enemyStats);
        }
    }
}

public class Enemy : MonoBehaviour
{
    int health;
    int luck;
    int attack;
    int defense;
    GameObject enemType;
    void generate(int thresh)
    {
        Random rnd = new Random();
        luck = rnd.Next(1, thresh);
        attack = rnd.Next(1, thresh);
        defense = rnd.Next(1, thresh);
        health = rnd.Next(1, thresh);
    }

}

















/*public class Map : MonoBehaviour
{
    public List<Tile> tiles;


    public void generateMap()
    {
        tiles = new List<Tile>();

        for (int i = 0; i < 5; i++)
        {
            Tile temp = new();
            tiles.Add(temp);
        }
    }
}

public class Tile : MonoBehaviour
{
    int enemyNum;
    int luck;
    GameObject background;
    List<Enemy> enemyStats ;
    List<Enemy> enemyTypes;
    public Tile()
    {
        Globals temp = new();
        Random rnd = new Random();
        enemyNum = rnd.Next(1, 3);
        luck = rnd.Next(1, 10);
        background = temp.backgrounds[rnd.Next(0, 1)];
        for (int i = 0; i < enemyNum; i++)
        {
            int enemyType = rnd.Next(0, 1);
            ene.Add(enemyStats[enemyType]);
        }
    }
}

public class Enemy : MonoBehaviour
{
    int health;
    int defense;
    int attack;
    public Enemy(int luck)
    {
        Random rnd = new Random();
        health = rnd.Next(1, luck);
        defense = rnd.Next(1, luck);
        attack = rnd.Next(1, luck);
    }
}
*/