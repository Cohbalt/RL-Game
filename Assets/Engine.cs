using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum AllStates { START, PLAYER, ENEMY, TRANSITION, RESET, PROGRESS }

public class Engine : MonoBehaviour
{
    public Text currhp;
    public Text lvl;
    public Text lck;
    public int backgroundSelector;
    //public Text maxhp;
    //private Animation anim;
    public GameObject enemyPrefab1, enemyPrefab2, enemyPrefab3;
    public GameObject background, backgroundPrefab;
    public GameObject playerPrefab;
    public GameObject playerGO, enemy1GO, enemy2GO, enemy3GO;
    public GameObject endTurnButton, attackButton;

    public Map map;
    private List<int> Attacks;
    private List<UnitAttributes> Targets;

    public Transform playerSpawn;
    public Transform backgroundAnchor;
    public Transform enemySpawn1, enemySpawn2, enemySpawn3;
    public bool enemyTrue1, enemyTrue2, enemyTrue3;

    public AllStates state;

    UnitAttributes playerUnit;
    UnitAttributes enemyUnit1, enemyUnit2, enemyUnit3;

    //private GameObject

    void Start()
    {

        state = AllStates.START;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        background = Instantiate(backgroundPrefab, backgroundAnchor); 

        playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<UnitAttributes>();
        currhp.text = playerUnit.unitCurrentHealth.ToString() + "/" + playerUnit.unitMaxHealth.ToString();
        lck.text = playerUnit.unitLuck.ToString();
        lvl.text = playerUnit.unitLevel.ToString();


        if (enemyTrue1)
        {
            enemy1GO = Instantiate(enemyPrefab1, enemySpawn1);
            enemyUnit1 = enemy1GO.GetComponent<UnitAttributes>();
        }

        if (enemyTrue2)
        {
            enemy2GO = Instantiate(enemyPrefab2, enemySpawn2);
            enemyUnit2 = enemy2GO.GetComponent<UnitAttributes>();
        }

        if (enemyTrue3)
        {
            enemy3GO = Instantiate(enemyPrefab3, enemySpawn3);
            enemyUnit3 = enemy3GO.GetComponent<UnitAttributes>();
        }

        state = AllStates.PLAYER;
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }

    void PlayerTurn()
    {
        Attacks = new List<int>();
        Targets = new List<UnitAttributes>();

    }
    public void checkEndTurn()
    {
        if (state != AllStates.PLAYER)
        {
            return;
        }
        state = AllStates.TRANSITION;
        Transition();
    }

    public void attackCheck()
    {
        if (state != AllStates.PLAYER)
        {
            return;
        }
        Attacks.Add(playerUnit.attack);
        Targets.Add(enemyUnit1);
    }

    IEnumerator PlayerAttack(UnitAttributes enemyunit, int attack)
    {

        enemyunit.takeDamage(attack);
        yield return new WaitForSeconds(2f);
   

    }

    void Transition()
    {
        for (int i = 0; i < Attacks.Count; i++)
        {
            StartCoroutine(PlayerAttack(Targets[i], Attacks[i]));

        }
        //yield return new WaitForSeconds(2f);
        state = AllStates.ENEMY;
        enemyTurn();
    }

    void enemyTurn()
    {
        if (enemyTrue1)
        {
            StartCoroutine(enemyAttack(enemyUnit1));
        }

        if (enemyTrue2)
        {
            StartCoroutine(enemyAttack(enemyUnit2));
        }

        if (enemyTrue3)
        {
            StartCoroutine(enemyAttack(enemyUnit3));
        }
        if (enemyTrue1 || enemyTrue2 || enemyTrue3)
        {
            state = AllStates.PLAYER;
            PlayerTurn();
        }
        else
        {
            state = AllStates.PROGRESS;
            progress();

        }
        
    }

    IEnumerator enemyAttack(UnitAttributes enemy)
    {
        playerUnit.takeDamage(enemy.attack);
        if (playerUnit.unitCurrentHealth == 0)
        {
            state = AllStates.RESET;
            endGame();
        }
        yield return new WaitForSeconds(2f);
    }

    void endGame()
    {
        SceneManager.LoadScene(0);
    }
    void progress()
    {
        SceneManager.LoadScene(2);
        StartBattle();
    }
}
