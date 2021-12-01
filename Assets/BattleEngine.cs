using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//Enum that holds a bunch of states that lets our program know whether or not it should do a certain action at a certain point in the battle phase.
public enum allStates { START, PLAYER, ENEMY, TRANSITION, RESET, PROGRESS , CHOOSE}

//Engine that basically runs the entire game.
public class BattleEngine : MonoBehaviour
{
    //All these variables control and retrieve individual elements like sprites, text boxes, and other object to use in our code.
    public Text currhp, lvl, luck, moves, message;
    public List<Text> weaponText;
    public List<GameObject> enemyHealth;

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
    public List<GameObject> toggle;
    public GameObject[] toDisable;
    public List<GameObject> enemies;
    public GameObject hero;
    public List<GameObject> buttons;
    public List<GameObject> movement;


    public Transform[] spawns;
    private bool[] enemyTrue;

    private allStates state;

    private UnitAttributes[] characterUnits;

    public String[] poses;

    //This process gets called as soon as the scene that holds BattleEngine is loaded.
    void Start()
    {
        characterGo = new GameObject[4];
        characterUnits = new UnitAttributes[4];
        enemyTrue = new bool[3];
        heroAttacks = new List<Attack>();
        
        //Gives the character (player) three basic attacks with their own respective base stats
        while (heroAttacks.Count < 3)
        {
            heroAttacks.Add(new Attack(heroAttacks.Count + 1));
        }

        //Clears the old map (if there is one) and generates a new map (graph/2d matrix) on a new game with randomized stats
        map = new Map();
        map.randomizeMap();

        //Holds the position values of the current stage in the map
        stagevert = 1;
        stagehorizontal = 0;
        baseAttackCount = 5;

        state = allStates.START;

        //Instantiates the player character using the default player prefab
        characterGo[0] = Instantiate(hero, spawns[1]);
        characterUnits[0] = characterGo[0].GetComponent<UnitAttributes>();

        StartCoroutine(startBattle());
    }

    //Function called when starting a new battle
    IEnumerator startBattle()
    {
        //Updates the message to let the user know what's happening
        message.text = "Starting New Battle...";

        //You get more attacks based on which level you're on (held by stagehorizontal);
        baseAttackCount += stagehorizontal;

        //At the start of each battle your currentAttackCount should be equal to your baseAttackCount
        currentAttackCount = baseAttackCount;

        //Enables buttons
        toDisable[0].SetActive(true);
        toDisable[1].SetActive(true);

        //Sets up the weapon drop array
        weaponDrops = new Attack[3];

        //Instantiates the background based off of what's held in the map's current tile
        background = Instantiate(backgrounds[map.tiles[stagehorizontal, stagevert].background], spawns[0]) ;

        //Instantiates the enemies based off of what's held in the map's current tile
        for (int i = 0; i < enemyTrue.Length; i++)
        {
            buttons[i].SetActive(false);
            movement[i].SetActive(false);
            toggle[i].SetActive(false);
            enemyHealth[i].SetActive(false);
            enemyTrue[i] = map.tiles[stagehorizontal, stagevert].enemyTrue[i] != 0;
            if (map.tiles[stagehorizontal, stagevert].enemyTrue[i] == 1)
            {
                buttons[i].SetActive(true);
                enemyHealth[i].SetActive(true);
                characterGo[i + 1] = Instantiate(enemies[map.tiles[stagehorizontal, stagevert].enemies[i].enemyType], spawns[i + 2]);
                characterUnits[i + 1] = characterGo[i + 1].GetComponent<UnitAttributes>();
                characterUnits[i + 1].setStats(map.tiles[stagehorizontal, stagevert].enemies[i].stats);
                enemyHealth[i].GetComponentInChildren<Text>().text = characterUnits[i + 1].unitCurrentHealth.ToString();
            }
        }

        //Updates the UI for the player's HP
        UpdatePlayerHP();

        //Sets the state equal to the player state
        state = allStates.PLAYER;
   
        //Simple delay function (creates a delay of .5f in this case)
        yield return new WaitForSeconds(.5f);

        //Progresses to the Player's turn
        playerTurn();
    }

