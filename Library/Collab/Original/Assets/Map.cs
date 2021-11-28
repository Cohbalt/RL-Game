using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Map : MonoBehaviour
{
    public List<Tile> tiles;

    public void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            tiles[i].generate(i + 1);
        }
    }
}
