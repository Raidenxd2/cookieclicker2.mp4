using raiden.utils;
using Cysharp.Threading.Tasks;
using SerialPackage.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


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

        await SceneManager.LoadSceneAsync(SceneNames.miniGameMineRef, LoadSceneMode.Additive);

        BeanLogger.Log("Loading AssetBundle " + environmentRef.BundleName + " and scene " + SceneNames.miniGameMineEnvironmentRef, this);

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
#endif
#if UNITY_WEBGL
            environmentBundle = DownloadHandlerAssetBundle.GetContent(await UnityWebRequestAssetBundle.GetAssetBundle(Application.streamingAssetsPath + "/Bundles/" + environmentRef.BundleName + ".bundle").SendWebRequest());
#else
            environmentBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + environmentRef.BundleName + ".bundle");
#endif
            await SceneManager.LoadSceneAsync(SceneNames.miniGameMineEnvironmentRef, LoadSceneMode.Additive);
#if UNITY_EDITOR
        }
        else
        {
            await EditorSceneManager.LoadSceneAsyncInPlayMode(SceneFullAssetPath, new(LoadSceneMode.Additive));
        }
#endif

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

#if UNITY_EDITOR
        if (LoadAssetBundlesInEditor)
        {
            BeanLogger.Log("Unloading Scene " + SceneNames.miniGameMineEnvironmentRef, this);
        }
        else
        {
#endif
            BeanLogger.Log("Unloading Scene " + SceneNames.miniGameMineEnvironmentRef + " and bundle " + environmentRef.BundleName, this);
#if UNITY_EDITOR
        }
#endif

        await SceneManager.UnloadSceneAsync(SceneNames.miniGameMineEnvironmentRef);

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

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}