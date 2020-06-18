using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneToLoadIndex = 0;
    [SerializeField] private GameObject loadingPanel = null;
    [SerializeField] private Slider progressBar = null;

    private void Awake()
    {
        if(loadingPanel)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadSceneAsync(sceneToLoadIndex));
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        if(loadingPanel)
        {
            loadingPanel.SetActive(true);
        }

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            if(progressBar)
            {
                progressBar.value = progress;
            }

            yield return null;
        }
    }
}
