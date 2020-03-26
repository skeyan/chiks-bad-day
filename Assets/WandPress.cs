using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandPress : MonoBehaviour
{


    public Texture2D WAND_PRESSED;
    public Texture2D WAND_NORMAL;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(WAND_PRESSED, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(WAND_NORMAL, Vector2.zero, CursorMode.Auto);
        }
    }
}
