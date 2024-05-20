#if DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using LoggerSystem;

public class AddressableDevLoad : MonoBehaviour
{
    [SerializeField] private AssetReference debugConsoleRef;

    private IEnumerator Start()
    {
        AsyncOperationHandle<GameObject> debugConsoleHandle = Addressables.LoadAssetAsync<GameObject>(debugConsoleRef);
        yield return debugConsoleHandle;

        if (debugConsoleHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(debugConsoleHandle.Result);
            LogSystem.Log("Instantiated Debug Console");
        }
    }
}
#endif