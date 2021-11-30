using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum allStates { START, PLAYER, ENEMY, TRANSITION, RESET, PROGRESS , CHOOSE}

public class BattleEngine : MonoBehaviour
{
    public Text currhp, lvl, luck, moves;
    public List<Text> weaponText;

    private GameObject background;
    private GameObject[] characterGo;
    
    private int baseAttackCount, currentAttackCount, stagevert, currentIndex, stagehorizontal;
    public int target;

    private Map map;

    private List<Attack> heroAttacks;
    private List<UnitAttributes> queuedTargets;
    private List<Attack> queuedAttacks;
    private Attack[] weaponDrops;

    public List<GameObject> backgrounds;
    public List<GameObject> enemies;
    public GameObject hero;
    public List<GameObject> buttons;
    public List<GameObject> movement;


    public Transform[] spawns;
    private bool[] enemyTrue;

    private allStates state;

    private UnitAttributes[] characterUnits;

    public String[] poses;


    void Start()
    {
        characterGo = new GameObject[4];
        characterUnits = new UnitAttributes[4];
        enemyTrue = new bool[3];
        heroAttacks = new List<Attack>();
        
        while (heroAttacks.Count < 3)
        {
            heroAttacks.Add(new Attack(heroAttacks.Count + 1));
        }

        map = new Map();
        map.randomizeMap();
        stagevert = 1;
        stagehorizontal = 0;
        baseAttackCount = 5;
        state = allStates.START;

        characterGo[0] = Instantiate(hero, spawns[1]);
        characterUnits[0] = characterGo[0].GetComponent<UnitAttributes>();

        StartCoroutine(startBattle());
    }

    IEnumerator startBattle()
    {
        weaponDrops = new Attack[3];
        background = Instantiate(backgrounds[map.tiles[stagehorizontal, stagevert].background], spawns[0]) ;
        for (int i = 0; i < enemyTrue.Length; i++)
        {
            buttons[i].SetActive(false);
            movement[i].SetActive(false);
            enemyTrue[i] = map.tiles[stagehorizontal, stagevert].enemyTrue[i] != 0;
            if (map.tiles[stagehorizontal, stagevert].enemyTrue[i] == 1)
            {
                buttons[i].SetActive(true);

                characterGo[i + 1] = Instantiate(enemies[map.tiles[stagehorizontal, stagevert].enemies[i].enemyType], spawns[i + 2]);
                characterUnits[i + 1] = characterGo[i + 1].GetComponent<UnitAttributes>();
                characterUnits[i + 1].setStats(map.tiles[stagehorizontal, stagevert].enemies[i].stats);
            }
        }

        UpdatePlayerHP();

        state = allStates.PLAYER;
   
        yield return new WaitForSeconds(.5f);
        playerTurn();
    }

    void UpdatePlayerHP()
    {
        currhp.text = characterUnits[0].unitCurrentHealth.ToString() + "/" + characterUnits[0].unitMaxHealth.ToString();
        luck.text = characterUnits[0].unitLuck.ToString();
        lvl.text = (stagehorizontal + 1).ToString();
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

    public void attackCheck(int i)
    {
        if (state != allStates.PLAYER || currentAttackCount < heroAttacks[i].cost)
        {
            return;
        }

        if (queuedAttacks.Count == queuedTargets.Count)
        {
            queuedAttacks.Add(heroAttacks[i]);
            currentAttackCount -= heroAttacks[i].cost;
        }

        else
        {
            currentAttackCount += queuedAttacks[queuedAttacks.Count - 1].cost;
            queuedAttacks.RemoveAt(queuedAttacks.Count - 1);
            queuedAttacks.Add(heroAttacks[i]);
            currentAttackCount -= heroAttacks[i].cost;
        }
    }

    public void addTarget(int i)
    {
        if (state != allStates.PLAYER || !enemyTrue[i])
        {
            return;
        }

        if (queuedTargets.Count < queuedAttacks.Count)
        {
            queuedTargets.Add(characterUnits[i+1]);
        }
        else if (queuedTargets.Count == 0)
        {
            return;
        }
        else
        {
            queuedAttacks.Add(queuedAttacks[queuedAttacks.Count - 1]);
            currentAttackCount += queuedAttacks[queuedAttacks.Count - 1].cost;
            queuedTargets.Add(characterUnits[i+1]);
        }
    }


    bool playerAttack(UnitAttributes enemyunit, Attack attack)
    {
        return enemyunit.takeDamage(attack.damage  * (1+ Convert.ToInt32((UnityEngine.Random.Range(0, 99) < characterUnits[0].unitLuck))));   
    }

    IEnumerator transition()
    {
        for (int i = 0; i < queuedTargets.Count; i++)
        {
            if (playerAttack(queuedTargets[i], queuedAttacks[i]))
            {
                characterUnits[0].GetComponentInChildren<Animator>().Play(poses[queuedAttacks[i].type]);
                yield return new WaitForSeconds(.5f);
            }
        }

        for (int i = 0; i < enemyTrue.Length; i++)
        {
            if (enemyTrue[i])
            {
                enemyTrue[i] = characterUnits[i+1].unitCurrentHealth != 0;
                buttons[i].SetActive(enemyTrue[i]);
            }

        }

        state = allStates.ENEMY;
        StartCoroutine(enemyTurn());
    }

    IEnumerator enemyTurn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (enemyTrue[i])
            {
                yield return new WaitForSeconds(.5f);
                StartCoroutine(enemyAttack(characterUnits[i + 1]));
            }
        }

