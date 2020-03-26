using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// on the Enemy prefab
public class DealWithDamage : MonoBehaviour
{
    public float timeIHaveBeenDrained; // basically the health since there are no super complex spells that require a health tracker variable
    GameObject spawnManager;
    public bool alreadyDied;

    public Animator eAnim;

    Text waveNum;

    SpawnEnemies s;

    GameObject player;
    UseTheWand w;

    // Start is called before the first frame update
    void Start()
    {
        timeIHaveBeenDrained = 0f;
        alreadyDied = false;
        eAnim = gameObject.GetComponent<Animator>();
        spawnManager = GameObject.Find("SpawnManager");
        waveNum = GameObject.Find("WaveText").GetComponent<Text>();
        s = spawnManager.GetComponent<SpawnEnemies>();
        player = GameObject.Find("Player");
        w = player.GetComponent<UseTheWand>();
    }

    // Update is called once per frame
    void Update()
    {
        // Is the enemy dead yet from draining
        if (timeIHaveBeenDrained >= 1)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Animator>().SetTrigger("Die");
            if(!alreadyDied)
            {
                s.totalNumEnemies--;
                AddMana(); // the "drain" effect
                if (s.currentWave == s.maxWaves && s.numHaveSpawned == s.maxNumEnemies && s.totalNumEnemies == 0)
                {
                    s.Victory();
                }

                if (s.totalNumEnemies == 0 
                    && s.numHaveSpawned == s.maxNumEnemies
                    && !s.gameIsOver)
                {
                    s.shouldSpawnMoreThisWave = true;
                    s.numHaveSpawned = 0;
                    s.currentWave++;
                    waveNum.text = "Wave: " + s.currentWave + " / 5";
                }
                alreadyDied = true;
            }
        }

        void AddMana()
        {
            if (w.playerMana == 0)
            {
                w.playerMana = 1;
                w.manaBar.GetComponent<Image>().sprite = w.ONE_MANA;
            }
            else if (w.playerMana == 1)
            {
                w.playerMana = 2;
                w.manaBar.GetComponent<Image>().sprite = w.TWO_MANA;
            }
            else if (w.playerMana == 2)
            {
                w.playerMana = 3;
                w.manaBar.GetComponent<Image>().sprite = w.THREE_MANA;
            }
            else if (w.playerMana == 3)
            {
                w.playerMana = 4;
                w.manaBar.GetComponent<Image>().sprite = w.FOUR_MANA;
            }
            else if (w.playerMana == 4)
            {
                w.playerMana = 5;
                w.manaBar.GetComponent<Image>().sprite = w.FULL_MANA;
            }
            else
            {
                w.playerMana = 5;
                w.manaBar.GetComponent<Image>().sprite = w.FULL_MANA;
            }
        }
    }
}
