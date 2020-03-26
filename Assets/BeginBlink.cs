using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginBlink : MonoBehaviour
{
    float textBlinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        textBlinkTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Text blinking
        Flicker();

        // Load scene
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoadMainScene();
        }
    }

    void Flicker()
    {
        textBlinkTimer += Time.deltaTime;
        if (textBlinkTimer >= 0.9f) // every so often
        {
            if (gameObject.GetComponent<Text>().enabled == true)
            {
                gameObject.GetComponent<Text>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Text>().enabled = true;
            }
            textBlinkTimer = 0f;
        }
    }

    void LoadMainScene()
    {
        gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MyFirstScene", LoadSceneMode.Single);
    }
}
