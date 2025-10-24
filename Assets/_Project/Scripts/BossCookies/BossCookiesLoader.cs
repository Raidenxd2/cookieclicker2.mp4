using com.raiden.assetbundleassetreference.Runtime;
using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class BossCookiesLoader : MonoBehaviour
{
    [SerializeField] private Game game;

    [SerializeField] private AssetBundleAssetReference environmentRef;
    private AssetBundle environmentBundle;

#if UNITY_EDITOR
    [SerializeField] private bool LoadAssetBundlesInEditor;
    [SerializeField] private string SceneFullAssetPath;
#endif

    [SerializeField] private Transform notificationCanvasParent;

    [SerializeField] private Canvas notificationCanvas;

    public static BossCookiesLoader instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void LoadMinigameMine()
    {
        LoadMinigameMineAsync().Forget();
    }

    private async UniTaskVoid LoadMinigameMineAsync()
    {
        game.Fade.Play("FadeIn");
        game.FadeCanvasGroup.blocksRaycasts = true;
        await UniTask.WaitForSeconds(1);

        if (game.ad.PostProcessing)
        {
            game.ad.pp_normal.SetActive(false);
        }

        game.researchFactory.GameCanvas.SetActive(false);
        game.gameCamera.gameObject.SetActive(false);

        await SceneManager.LoadSceneAsync(AddressableHandles.bossCookiesRef, LoadSceneMode.Additive);

        LogSystem.Log("Loading AssetBundle " + environmentRef.BundleName + " and scene " + AddressableHandles.bossCookiesEnvironmentRef);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
#endif
#if UNITY_WEBGL
            environmentBundle = DownloadHandlerAssetBundle.GetContent(await UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/Bundles/" + environmentRef.BundleName + ".bundle").SendWebRequest());
#else
            environmentBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + environmentRef.BundleName + ".bundle");
#endif
            await SceneManager.LoadSceneAsync(AddressableHandles.bossCookiesEnvironmentRef, LoadSceneMode.Additive);
#if UNITY_EDITOR
        }
        else
        {
            await EditorSceneManager.LoadSceneAsyncInPlayMode(SceneFullAssetPath, new(LoadSceneMode.Additive));
        }
#endif
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.bossCookiesEnvironmentRef));

        Notification.instance.NotificationCanvas.worldCamera = BossCookiesGame.instance.Camera.GetComponent<Camera>();

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }

    public void UnloadMinigameMine()
    {
        UnloadMinigameMineAsync().Forget();
    }

    private async UniTaskVoid UnloadMinigameMineAsync()
    {
        game.Fade.Play("FadeIn");
        game.FadeCanvasGroup.blocksRaycasts = true;
        await UniTask.WaitForSeconds(1);

#if !CC2_REMOVE_VR_SUPPORT
        if (!VRManager.instance.VREnabled)
        {
#endif
            Notification.instance.NotificationCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            Notification.instance.NotificationCanvas.transform.parent = notificationCanvasParent;
            Notification.instance.NotificationCanvas.worldCamera = game.gameCamera;
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            VRCanvas vrCanvas = Notification.instance.NotificationCanvas.GetComponent<VRCanvas>();
            Notification.instance.NotificationCanvas.transform.SetPositionAndRotation(vrCanvas.newPos, Quaternion.Euler(vrCanvas.newRot));
            Notification.instance.NotificationCanvas.transform.parent = notificationCanvasParent;
        }
#endif

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            Game.instance.XROrigin.transform.parent = BossCookiesGame.instance.OldVRParent;
            Game.instance.XROrigin.transform.SetPositionAndRotation(BossCookiesGame.instance.OldVRPosition, BossCookiesGame.instance.OldVRRotation);
        }
#endif

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.gameSceneRef));

        await SceneManager.UnloadSceneAsync(AddressableHandles.bossCookiesRef);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
            LogSystem.Log("Unloading Scene " + AddressableHandles.bossCookiesEnvironmentRef);
        }
        else
        {
#endif
            LogSystem.Log("Unloading Scene " + AddressableHandles.bossCookiesEnvironmentRef + " and bundle " + environmentRef.BundleName);
#if UNITY_EDITOR
        }
#endif

        await SceneManager.UnloadSceneAsync(AddressableHandles.bossCookiesEnvironmentRef);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
#endif
            await environmentBundle.UnloadAsync(true);
#if UNITY_EDITOR
        }
#endif

        game.researchFactory.GameCanvas.SetActive(true);

#if !CC2_REMOVE_VR_SUPPORT
        if (!VRManager.instance.VREnabled)
        {
#endif
            game.gameCamera.gameObject.SetActive(true);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

        if (game.ad.PostProcessing)
        {
            game.ad.pp_normal.SetActive(true);
        }

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}