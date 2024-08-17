using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.AddressableAssets;
using LoggerSystem;
using System;

public class BetaContent : MonoBehaviour
{
    public GameObject BetaContentWarning;
    public GameObject EnableCloudSaveScreen;
    public GameObject GPGSignInScreen;
    public GameObject SideBar;
    public GameObject ModsBTN;
    public GameObject ResearchFactoryButton;
    public GameObject ErrorScreen;

    public TMP_Text ErrorText;

    public Image ProgressBar;
    public TMP_Text progressText;
    public TMP_Text infoText;

    public static BetaContent instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            LogSystem.Log(PlayerPrefs.GetInt("HasPlayed", 0).ToString());
            if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
            {
                PlayerPrefs.SetInt("BetaContent", 0);
                PlayerPrefs.SetInt("BETA_EnableSideBar", 0);
                PlayerPrefs.SetInt("BETA_Mods", 0);
                PlayerPrefs.SetInt("BETA_FPSLIMIT", 0);
                PlayerPrefs.SetInt("BETA_CookieMonster", 0);
                PlayerPrefs.SetInt("BETA_ResearchFactory", 0);
                PlayerPrefs.Save();
            }
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Init" && PlayerPrefs.GetInt("BetaContent", 0) == 1)
            {
                BetaContentWarning.SetActive(true);
            }
            UpdateBetaContent();
            if (scene.name != "Game")
            {
                LoadScene();
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText(Application.temporaryCachePath + "/BetaContentInitFail.txt", ex.ToString());
            LoadScene();
        }
    }

    public void UpdateBetaContent()
    {
        if (PlayerPrefs.GetInt("BetaContent", 0) == 1 && SceneManager.GetActiveScene().name != "Init")
        {
            if (PlayerPrefs.GetInt("BETA_EnableSideBar", 0) == 1)
            {
                SideBar.SetActive(true);
            }
            else
            {
                SideBar.SetActive(false);
            }
            if (PlayerPrefs.GetInt("BETA_ResearchFactory", 0) == 1)
            {
                ResearchFactoryButton.SetActive(true);
            }
            else
            {
                ResearchFactoryButton.SetActive(false);
            }
        }
    }

    public void LoadScene()
    {
        LoadSceneAsync();
    }

    private void LoadSceneAsync()
    {
        infoText.text = "Loading Game...";

        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        Addressables.UnloadSceneAsync(AddressableHandles.instance.initSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.gameSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.gameSceneRef, LoadSceneMode.Single);

        if (AddressableHandles.instance.gameSceneHandle.OperationException != null)
        {
            ErrorScreen.SetActive(true);
            ErrorText.text = "Game failed to load\n" + AddressableHandles.instance.gameSceneHandle.OperationException.ToString();
        }

        while (!AddressableHandles.instance.gameSceneHandle.IsDone)
        {
            float progressValue = Mathf.Clamp01(AddressableHandles.instance.gameSceneHandle.PercentComplete);
            ProgressBar.fillAmount = progressValue;
            progressText.text = progressValue * 100f + "%";

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}