    //Function updates the values of the enemies health text in the UI
    void updateEnemy()
    {
        for (int i = 0; i < enemyTrue.Length; i++)
        {
            if (map.tiles[stagehorizontal, stagevert].enemyTrue[i] == 1)
            {
                enemyHealth[i].GetComponentInChildren<Text>().text = characterUnits[i + 1].unitCurrentHealth.ToString();
            }
        }
    }

    //Function updates the values of the player's UI text (misleading since it says HP, it actually updates everything)
    void UpdatePlayerHP()
    {
        currhp.text = characterUnits[0].unitCurrentHealth.ToString();
        luck.text = characterUnits[0].unitLuck.ToString();
        lvl.text = (stagehorizontal + 1).ToString();
        moves.text = currentAttackCount.ToString();
    }

    //Function instantiates the list of attacks and targets and resets the base attack count
    void playerTurn()
    {
        currentAttackCount = baseAttackCount;
        queuedAttacks = new List<Attack>();
        queuedTargets = new List<UnitAttributes>();
        message.text = "Choose Attack:";
    }
    
    //Function checks if the end turn button is pressed and only progresses to the transition state if the state is = to the player state
    public void checkEndTurn()
    {
        if (state != allStates.PLAYER)
        {
            return;
        }

        state = allStates.TRANSITION;
        StartCoroutine(transition());
    }

    //Using the int passed in by the button press, it checks whether or not the attack can be selected and added to the vector of attacks
    public void attackCheck(int i)
    {
        //Prevents users from queuing attacks outside their turn, and only when they have the cost to do so
        if (state != allStates.PLAYER || currentAttackCount < heroAttacks[i].cost)
        {
            return;
        }

        //Using the fact that queuedAttacks will only be up to one more than queuedTargets, we set this in order to check if this is a new attack to be queued or not
        if (queuedAttacks.Count == queuedTargets.Count)
        {
            queuedAttacks.Add(heroAttacks[i]);
        }

        else
        {
            queuedAttacks.RemoveAt(queuedAttacks.Count - 1);
            queuedAttacks.Add(heroAttacks[i]);
        }
    }

    public void addTarget(int i)
    {
        //Prevents users from queuing targets outside their turn, and only when the enemy exists
        if (state != allStates.PLAYER || !enemyTrue[i])
        {
            return;
        }

        //If an attack has been queued already, it'll just add a target for that attack, and if a target has been pressed without an attack change, it'll duplicate the previous attack on a different target
        if (queuedTargets.Count < queuedAttacks.Count)
        {
            queuedTargets.Add(characterUnits[i+1]);
            currentAttackCount -= queuedAttacks[queuedAttacks.Count - 1].cost;
        }
        else if (queuedTargets.Count == 0)
        {
            return;
        }
        else if (currentAttackCount >= queuedAttacks[queuedAttacks.Count - 1].cost)
        {
            queuedAttacks.Add(queuedAttacks[queuedAttacks.Count - 1]);
            currentAttackCount -= queuedAttacks[queuedAttacks.Count - 1].cost;
            queuedTargets.Add(characterUnits[i+1]);
        }
        UpdatePlayerHP();
    }

    //Function called to deal damage to an enemy on the player's transition phase
    bool playerAttack(UnitAttributes enemyunit, Attack attack)
    {
        return enemyunit.takeDamage(attack.damage  * (1 + Convert.ToInt32(UnityEngine.Random.Range(0, 99) < characterUnits[0].unitLuck)));   
    }

