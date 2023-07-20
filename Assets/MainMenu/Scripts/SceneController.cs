using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : PersistentSingleton<SceneController>
{
    public enum SceneType
    {
        GAME,
        MENU
    }

    private Animator transition;

    private void Awake()
    {
        base.Awake();
        transition = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene(SceneType.MENU);
        }
    }

    public void LoadScene(SceneType scene, float waitTime = 0f)
    {
        int sceneIndex = -1;

        switch (scene)
        {
            case SceneType.MENU:
                sceneIndex = 0;
                break;
            case SceneType.GAME:
                sceneIndex = 1;
                break;
        }

        StartCoroutine(LoadAsyncScene(sceneIndex, waitTime));
    }

    IEnumerator LoadAsyncScene(int sceneIndex, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        transition.SetTrigger("Start");
        //AudioController.Instance.Play("Transition");

        yield return new WaitForSeconds(1f);

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex);

        while (!loading.isDone)
        {
            yield return null;
        }
    }
}