        if (enemyTrue[0] || enemyTrue[1] || enemyTrue[2])
        {
            state = allStates.PLAYER;
            playerTurn();
        }

        else
        {
            state = allStates.PROGRESS;
            battleWin();
        }
        
    }

    IEnumerator enemyAttack(UnitAttributes enemy)
    {
        enemy.GetComponentInChildren<Animator>().Play("Attack");
        yield return new WaitForSeconds(.5f);
        characterUnits[0].takeDamage(enemy.unitAttack);

        if (characterUnits[0].unitCurrentHealth == 0)
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

    public void progress(int direction)
    {
        
        state = allStates.START;
        Destroy(background);
        for (int i = 1; i < characterGo.Length; i++)
        {
            Destroy(characterGo[i]);
        }
        stagehorizontal++;
        stagevert += direction;
        StartCoroutine(startBattle());
    }

    void battleWin()
    {
        bool pathFound = false;

        if (stagehorizontal == 4)
        {
            stagehorizontal = 0;
            SceneManager.LoadScene(0);
            return;
        }
        if (map.tileTrue[stagehorizontal + 1, stagevert])
        {
            movement[1].SetActive(true);
            pathFound = true;
        }
        if (stagevert > 0)
        {
            if (map.tileTrue[stagehorizontal + 1, stagevert - 1])
            {
                movement[0].SetActive(true);
                pathFound = true;
            }
        }
        if (stagevert < 2)
        {
            if (map.tileTrue[stagehorizontal + 1, stagevert + 1])
            {
                movement[2].SetActive(true);
                pathFound = true;
            }
        }

        if (!pathFound)
        {
            map.tiles[stagehorizontal + 1, stagevert].randomizeTile(stagehorizontal + 1);
            map.tileTrue[stagehorizontal + 1, stagevert] = true;
            movement[1].SetActive(true);

        }
        state = allStates.CHOOSE;
        for (int i = 0; i < 3; i++)
        {
            if (map.tiles[stagehorizontal, stagevert].enemyTrue[i] == 1)
            {
                weaponDrops[i] = new Attack();
                buttons[i].SetActive(true);
                weaponDrops[i].randomize(UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, stagehorizontal * 5));
                weaponText[i].text = "Weapon: " + ((weaponDrops[i].type)).ToString() + "\nDamage:" + weaponDrops[i].damage.ToString() + "\nCost: " + weaponDrops[i].cost.ToString();
            }
        }
    }

    public void takeWeapon(int j)
    {
        if (state == allStates.CHOOSE)
        {
            heroAttacks[weaponDrops[j].type - 1] = weaponDrops[j];
        }
        else
        {
            return;
        }
        for (int i = 0; i < 3; i++)
        {
            weaponText[i].text = "";
        }
        state = allStates.PROGRESS;
    }
}
