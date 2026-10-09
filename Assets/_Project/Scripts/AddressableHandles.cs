using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public static class AddressableHandles
{
    public static AsyncOperationHandle<SceneInstance> InitHandle;
    public static AsyncOperationHandle<SceneInstance> GameHandle;
    public static AsyncOperationHandle<SceneInstance> MineHandle;
    public static AsyncOperationHandle<SceneInstance> BossCookiesHandle;
    public static AsyncOperationHandle<SceneInstance> OnlineLobbyHandle;
}