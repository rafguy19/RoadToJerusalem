using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    private string scenestring;
    public GameObject prompt;
    private bool pressed;


    // Start is called before the first frame update
    void Start()
    {
        pressed = false;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        prompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        prompt.SetActive(false);
        pressed = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pressed)
        {
            if (Input.GetKey(KeyCode.E))
            {
                switch (GameManager.instance.scene)
                {
                    case 1:
                        GameManager.instance.scene += 1;

                        gameObject.GetComponent<SceneLoader>().LoadScene("Stage" + GameManager.instance.scene);


                        break;
                    case 2:
                        GameManager.instance.scene += 1;

                        gameObject.GetComponent<SceneLoader>().LoadScene("Stage" + GameManager.instance.scene);
                        break;
                    case 3:
                        GameManager.instance.scene += 1;

                        gameObject.GetComponent<SceneLoader>().LoadScene("Stage" + GameManager.instance.scene);
                        break;
                    case 4:
                        GameManager.instance.scene = 1;
                        gameObject.GetComponent<SceneLoader>().LoadScene("MainMenu");
                        break;
                }
                pressed = true;
            }
        }

    }
}
