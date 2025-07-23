using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class MiniGameMineLoader : MonoBehaviour
{
    [SerializeField] private AssetReference MinigameMineScene;
    private SceneInstance MinigameMineSceneHandle;

    [SerializeField] private GameObject LoadingAsset;
#if !CC2_REMOVE_VR_SUPPORT
    [SerializeField] private GameObject LoadingAssetVR;
#endif

    [SerializeField] private Game game;

    public static MiniGameMineLoader instance;

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

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            LoadingAssetVR.SetActive(true);
        }
        else
        {
#endif
            LoadingAsset.SetActive(true);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

        game.researchFactory.GameCanvas.SetActive(false);
        game.gameCamera.gameObject.SetActive(false);

        MinigameMineSceneHandle = await Addressables.LoadSceneAsync(MinigameMineScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(MinigameMineSceneHandle.Scene);

#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            LoadingAssetVR.SetActive(false);
        }
        else
        {
#endif
            LoadingAsset.SetActive(false);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

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
        if (VRManager.instance.VREnabled)
        {
            Game.instance.XROrigin.transform.parent = MiniGameMine.instance.OldVRParent;
            Game.instance.XROrigin.transform.SetPositionAndRotation(MiniGameMine.instance.OldVRPosition, MiniGameMine.instance.OldVRRotation);
        }
#endif

        SceneManager.SetActiveScene(AddressableHandles.instance.gameSceneHandle.Result.Scene);

        await Addressables.UnloadSceneAsync(MinigameMineSceneHandle);

        game.researchFactory.GameCanvas.SetActive(true);

#if !CC2_REMOVE_VR_SUPPORT
        if (!VRManager.instance.VREnabled)
        {
#endif
            game.gameCamera.gameObject.SetActive(true);
#if !CC2_REMOVE_VR_SUPPORT
        }
#endif

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}