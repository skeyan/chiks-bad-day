using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public float spd;
    Vector3 playerPos;

    SpriteRenderer sr;
    GameObject player;

    public LayerMask wallMask;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = new Vector3(0f, 3f, 0f);
        spd = 1.25f;
        sr = gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player"); // prefab cannot reference game object in scene

    }

    // Update is called once per frame
    void Update()
    {
        // REALLY BUDGET ATTEMPT AT A.I. NAVIGATION
        // Raycast to attempt to find walls
        // RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector3.left, 0.3f, wallMask);
        // RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector3.right, 0.3f, wallMask);

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, wallMask);

        //if (hitLeft.collider != null || hitRight.collider != null)
        if (hit != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.forward, spd * Time.deltaTime); // avoid walking on top of walls
        }
        else
        {
            // Move directly toward player
            transform.position = Vector3.MoveTowards(transform.position, playerPos, spd * Time.deltaTime);
        }

        // Flip to always face player 
        // (flip horizontally if enemy's position is greater than the player's)
        // We have this condition because the sprite default face is right.
        sr.flipX = player.transform.position.x < transform.position.x;
    }
}
