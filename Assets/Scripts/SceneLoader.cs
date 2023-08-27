using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;


    private void Start()
    {

    }
    // Start is called before the first frame update
    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {

        if(transition != null)
            transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        Transform player;
        Transform playerSprite;
        player = GameObject.FindGameObjectWithTag("PlayerParent").transform;
        playerSprite = GameObject.FindGameObjectWithTag("Player").transform;

        if (GameManager.instance.scene == 2)
        {
            playerSprite.localPosition = new Vector3(0, 0, 0);
            player.localPosition = new Vector3(-194 ,61, 0);
        }
        else if (GameManager.instance.scene == 3)
        {
            playerSprite.localPosition = new Vector3(0, 0, 0);
            player.localPosition = new Vector3(537, -251, 0);
        }
        else if (GameManager.instance.scene == 4)
        {
            playerSprite.localPosition = new Vector3(0, 0, 0);
            player.localPosition = new Vector3(-180, -254, 0);
        }

        if (transition != null)
        transition.SetTrigger("end");

        while (op.isDone == false)
        {
            yield return null;
        }
    }
    IEnumerator LoadGame()
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);
        GameManager.instance.scene = 1;
        AsyncOperation op = SceneManager.LoadSceneAsync("Stage1");
        while (op.isDone == false)
        {
            yield return null;
        }
   
    }
}
