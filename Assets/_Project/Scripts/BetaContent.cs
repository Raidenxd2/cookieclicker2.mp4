using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.AddressableAssets;
using LoggerSystem;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;

public class BetaContent : MonoBehaviour
{
    public GameObject BetaContentWarning;
    public GameObject EnableCloudSaveScreen;
    public GameObject SideBar;
    public GameObject ModsBTN;
    public GameObject ResearchFactoryButton;

    public Image ProgressBar;
    public TMP_Text progressText;
    public TMP_Text infoText;

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
            if (PlayerPrefs.GetInt("BetaContent", 0) == 1 && scene.name != "Init")
            {
                if (PlayerPrefs.GetInt("BETA_EnableSideBar", 0) == 1)
                {
                    SideBar.SetActive(true);
                }
                if (PlayerPrefs.GetInt("BETA_Mods", 0) == 1)
                {
                    ModsBTN.SetActive(true);
                }
                if (PlayerPrefs.GetInt("BETA_ResearchFactory", 0) == 1)
                {
                    ResearchFactoryButton.SetActive(true);
                }
            }
            if (scene.name != "Game")
            {
                LoadScene();
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText(Application.temporaryCachePath + "/BetaContentInitFail.txt", ex.StackTrace + "\n" + ex.Message);
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (PlayerPrefs.GetInt("EnableCloudSave", 0) == 0)
        {
            EnableCloudSaveScreen.SetActive(true);
            return;
        }

        LoadSceneAsync();
    }

    public void EnableCloudSave()
    {
        PlayerPrefs.SetInt("EnableCloudSave", 1);

        LoadScene();
    }

    public void DisableCloudSave()
    {
        PlayerPrefs.SetInt("EnableCloudSave", 2);

        LoadScene();
    }

    private async void LoadSceneAsync()
    {
        if (PlayerPrefs.GetInt("EnableCloudSave", 0) == 1)
        {
            infoText.text = "Initializing Unity Gaming Services...";
            await UnityServices.InitializeAsync();

            #if UNITY_ANDROID
            infoText.text = "Signing in...";

            try
            {
                #if UNITY_EDITOR
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                #else
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(GooglePlayGamesManager.instance.Token);
                #endif
            }
            catch (Exception ex)
            {
                LogSystem.Log("Failed to sign in.\n" + ex.ToString(), LogTypes.Exception);
            }

            infoText.text = "Checking if user played the game before...";

            if (AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
                    {
                        var hasPlayed_Cloud = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"UserPlayed"});
                        if (hasPlayed_Cloud.TryGetValue("UserPlayed", out var keyName))
                        {
                            LogSystem.Log("UserPlayed (from cloud): " + keyName.Value.GetAs<string>());
                            PlayerPrefs.SetInt("HasPlayed", 1);

                            infoText.text = "Downloading save file...";

                            PlayerPrefs.SetInt("GRAPHICS_PostProcessing", 0);
                            PlayerPrefs.SetInt("GRAPHICS_Lighting", 1);
                            PlayerPrefs.SetInt("GRAPHICS_Particles", 1);
                            PlayerPrefs.SetInt("GRAPHICS_Trees", 1);
                            PlayerPrefs.SetInt("GRAPHICS_VSync", 0);
                            PlayerPrefs.SetInt("GRAPHICS_Fog", 1);
                            PlayerPrefs.SetInt("GRAPHICS_TextureQuality", 0);
                            PlayerPrefs.SetInt("GRAPHICS_Textures", 1);
                            PlayerPrefs.SetInt("GRAPHICS_AO", 0);
                            PlayerPrefs.SetInt("GRAPHICS_HDR", 0);
                            PlayerPrefs.SetFloat("GRAPHICS_RenderQuality", 0.75f);

                            byte[] file = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("cookie2");
                            File.WriteAllBytes(Application.persistentDataPath + "/cookie2", file);
                        }
                        else
                        {
                            LogSystem.Log("UserPlayed doesn't exist.", LogTypes.Warning);
                        }
                    }
                    else if (PlayerPrefs.GetInt("HasPlayed", 0) == 1)
                    {
                        LogSystem.Log("This device has played this game before, setting UserPlayed to 1 on the cloud.");

                        var data = new Dictionary<string, object>{ {"UserPlayed", "1"} };
                        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(data);

                        var metadata = await CloudSaveService.Instance.Files.Player.GetMetadataAsync("cookie2");
                        LogSystem.Log("LastModified (cloud file): " + metadata.Modified.Value.ToString());
                        LogSystem.Log("LastModified (local file): " + File.GetLastWriteTimeUtc(Application.persistentDataPath + "/cookie2").ToString());

                        if (metadata.Modified.Value > File.GetLastWriteTimeUtc(Application.persistentDataPath + "/cookie2"))
                        {
                            infoText.text = "Downloading updated save file...";
                            byte[] cloudFile = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("cookie2");
                            File.WriteAllBytes(Application.persistentDataPath + "/cookie2", cloudFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogSystem.Log("Failed to check if user played the game before.\n" + ex.ToString(), LogTypes.Exception);
                }
            }
            #endif
        }
        

        infoText.text = "Loading Game...";
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        Addressables.UnloadSceneAsync(AddressableHandles.instance.initSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.gameSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.gameSceneRef, LoadSceneMode.Single);

        while (!AddressableHandles.instance.gameSceneHandle.IsDone)
        {
            float progressValue = Mathf.Clamp01(AddressableHandles.instance.gameSceneHandle.PercentComplete / 0.9f);
            ProgressBar.fillAmount = progressValue;
            progressText.text = progressValue * 100f + "%";

            yield return null;
        }
    }
}