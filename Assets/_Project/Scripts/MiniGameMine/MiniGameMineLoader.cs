using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class MiniGameMineLoader : MonoBehaviour
{
    [SerializeField] private Game game;

    [SerializeField] private AssetReference environmentRef;
    private AsyncOperationHandle<SceneInstance> environmentHandle;

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

        game.researchFactory.GameCanvas.SetActive(false);
        game.gameCamera.gameObject.SetActive(false);

        await SceneManager.LoadSceneAsync(SceneNames.miniGameMineRef, LoadSceneMode.Additive);

        BeanLogger.Log("Loading Scene " + SceneNames.miniGameMineEnvironmentRef, this);

        environmentHandle = Addressables.LoadSceneAsync(environmentRef, LoadSceneMode.Additive);
        await environmentHandle;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.miniGameMineEnvironmentRef));

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.gameSceneRef));

        await SceneManager.UnloadSceneAsync(SceneNames.miniGameMineRef);

        BeanLogger.Log("Unloading Scene " + SceneNames.miniGameMineEnvironmentRef, this);

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

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}