using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRFallbackScene : MonoBehaviour
{
    public void LoadInitScene()
    {
#if !CC2_REMOVE_VR_SUPPORT
        LoadInitSceneAsync().Forget();
#endif
    }

#if !CC2_REMOVE_VR_SUPPORT
    private async UniTaskVoid LoadInitSceneAsync()
    {
        await SceneManager.LoadSceneAsync(AddressableHandles.initSceneRef);
    }
#endif
}