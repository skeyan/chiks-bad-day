using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // mouse vars
    // public float speed;
    // Vector3 mousePos;

    // sprite vars
    SpriteRenderer sr;
    float playerHeight;
    float playerWidth;

    // handle collision vars
    public LayerMask doNotPassLayers;
    Collider2D currentCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.visible = false;
        sr = gameObject.GetComponent<SpriteRenderer>();
        playerHeight = sr.bounds.size.y;
        playerWidth = sr.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();

        // only if our mouse is not colliding with the environment, get the player to follow the mouse
        if(currentCollider == null)
        {
            // move the player to the mouse coords
            Vector3 worldMouse = MousePositionInWorld();
            transform.position = new Vector3(worldMouse.x - playerWidth/2, worldMouse.y - playerHeight/2, 10);
            // transform.position = Vector2.MoveTowards(transform.position, mouseOnScreen, speed * Time.deltaTime);
        }

    }

    Vector3 MousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }

    void CheckCollision()
    {
        // Find any collider in the right layer overlapping our mouse
        currentCollider = Physics2D.OverlapPoint(MousePositionInWorld(), doNotPassLayers);
        if(currentCollider == null) {
            Debug.Log("am NOT colliding");
            return;
        }
        else {
            Debug.Log("am colliding");
        }

    }
}
