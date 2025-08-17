#if !CC2_REMOVE_VR_SUPPORT
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRFallbackScene : MonoBehaviour
{
    public void LoadInitScene()
    {
        LoadInitSceneAsync().Forget();
    }

    private async UniTaskVoid LoadInitSceneAsync()
    {
        await SceneManager.LoadSceneAsync(AddressableHandles.initSceneRef);
    }
}
#endif