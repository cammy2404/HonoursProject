using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LoadingScreen[] loadingScreen;
    int loadingScreenIndex = 0;
    //public Transform loadingScreen;
    //public Image progressBar;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalSceneProgress;

    private void Awake()
    {
        instance = this;

        LoadHome(true);
    }

    public void LoadHome(bool initialLoad = false)
    {
        SetLoadingScreenIndex((int)SceneIndexes.HOME);

        loadingScreen[loadingScreenIndex].screenPrefab.gameObject.SetActive(true);

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != null && scene.buildIndex != (int)SceneIndexes.HOME && scene.buildIndex != (int)SceneIndexes.MANAGER)
                scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));

        }

        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.HOME, LoadSceneMode.Additive));

        if (!initialLoad)
            StartCoroutine(GetSceneLoadProgress());
        else
            loadingScreen[loadingScreenIndex].screenPrefab.gameObject.SetActive(false);
    }

    public void LoadGame(int sceneIndex)
    {
        SetLoadingScreenIndex(sceneIndex);

        loadingScreen[loadingScreenIndex].screenPrefab.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.HOME));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress(sceneIndex));
    }

    public IEnumerator GetSceneLoadProgress(int activeScene = -1)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation op in scenesLoading)
                {
                    totalSceneProgress += op.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                UpdateProgressBar(totalSceneProgress, activeScene);

                yield return null;
            }
        }

        if (activeScene != -1)
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(activeScene));

        yield return new WaitForSeconds(1f);

        loadingScreen[loadingScreenIndex].screenPrefab.gameObject.SetActive(false);
    }

    void UpdateProgressBar(float progress, int sceneIndex)
    {
        float width = progress * 10f;

        if (sceneIndex != -1)
            loadingScreen[loadingScreenIndex].progressBar.rectTransform.sizeDelta = new Vector2(width, loadingScreen[loadingScreenIndex].progressBar.rectTransform.sizeDelta.y);
    }

    void SetLoadingScreenIndex(int sceneIndex)
    {
        for (int i = 0; i < loadingScreen.Length; i++)
        {
            if ((int)loadingScreen[i].scene == sceneIndex)
            {
                loadingScreenIndex = i;
                break;
            }
        }
    }


    [System.Serializable]
    public class LoadingScreen
    {
        public SceneIndexes scene;
        public Transform screenPrefab;
        public Image progressBar;
    }
}