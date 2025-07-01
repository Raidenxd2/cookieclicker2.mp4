using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class VRFallbackScene : MonoBehaviour
{
    public void LoadInitScene()
    {
        LoadInitSceneAsync().Forget();
    }

    private async UniTaskVoid LoadInitSceneAsync()
    {
        await Addressables.UnloadSceneAsync(AddressableHandles.instance.vrFallbackSceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        AddressableHandles.instance.initSceneHandle = Addressables.LoadSceneAsync(AddressableHandles.instance.initSceneRef, LoadSceneMode.Single);
        await AddressableHandles.instance.initSceneHandle;
    }
}