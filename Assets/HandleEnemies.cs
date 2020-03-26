using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleEnemies : MonoBehaviour
{
    public GameObject spawnManager;
    SpawnEnemies h;
    public float playerHealth;

    Animator myAnim;

    public GameObject healthBar;

    public Sprite FULL_HP;
    public Sprite TWO_HP;
    public Sprite ONE_HP;
    public Sprite NO_HP;

    public Text waveNum;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 3;
        h = spawnManager.GetComponent<SpawnEnemies>();
        myAnim = gameObject.GetComponent<Animator>();

        GameObject.Find("GameStatusText").GetComponent<Text>().text = "";
        GameObject.Find("DayText").GetComponent<Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0) // dead/gameover
        {
            healthBar.GetComponent<Image>().sprite = NO_HP;
            myAnim.SetTrigger("Die");
            GameOver();
        }
    }

    // BASIC TAKE-DAMAGE SYSTEM
    // When an enemy touches the player, decrease health and keep tabs with SpawnManager
    void OnCollisionEnter2D(Collision2D col) // requires at least one Rigidbody2D
    { 
        GameObject other = col.gameObject;

        if (other.gameObject.tag.Equals("Enemy"))
        {
            // Deal with player stuff
            playerHealth--;
            if(playerHealth == 3) // full health
            {
                healthBar.GetComponent<Image>().sprite = FULL_HP;
            }
            if(playerHealth == 2)
            {
                healthBar.GetComponent<Image>().sprite = TWO_HP;
            }
            if(playerHealth == 1)
            {
                healthBar.GetComponent<Image>().sprite = ONE_HP;
            }
            if(playerHealth <= 0) // dead/gameover
            {
                healthBar.GetComponent<Image>().sprite = NO_HP;
                myAnim.SetTrigger("Die");
                GameOver();
            }

            // Deal with enemy stuff
            h.totalNumEnemies--; // decrease the variable keeping track of the total number of enemies
            myAnim.SetTrigger("Hurt");

            if(h.totalNumEnemies == 0)
            {
                h.currentWave++;
                h.numHaveSpawned = 0;
                h.shouldSpawnMoreThisWave = true;
                waveNum.text = "Wave: " + h.currentWave + " / 5";
            }

            // Destroy enemy (as though enemy had 1hp)
            other.GetComponent<Collider2D>().enabled = false;
            other.GetComponent<Animator>().SetTrigger("Die");
        }
    }

    // What happens when the game ends poorly
    void GameOver()
    {
        // Deal with player and enemies
        gameObject.GetComponent<FloatThePlayer>().speedOfFloating = 0f; // stop player from moving any more
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < allEnemies.Length; i++)
        {
            allEnemies[i].GetComponent<MoveToPlayer>().spd = 0f; // stop enemies from moving
            allEnemies[i].GetComponent<Animator>().SetTrigger("Idle"); // make their animation stationary/idle
        }

        // Deal with display/UI
        GameObject.Find("GameStatusText").GetComponent<Text>().text = "Too bad!";
        GameObject.Find("DayText").GetComponent<Text>().text = "Chik had a bad day.";
    }
}
