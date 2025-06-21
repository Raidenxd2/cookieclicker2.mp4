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
    [SerializeField] private GameObject LoadingAssetVR;

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
        await UniTask.WaitForSeconds(1);

        if (VRManager.instance.VREnabled)
        {
            LoadingAssetVR.SetActive(true);
        }
        else
        {
            LoadingAsset.SetActive(true);
        }

        game.researchFactory.GameCanvas.SetActive(false);
        game.gameCamera.gameObject.SetActive(false);

        MinigameMineSceneHandle = await Addressables.LoadSceneAsync(MinigameMineScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(MinigameMineSceneHandle.Scene);

        if (VRManager.instance.VREnabled)
        {
            LoadingAssetVR.SetActive(false);
        }
        else
        {
            LoadingAsset.SetActive(false);
        }

        game.Fade.Play("FadeOut");
    }

    public void UnloadMinigameMine()
    {
        UnloadMinigameMineAsync().Forget();
    }

    private async UniTaskVoid UnloadMinigameMineAsync()
    {
        game.Fade.Play("FadeIn");
        await UniTask.WaitForSeconds(1);

        if (VRManager.instance.VREnabled)
        {
            Game.instance.XROrigin.transform.parent = MiniGameMine.instance.OldVRParent;
            Game.instance.XROrigin.transform.SetPositionAndRotation(MiniGameMine.instance.OldVRPosition, MiniGameMine.instance.OldVRRotation);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));

        await Addressables.UnloadSceneAsync(MinigameMineSceneHandle);

        game.researchFactory.GameCanvas.SetActive(true);

        if (!VRManager.instance.VREnabled)
        {
            game.gameCamera.gameObject.SetActive(true);
        }

        game.Fade.Play("FadeOut");
    }
}