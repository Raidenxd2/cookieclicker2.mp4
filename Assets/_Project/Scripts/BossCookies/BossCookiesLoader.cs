using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class BossCookiesLoader : MonoBehaviour
{
    [SerializeField] private Game game;

    [SerializeField] private AssetReference environmentRef;
    private AsyncOperationHandle<SceneInstance> environmentHandle;

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

        await SceneManager.LoadSceneAsync(SceneNames.bossCookiesRef, LoadSceneMode.Additive);

        BeanLogger.Log("Loading Scene " + SceneNames.bossCookiesEnvironmentRef, this);

        environmentHandle = Addressables.LoadSceneAsync(environmentRef, LoadSceneMode.Additive);
        await environmentHandle;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.bossCookiesEnvironmentRef));

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
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            VRCanvas vrCanvas = Notification.instance.NotificationCanvas.GetComponent<VRCanvas>();
            Notification.instance.NotificationCanvas.transform.SetPositionAndRotation(vrCanvas.newPos, Quaternion.Euler(vrCanvas.newRot));
            Notification.instance.NotificationCanvas.transform.parent = notificationCanvasParent;
            
            Game.instance.XROrigin.transform.parent = BossCookiesGame.instance.OldVRParent;
            Game.instance.XROrigin.transform.SetPositionAndRotation(BossCookiesGame.instance.OldVRPosition, BossCookiesGame.instance.OldVRRotation);
        }
#endif

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.gameSceneRef));

        await SceneManager.UnloadSceneAsync(SceneNames.bossCookiesRef);

        BeanLogger.Log("Unloading Scene " + SceneNames.bossCookiesEnvironmentRef, this);

        await Addressables.UnloadSceneAsync(environmentHandle);

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
        
#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            Game.instance.XROrigin.SetActive(true);
        }
#endif
        
        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}