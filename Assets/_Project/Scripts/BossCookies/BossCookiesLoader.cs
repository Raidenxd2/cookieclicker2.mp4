using com.raiden.assetbundleassetreference.Runtime;
using Cysharp.Threading.Tasks;
using LoggerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            environmentBundle = await AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/Bundles/" + environmentRef.BundleName);
            await SceneManager.LoadSceneAsync(AddressableHandles.bossCookiesEnvironmentRef, LoadSceneMode.Additive);
#if UNITY_EDITOR
        }
        else
        {
            await EditorSceneManager.LoadSceneAsyncInPlayMode(SceneFullAssetPath, new(LoadSceneMode.Additive));
        }
#endif
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(AddressableHandles.bossCookiesEnvironmentRef));

        notificationCanvas.worldCamera = BossCookiesGame.instance.Camera.GetComponent<Camera>();

        game.Fade.Play("FadeOut");
        game.FadeCanvasGroup.blocksRaycasts = false;
    }
}