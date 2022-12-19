using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Logger = LoggerSystem.Logger;
using LoggerSystem;

public class SceneLoader : MonoBehaviour
{

    public Slider slider;
    public TMP_Text progressText;
    public TMP_Text ErrorInfo;
    public GameObject FailedToLoadGameScreen;
    int error;

    public IEnumerator LoadScene(int sceneIndex)
    {
        try
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
        catch (System.Exception ex)
        {
            FailedToLoadGameScreen.SetActive(true);
            ErrorInfo.text = "" + ex;
            Logger.Log("" + ex, LogTypes.Exception);
            error = 1;
        }

        if (error == 1)
        {
            yield return new WaitForSeconds(30);
            Application.Quit();
        }
        
    }

    public void ButtonLoad()
    {
        StartCoroutine(LoadScene(1));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        yield return new WaitForSeconds(0.2f);
        try
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                slider.value = progress;
                progressText.text = progress * 100f + "%";
                Debug.Log(progress);
            }
        }
        catch (System.Exception ex)
        {
            FailedToLoadGameScreen.SetActive(true);
            ErrorInfo.text = "" + ex;
            Logger.Log("" + ex, LogTypes.Exception);
            error = 1;
        }

        if (error == 1)
        {
            yield return new WaitForSeconds(30);
            Application.Quit();
        }
        yield return null;
        

        
    }
}