    //This is the function where all the queued moves get performed.
    IEnumerator transition()
    {
        message.text = "Executing Attacks...";

        //Outputs the attacks on the given targets and skips over if the target has already died from an attack. Plays the appropriate animations with delays
        for (int i = 0; i < queuedTargets.Count; i++)
        {
            if (playerAttack(queuedTargets[i], queuedAttacks[i]))
            {
                updateEnemy();
                characterUnits[0].GetComponentInChildren<Animator>().Play(poses[queuedAttacks[i].type]);
                if (queuedAttacks[i].type == 2)
                {
                    yield return new WaitForSeconds(1.5f);
                }
                else
                {
                    yield return new WaitForSeconds(.5f);
                }
            }
        }

        //Checks what enemies are dead after the encounter and disables buttons if they died.
        for (int i = 0; i < enemyTrue.Length; i++)
        {
            if (enemyTrue[i])
            {
                enemyTrue[i] = characterUnits[i+1].unitCurrentHealth != 0;
                buttons[i].SetActive(enemyTrue[i]);
            }

        }

        state = allStates.ENEMY;

        //Transitions to the enemyTurn phase
        StartCoroutine(enemyTurn());
    }

    IEnumerator enemyTurn()
    {
        message.text = "Enemies Attacking...";
        
        //All the living enemies attack
        for (int i = 0; i < 3; i++)
        {
            if (enemyTrue[i])
            {
                yield return new WaitForSeconds(.5f);
                StartCoroutine(enemyAttack(characterUnits[i + 1]));
            }
        }

        //If any enemies are still alive we go back to the player's turn, otherwise, we allow the player to progress
        if (enemyTrue[0] || enemyTrue[1] || enemyTrue[2])
        {
            state = allStates.PLAYER;
            playerTurn();
        }
        else
        {
            state = allStates.PROGRESS;
            toDisable[0].SetActive(false);
            toDisable[1].SetActive(false);
            battleWin();
        }
        
    }

    //Enemy attack function that checks if the player is dead, then resets the game if they are
    IEnumerator enemyAttack(UnitAttributes enemy)
    {
        enemy.GetComponentInChildren<Animator>().Play("Attack");
        yield return new WaitForSeconds(.5f);
        characterUnits[0].takeDamage(enemy.unitAttack * (1 + Convert.ToInt32(UnityEngine.Random.Range(0, 99) < enemy.unitLuck)));

        if (characterUnits[0].unitCurrentHealth == 0)
        {
            state = allStates.RESET;
            endGame();
        }

        UpdatePlayerHP();
    }

    //Function to end the game and return to the title screen
    void endGame()
    {
        SceneManager.LoadScene(0);
    }

    //Function that allows the user to progress on the map based on if a tile is adjacent to it while being at a higher horizontal index
    public void progress(int direction)
    {
        if (state != allStates.PROGRESS)
        {
            return;
        }
        
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

    //Function called when the player wins the battle. Instantiates weapon drops and directional arrows.
    void battleWin()
    {
        message.text = "Select Weapon Drop...";
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
            toggle[1].SetActive(true);
            pathFound = true;
        }
        if (stagevert > 0)
        {
            if (map.tileTrue[stagehorizontal + 1, stagevert - 1])
            {
                movement[0].SetActive(true);
                toggle[0].SetActive(true);
                pathFound = true;
            }
        }
        if (stagevert < 2)
        {
            if (map.tileTrue[stagehorizontal + 1, stagevert + 1])
            {
                movement[2].SetActive(true);
                toggle[2].SetActive(true);
                pathFound = true;
            }
        }

        if (!pathFound)
        {
            map.tiles[stagehorizontal + 1, stagevert].randomizeTile(stagehorizontal + 1);
            map.tileTrue[stagehorizontal + 1, stagevert] = true;
            movement[1].SetActive(true);
            toggle[1].SetActive(true);

        }
        state = allStates.CHOOSE;
        for (int i = 0; i < 3; i++)
        {
            if (map.tiles[stagehorizontal, stagevert].enemyTrue[i] == 1)
            {
                weaponDrops[i] = new Attack();
                buttons[i].SetActive(true);
                weaponDrops[i].randomize(UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, stagehorizontal * 5));
                weaponText[i].text = "Slot: " + ((weaponDrops[i].type)).ToString() + "\nDamage: " + weaponDrops[i].damage.ToString() + "\nCost: " + weaponDrops[i].cost.ToString();
            }
        }
    }

    //Function called when pressing a button to take a weapon from the weapon drop pool
    public void takeWeapon(int j)
    {
        if (state == allStates.CHOOSE)
        {
            message.text = "Choose Next Stage:";
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
