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
    public GameObject background;
    public GameObject playerGO, enemy1GO, enemy2GO, enemy3GO;
    public GameObject endTurnButton, attackButton;
    
    public int baseAttackCount;
    public int currentAttackCount;
    public int stage;

    public Map map;

    public List<Attack> heroAttacks;
    public List<UnitAttributes> queuedTargets;
    public List<Attack> queuedAttacks;

    public List<GameObject> backgrounds;
    public List<GameObject> enemies;
    public GameObject hero;
    public List<GameObject> buttons;

    //public GameObject map;
    public int currentIndex;

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
        heroAttacks = new List<Attack>();
        
        while (heroAttacks.Count < 3)
        {
            heroAttacks.Add(new Attack());
        }

        map = new Map();
        map.randomizeMap();
        stage = 0;
        baseAttackCount = 5;
        state = allStates.START;

        playerGO = Instantiate(hero, playerSpawn);
        playerUnit = playerGO.GetComponent<UnitAttributes>();

        StartCoroutine(startBattle());
    }

    IEnumerator startBattle()
    {
        background = Instantiate(backgrounds[map.tiles[stage].background], backgroundAnchor);

        enemyTrue1 = map.tiles[stage].enemyTrue[0] != 0;
        if (map.tiles[stage].enemyTrue[0] == 1)
        {
            enemy1GO = Instantiate(enemies[map.tiles[stage].enemies[0].enemyType], enemySpawn1);
            enemyUnit1 = enemy1GO.GetComponent<UnitAttributes>();
            enemyUnit1.setStats(map.tiles[stage].enemies[0].stats);
        }

        enemyTrue2 = map.tiles[stage].enemyTrue[1] != 0;
        if (map.tiles[stage].enemyTrue[1] == 1)
        {
            enemy2GO = Instantiate(enemies[map.tiles[stage].enemies[1].enemyType], enemySpawn2);
            enemyUnit2 = enemy2GO.GetComponent<UnitAttributes>();
            enemyUnit2.setStats(map.tiles[stage].enemies[1].stats);
        }

        enemyTrue3 = map.tiles[stage].enemyTrue[2] != 0;
        if (map.tiles[stage].enemyTrue[2] == 1)
        {
            enemy3GO = Instantiate(enemies[map.tiles[stage].enemies[2].enemyType], enemySpawn3);
            enemyUnit3 = enemy3GO.GetComponent<UnitAttributes>();
            enemyUnit3.setStats(map.tiles[stage].enemies[2].stats);
        }

        UpdatePlayerHP();

        state = allStates.PLAYER;
        
        yield return new WaitForSeconds(.5f);
        
        playerTurn();
    }

    void UpdatePlayerHP()
    {
        currhp.text = playerUnit.unitCurrentHealth.ToString() + "/" + playerUnit.unitMaxHealth.ToString();
        lck.text = playerUnit.unitLuck.ToString();
        lvl.text = (stage + 1).ToString();
    }

    void playerTurn()
    {
        currentAttackCount = baseAttackCount;
        queuedAttacks = new List<Attack>();
        queuedTargets = new List<UnitAttributes>();
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

    public void attackCheck1()
    {
        if (state != allStates.PLAYER || currentAttackCount < heroAttacks[0].cost)
        {
            return;
        }

        if (queuedAttacks.Count == queuedTargets.Count)
        {
            queuedAttacks.Add(heroAttacks[0]);
            currentAttackCount -= heroAttacks[0].cost;
        }

        else
        {
            currentAttackCount += queuedAttacks[queuedAttacks.Count - 1].cost;
            queuedAttacks.RemoveAt(queuedAttacks.Count - 1);
            queuedAttacks.Add(heroAttacks[0]);
            currentAttackCount -= heroAttacks[0].cost;
        }
    }

    public void attackCheck2()
    {
        if (state != allStates.PLAYER || currentAttackCount < heroAttacks[1].cost)
        {
            return;
        }

        if (queuedAttacks.Count == queuedTargets.Count)
        {
            queuedAttacks.Add(heroAttacks[1]);
            currentAttackCount -= heroAttacks[1].cost;
        }

        else
        {
            currentAttackCount += queuedAttacks[queuedAttacks.Count - 1].cost;
            queuedAttacks.RemoveAt(queuedAttacks.Count - 1);
            queuedAttacks.Add(heroAttacks[1]);
            currentAttackCount -= heroAttacks[1].cost;

        }
    }

    public void attackCheck3()
    {
        if (state != allStates.PLAYER || currentAttackCount < heroAttacks[2].cost)
        {
            return;
        }

        if (queuedAttacks.Count == queuedTargets.Count)
        {
            queuedAttacks.Add(heroAttacks[2]);
            currentAttackCount -= heroAttacks[2].cost;
        }

        else
        {
            currentAttackCount += queuedAttacks[queuedAttacks.Count - 1].cost;
            queuedAttacks.RemoveAt(queuedAttacks.Count - 1);
            queuedAttacks.Add(heroAttacks[2]);
            currentAttackCount -= heroAttacks[2].cost;

        }
    }

    public void addTarget1()
    {
        if (state != allStates.PLAYER || !enemyTrue1)
        {
            return;
        }

        if (queuedTargets.Count < queuedAttacks.Count)
        {
            queuedTargets.Add(enemyUnit1);
        }
        else if (queuedTargets.Count == 0)
        {
            return;
        }
        else
        {
            queuedTargets.RemoveAt(queuedTargets.Count - 1);
            queuedTargets.Add(enemyUnit1);
        }
    }

    public void addTarget2()
    {
        if (state != allStates.PLAYER || !enemyTrue2)
        {
            return;
        }

        if (queuedTargets.Count < queuedAttacks.Count)
        {
            queuedTargets.Add(enemyUnit2);
        }
        else if (queuedTargets.Count == 0)
        {
            return;
        }
        else
        {
            queuedTargets.RemoveAt(queuedTargets.Count - 1);
            queuedTargets.Add(enemyUnit2);
        }
    }

    public void addTarget3()
    {
        if (state != allStates.PLAYER || !enemyTrue3)
        {
            return;
        }

        if (queuedTargets.Count < queuedAttacks.Count)
        {
            queuedTargets.Add(enemyUnit3);
        }
        else if (queuedTargets.Count == 0)
        {
            return;
        }
        else
        {
            queuedTargets.RemoveAt(queuedTargets.Count - 1);
            queuedTargets.Add(enemyUnit3);
        }
    }

    bool playerAttack(UnitAttributes enemyunit, Attack attack)
    {
        return enemyunit.takeDamage(attack.damage);   
    }

    IEnumerator transition()
    {
        for (int i = 0; i < queuedTargets.Count; i++)
        {
            if (playerAttack(queuedTargets[i], queuedAttacks[i]))
            {
                yield return new WaitForSeconds(.5f);
            }
        }

        if (enemyTrue1)
        {
            enemyTrue1 = enemyUnit1.unitCurrentHealth != 0;
        }
        if (enemyTrue2)
        {
            enemyTrue2 = enemyUnit2.unitCurrentHealth != 0;
        }
        if (enemyTrue3)
        {
            enemyTrue3 = enemyUnit3.unitCurrentHealth != 0;
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

        UpdatePlayerHP();
    }

    void endGame()
    {
        SceneManager.LoadScene(0);
    }

    void progress()
    {
        state = allStates.START;
        Destroy(background);
        
        if (map.tiles[stage].enemyTrue[0] == 1)
        {
            Destroy(enemy1GO);
        }
        if (map.tiles[stage].enemyTrue[1] == 1)
        {
            Destroy(enemy2GO);
        }
        if (map.tiles[stage].enemyTrue[2] == 1)
        {
            Destroy(enemy3GO);
        }

        stage++;

        StartCoroutine(startBattle());
    }
}
