using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// on the Player
public class UseTheWand : MonoBehaviour
{
    public GameObject wandParticles;
    GameObject myWandParticles;
    public GameObject lightningEffect;
    GameObject myLightningEffect;
    public GameObject circleThing;
    GameObject myCircleThing;

    public Texture2D WAND_PRESSED;
    public Texture2D WAND_NORMAL;

    Vector3 mousePos; // holds the proper mouse coordinates as we will use them in game
    float mouseRadius;
    public LayerMask enemyMask;

    public int wandNum;

    public GameObject manaBar;
    public float playerMana;
    public Sprite NO_MANA;
    public Sprite ONE_MANA;
    public Sprite TWO_MANA;
    public Sprite THREE_MANA;
    public Sprite FOUR_MANA;
    public Sprite FULL_MANA;

    public Text ZERO_WAND_NUM;
    public Text ONE_WAND_NUM;

    public Animator myAnim;

    public GameObject spawnManager;
    SpawnEnemies s;

    // Start is called before the first frame update
    void Start()
    {
        // Convert the mouse coordinates
        Vector3 worldMousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(worldMousePos.x, worldMousePos.y, 10));
        mouseRadius = 0.5f;
        wandNum = 0;
        myAnim = gameObject.GetComponent<Animator>();
        playerMana = 0f;
        s = spawnManager.GetComponent<SpawnEnemies>();
    }

    // Update is called once per frame
    void Update()
    {
        // Continually calculate and update mouse position
        UpdateMousePosition();

        // Check wand num
        UpdateWandNum();

        // Am I pressing the mouse?
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(WAND_PRESSED, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(WAND_NORMAL, Vector2.zero, CursorMode.Auto);
        }

        // Check for overlap cursor with this enemy
        Collider2D hit = Physics2D.OverlapCircle(mousePos, mouseRadius, enemyMask);
        if(hit != null) // If hovering over an enemy within a certain radius
        {
            if(Input.GetMouseButton(0) && wandNum == 0) 
            {
                Cursor.SetCursor(WAND_PRESSED, Vector2.zero, CursorMode.Auto);
                myAnim.SetTrigger("Attacking");

                if (myWandParticles == null)
                {
                    myWandParticles = Instantiate(wandParticles, new Vector3(mousePos.x, mousePos.y - 0.4f, mousePos.z), Quaternion.identity) as GameObject;
                    myWandParticles.GetComponent<ParticleSystem>().Play(true);
                }
                if (myCircleThing == null)
                {
                    myCircleThing = Instantiate(circleThing, new Vector3(mousePos.x, mousePos.y - 0.4f, mousePos.z), Quaternion.identity) as GameObject;
                }

                if(!myWandParticles.GetComponent<ParticleSystem>().isEmitting)
                {
                    myWandParticles.GetComponent<ParticleSystem>().Play(true);
                }

                myWandParticles.transform.position = new Vector3(mousePos.x, mousePos.y - 0.4f, mousePos.z);
                myCircleThing.transform.position = new Vector3(mousePos.x, mousePos.y - 0.4f, mousePos.z);
                hit.gameObject.GetComponent<DealWithDamage>().timeIHaveBeenDrained += Time.deltaTime;
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0) && wandNum == 1 && playerMana > 0)
            {
                Cursor.SetCursor(WAND_PRESSED, Vector2.zero, CursorMode.Auto);
                myAnim.SetTrigger("Attacking");

                // Spell effect
                Vector3 enemyPos = hit.gameObject.transform.position;
                Instantiate(lightningEffect, new Vector3(enemyPos.x, enemyPos.y, enemyPos.z), Quaternion.identity);

                gameObject.GetComponent<AudioSource>().Play();


                // Mana stuff
                SubtractMana();

                // Damage the enemy stuff
                hit.gameObject.GetComponent<DealWithDamage>().alreadyDied = true;
                s.totalNumEnemies--;
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
                }
                hit.gameObject.GetComponent<DealWithDamage>().eAnim.SetTrigger("Die");
            }
            else
            {
                if(myWandParticles != null && myWandParticles.GetComponent<ParticleSystem>().isEmitting) 
                    myWandParticles.GetComponent<ParticleSystem>().Stop();
                Cursor.SetCursor(WAND_NORMAL, Vector2.zero, CursorMode.Auto);
                Destroy(myCircleThing);
            }
        } 
        else
        {
            Destroy(myCircleThing);
            if (myWandParticles != null && myWandParticles.GetComponent<ParticleSystem>().isEmitting)
                myWandParticles.GetComponent<ParticleSystem>().Stop();
        }
    }

    void UpdateMousePosition()
    {
        Vector3 worldMousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(worldMousePos.x, worldMousePos.y, 10));
    }

    void UpdateWandNum()
    {
        if(Input.GetKeyDown("1")) // 1-mana 'spell'
        {
            wandNum = 1;
            ZERO_WAND_NUM.color = new Color(130 / 255f, 129 / 255f, 140 / 255f);
            ONE_WAND_NUM.color = new Color(17 / 255f, 16 / 255f, 2 / 255f);
        } 
        else if (Input.GetKeyDown("0")) // default
        {
            wandNum = 0;
            ZERO_WAND_NUM.color = new Color(17 / 255f, 16 / 255f, 27 / 255f);
            ONE_WAND_NUM.color = new Color(130 / 255f, 129 / 255f, 140 / 255f);
        }
        else
        {
            wandNum = wandNum;
        }
    }

    void SubtractMana()
    {
        if (playerMana == 5)
        {
            playerMana = 4;
            manaBar.GetComponent<Image>().sprite = FOUR_MANA;
        }
        else if (playerMana == 4)
        {
            playerMana = 3;
            manaBar.GetComponent<Image>().sprite = THREE_MANA;
        }
        else if (playerMana == 3)
        {
            playerMana = 2;
            manaBar.GetComponent<Image>().sprite = TWO_MANA;
        }
        else if (playerMana == 2)
        {
            playerMana = 1;
            manaBar.GetComponent<Image>().sprite = ONE_MANA;
        }
        else if (playerMana == 1)
        {
            playerMana = 0;
            manaBar.GetComponent<Image>().sprite = NO_MANA;
        }
        else
        {
            playerMana = 0;
            manaBar.GetComponent<Image>().sprite = NO_MANA;
        }
    }
}
