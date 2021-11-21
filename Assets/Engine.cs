using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AllStates { START, PLAYER, ENEMY, TRANSITION, RESET }

public class Engine : MonoBehaviour
{ 
    public GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3;
    public GameObject playerPrefab;

    public Transform playerSpawn;
    public Transform enemySpawn1, enemySpawn2, enemySpawn3;
    public bool enemyTrue1, enemyTrue2, enemyTrue3;

    public AllStates state;

    UnitAttributes playerUnit;
    UnitAttributes enemyUnit1, enemyUnit2, enemyUnit3;

    void Start()
    {
        state = AllStates.START;
        StartBattle();
    }

    void StartBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<UnitAttributes>();

        if (enemyTrue1)
        {
            GameObject enemy1GO = Instantiate(enemyPrefab1, enemySpawn1);
            enemyUnit1 = enemy1GO.GetComponent<UnitAttributes>();
        }

        if (enemyTrue2)
        {
            GameObject enemy2GO = Instantiate(enemyPrefab2, enemySpawn2);
            enemyUnit2 = enemy2GO.GetComponent<UnitAttributes>();
        }

        if (enemyTrue3)
        {
            GameObject enemy3GO = Instantiate(enemyPrefab3, enemySpawn3);
            enemyUnit3 = enemy3GO.GetComponent<UnitAttributes>();
        }
    }
}
