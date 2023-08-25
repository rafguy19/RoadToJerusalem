using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;
    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (op.isDone == false)
        {
            yield return null;
        }
    }
}
