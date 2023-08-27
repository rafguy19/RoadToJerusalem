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
        player = GameObject.FindGameObjectWithTag("PlayerParent").transform;
        player.position = new Vector3(-195 ,66, 0);

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
