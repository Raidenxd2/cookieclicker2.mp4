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
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}