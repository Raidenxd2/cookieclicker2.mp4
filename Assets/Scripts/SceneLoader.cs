using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Logger = LoggerSystem.Logger;
using LoggerSystem;
using System;

public class SceneLoader : MonoBehaviour
{

    public Slider slider;
    public TMP_Text progressText;
    public TMP_Text ErrorInfo;
    public GameObject FailedToLoadGameScreen;
    int error;

    public void ButtonLoad()
    {
        SceneManager.LoadScene(1);
    }

    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        yield return new WaitForSeconds(0.2f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            Logger.Log("Progress: " + progress, LogTypes.Normal);
        }
        yield return null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
}
