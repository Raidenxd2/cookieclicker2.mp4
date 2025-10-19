using com.raiden.assetbundleassetreference.Runtime;
using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class MiniGameMineLoader : MonoBehaviour
{
    [SerializeField] private Game game;

    [SerializeField] private AssetBundleAssetReference environmentRef;
    private AssetBundle environmentBundle;

#if UNITY_EDITOR
    [SerializeField] private bool LoadAssetBundlesInEditor;
    [SerializeField] private string SceneFullAssetPath;
#endif

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

        await SceneManager.LoadSceneAsync(AddressableHandles.miniGameMineRef, LoadSceneMode.Additive);

        LogSystem.Log("Loading AssetBundle " + environmentRef.BundleName + " and scene " + AddressableHandles.miniGameMineEnvironmentRef);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
#endif
            environmentBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + environmentRef.BundleName);
            await SceneManager.LoadSceneAsync(AddressableHandles.miniGameMineEnvironmentRef, LoadSceneMode.Additive);
#if UNITY_EDITOR
        }
        else
        {
            await EditorSceneManager.LoadSceneAsyncInPlayMode(SceneFullAssetPath, new(LoadSceneMode.Additive));
        }
#endif

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.miniGameMineEnvironmentRef));

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

        if (VRManager.instance.VREnabled)
        {
            // Game.instance.XROrigin.transform.parent = MiniGameMine.instance.OldVRParent;
            // Game.instance.XROrigin.transform.SetPositionAndRotation(MiniGameMine.instance.OldVRPosition, MiniGameMine.instance.OldVRRotation);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.gameSceneRef));

        await SceneManager.UnloadSceneAsync(AddressableHandles.miniGameMineRef);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
            LogSystem.Log("Unloading Scene " + AddressableHandles.miniGameMineEnvironmentRef);
            
        }
        else
        {
#endif
            LogSystem.Log("Unloading Scene " + AddressableHandles.miniGameMineEnvironmentRef + " and bundle " + environmentRef.BundleName);
#if UNITY_EDITOR
        }
#endif

        await SceneManager.UnloadSceneAsync(AddressableHandles.miniGameMineEnvironmentRef);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
#endif
            await environmentBundle.UnloadAsync(true);
#if UNITY_EDITOR
        }
#endif

        game.researchFactory.GameCanvas.SetActive(true);

        if (!VRManager.instance.VREnabled)
        {
            game.gameCamera.gameObject.SetActive(true);
        }

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}