using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameActionFloat(float value);

public class GameScenesManager : Singleton<GameScenesManager>
{
    public bool SceneLoaded { get; private set; }

    string loadingSceneName = null, currentSceneName = null;
    private bool sceneLoading = false;

    public event GameActionFloat OnSceneLoadingProgressUpdated;

    const string menuSceneName = "Menu", gameSceneName = "Game";

    private void Update()
    {
        if(!sceneLoading && !string.IsNullOrWhiteSpace(currentSceneName) && currentSceneName.Equals(gameSceneName))
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                LoadMenu();
            }
        }
    }

    public void LoadMenu()
    {
        LoadScene(menuSceneName);
    }

    public void LoadGame()
    {
        LoadScene(gameSceneName);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneLoading(sceneName));
    }

    IEnumerator SceneLoading(string sceneName)
    {
        loadingSceneName = sceneName;

        SceneLoaded = false;
        sceneLoading = true;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = true;
        while (asyncOperation.progress < 0.9f)
        {
            SetSceneLoadingProgress(asyncOperation.progress);
            yield return null;
        }

        SetSceneLoadingProgress(1f);
        yield return new WaitForEndOfFrame();

        sceneLoading = false;
        SceneLoaded = true;
        currentSceneName = sceneName;
    }

    void SetSceneLoadingProgress(float v)
    {
        OnSceneLoadingProgressUpdated?.Invoke(v);
    }

}