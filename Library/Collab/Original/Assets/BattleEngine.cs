using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum allStates { START, PLAYER, ENEMY, TRANSITION, RESET, PROGRESS }

public class BattleEngine : MonoBehaviour
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
    int attackCount;

    public List<GameObject> backgrounds;
    public List<GameObject> enemies;
    public GameObject hero;

    public GameObject map;
    public int currentIndex;
    private List<int> Attacks;
    private List<UnitAttributes> Targets;

    public Transform playerSpawn;
    public Transform backgroundAnchor;
    public Transform enemySpawn1, enemySpawn2, enemySpawn3;
    public bool enemyTrue1, enemyTrue2, enemyTrue3;

    public allStates state;

    UnitAttributes playerUnit;
    UnitAttributes enemyUnit1, enemyUnit2, enemyUnit3;

    //private GameObject

    void Start()
    {
        currentIndex = 0;
        state = allStates.START;
        StartCoroutine(startBattle());
    }

    IEnumerator startBattle()
    {
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

        state = allStates.PLAYER;
        
        yield return new WaitForSeconds(.5f);
        
        playerTurn();
    }

    void playerTurn()
    {
        
        Attacks = new List<int>();
        Targets = new List<UnitAttributes>();
        attackCount = 0;

    }
    
    public void checkEndTurn()
    {
        if (state != allStates.PLAYER)
        {
            return;
        }

        state = allStates.TRANSITION;
        StartCoroutine(transition());
    }

    public void attackCheck()
    {
        if (state != allStates.PLAYER)
        {
            return;
        }

        Attacks.Add(playerUnit.unitAttack);
        Targets.Add(enemyUnit1);
        attackCount++;
    }

    void playerAttack(UnitAttributes enemyunit, int attack)
    {
        enemyunit.takeDamage(attack);   
    }

    IEnumerator transition()
    {
        for (int i = 0; i < Attacks.Count; i++)
        {         
            playerAttack(Targets[i], Attacks[i]);
            yield return new WaitForSeconds(.5f);
        }

        state = allStates.ENEMY;
        StartCoroutine(enemyTurn());
    }

    IEnumerator enemyTurn()
    {
        if (enemyTrue1)
        {
            enemyAttack(enemyUnit1);
            yield return new WaitForSeconds(.5f);
        }

        if (enemyTrue2)
        {
            enemyAttack(enemyUnit2);
            yield return new WaitForSeconds(.5f);
        }

        if (enemyTrue3)
        {
            enemyAttack(enemyUnit3);
            yield return new WaitForSeconds(.5f);
        }

        if (enemyTrue1 || enemyTrue2 || enemyTrue3)
        {
            state = allStates.PLAYER;
            playerTurn();
        }

        else
        {
            state = allStates.PROGRESS;
            progress();
        }
        
    }

    void enemyAttack(UnitAttributes enemy)
    {
        playerUnit.takeDamage(enemy.unitAttack);

        if (playerUnit.unitCurrentHealth == 0)
        {
            state = allStates.RESET;
            endGame();
        }
    }

    void endGame()
    {
        SceneManager.LoadScene(0);
    }

    void progress()
    {
        SceneManager.LoadScene(2);
        startBattle();
    }
}
