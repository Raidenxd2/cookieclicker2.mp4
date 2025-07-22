using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.AddressableAssets;
using System;
#if !CC2_REMOVE_VR_SUPPORT
using Cysharp.Threading.Tasks;
#endif
#if UNITY_ANDROID
using UnityEngine.Rendering.Universal;
#endif

public class BetaContent : MonoBehaviour
{
    public GameObject ResearchFactoryButton;

    public Image ProgressBar;
    public TMP_Text progressText;
    public TMP_Text infoText;

    public static BetaContent instance;

    [SerializeField] private AssetReference VRPrefab;
    private GameObject VRPrefabGO;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("unity.player_session_count", 0);
        PlayerPrefs.SetInt("unity.player_sessionid", 0);
        PlayerPrefs.SetInt("unity.cloud_userid", 0);
        PlayerPrefs.Save();

#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.IsMobileVR)
        {
            UniversalRenderPipelineAsset urp = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
            urp.msaaSampleCount = 4;
        }
#endif

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LoadVRData();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
#endif

        try
        {
            if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
            {
                PlayerPrefs.SetInt("BetaContent", 0);
                PlayerPrefs.SetInt("BETA_ResearchFactory", 0);
                PlayerPrefs.Save();
            }
            Scene scene = SceneManager.GetActiveScene();
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

#if !CC2_REMOVE_VR_SUPPORT
    private async UniTask LoadVRData()
    {
        VRPrefabGO = Instantiate(await Addressables.LoadAssetAsync<GameObject>(VRPrefab));
    }
#endif

    public void UpdateBetaContent()
    {
        if (PlayerPrefs.GetInt("BetaContent", 0) == 1 && SceneManager.GetActiveScene().name != "Init")
        {
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
        infoText.text = "Loading Game...";

        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        Addressables.UnloadSceneAsync(AddressableHandles.instance.initSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.gameSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.gameSceneRef, LoadSceneMode.Single);

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