using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemies : MonoBehaviour
{

    public GameObject enemyPrefab;
    float timerVal;
    public int totalNumEnemies;
    float timerLimit;
    public int maxNumEnemies;
    public int numHaveSpawned;

    public int currentWave;
    public bool shouldSpawnMoreThisWave;

    int whereToSpawn;

    public Text waveNum;

    public bool gameIsOver;

    public int maxWaves;

    // Start is called before the first frame update
    void Start()
    {
        timerVal = 0f;
        totalNumEnemies = 0;
        numHaveSpawned = 0;
        timerLimit = 1.5f;
        currentWave = 1;
        shouldSpawnMoreThisWave = true;
        waveNum.text = "Wave: " + currentWave + " / 5";
        gameIsOver = false;
    }

    // Update is called once per frame
    // BASIC SPAWNER
    void Update()
    {
        waveNum.text = "Wave: " + currentWave + " / 5";

        if (currentWave == 2 && numHaveSpawned == maxNumEnemies && totalNumEnemies == 0)
        {
            Victory();
        }

        if(shouldSpawnMoreThisWave)
        {
            maxNumEnemies = DecideNumberOfEnemies();
        }

        // Must check first if we've exceeded the current wave's set max number of enemies
        // If so, spawn no more
        if (numHaveSpawned >= maxNumEnemies)
        {
            shouldSpawnMoreThisWave = false;
        }

        // DON'T SPAWN YET
        // If timer isn't at limit yet (not time to spawn a new enemy)
        if (timerVal < timerLimit)
        {
            timerVal += Time.deltaTime;
        } 
        // TIME TO SPAWN
        // If timer is at or has exceeded limit and the max enemies has never been reached for this wave yet, spawn them
        else if (timerVal >= timerLimit && shouldSpawnMoreThisWave)
        {
            GameObject e = Instantiate(enemyPrefab);

            e.transform.position = DecideWheretoSpawn();

            totalNumEnemies += 1;
            numHaveSpawned++;
            timerVal = 0f;
        } 
    }

    Vector3 DecideWheretoSpawn()
    {
        whereToSpawn = Random.Range(1, 4); // Random.Range with integers is exclusive-max
        Vector3 spawnLocation;

        if(whereToSpawn == 1)
        {
            spawnLocation = new Vector3(0f, -5.5f, 0f); // Bottom tunnel
        } 
        else if(whereToSpawn == 2)
        {
            spawnLocation = new Vector3(-8.5f, -1.3f, 0f); // Left tunnel
        }
        else 
        {
            spawnLocation = new Vector3(8.5f, -1.3f, 0f); // Right tunnel
        }

        return spawnLocation;
    }

    int DecideNumberOfEnemies()
    {
        if(currentWave == 1)
        {
            timerLimit = 1.5f;
            return 5;
        }

        else if(currentWave == 2)
        {
            timerLimit = 1.3f;
            return 8;
        }

        else if(currentWave == 3)
        {
            timerLimit = 1.3f;
            return 13;
        }
        else if(currentWave == 4)
        {
            timerLimit = 1.1f;
            return 21;
        }
        else if(currentWave == 5)
        {
            timerLimit = 1.1f;
            return 26;
        } 
        else
        {
            return 0;
        }
    }

    // What happens when the game ends well
    public void Victory()
    {
        shouldSpawnMoreThisWave = false;
        gameIsOver = true;

        // Deal with player and enemies
        GameObject.Find("Player").GetComponent<FloatThePlayer>().speedOfFloating = 0f; // stop player from moving any more
        GameObject.Find("Player").GetComponent<UseTheWand>().myAnim.SetTrigger("Yay");
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].GetComponent<MoveToPlayer>().spd = 0f; // stop enemies from moving
            allEnemies[i].GetComponent<Animator>().SetTrigger("Idle"); // make their animation stationary/idle
        }

        // Deal with display/UI
        GameObject.Find("GameStatusText").GetComponent<Text>().text = "You did it!";
        GameObject.Find("DayText").GetComponent<Text>().text = "Chik had a good day.";
    }
}
