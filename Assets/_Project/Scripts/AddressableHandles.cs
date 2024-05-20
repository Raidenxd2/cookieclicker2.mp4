using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class AddressableHandles : MonoBehaviour
{
    public AssetReference gameSceneRef;
    public AssetReference initSceneRef;

    public AsyncOperationHandle<SceneInstance> gameSceneHandle;
    public AsyncOperationHandle<SceneInstance> initSceneHandle;

    public static AddressableHandles instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}