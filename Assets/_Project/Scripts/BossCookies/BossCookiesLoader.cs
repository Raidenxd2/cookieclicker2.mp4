using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class BossCookiesLoader : MonoBehaviour
{
    [SerializeField] private Game game;

    [SerializeField] private AssetReference BossCookiesRef;

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

        AddressableHandles.BossCookiesHandle = Addressables.LoadSceneAsync(BossCookiesRef, LoadSceneMode.Additive);
        await AddressableHandles.BossCookiesHandle;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.bossCookiesRef));

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(ThemeManager.instance.CurrentSceneName));

        await Addressables.UnloadSceneAsync(AddressableHandles.BossCookiesHandle);

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