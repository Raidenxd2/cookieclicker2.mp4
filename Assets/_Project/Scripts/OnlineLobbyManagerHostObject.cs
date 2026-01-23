using Cysharp.Threading.Tasks;
using LoggerSystem;
using Unity.Netcode;

public class OnlineLobbyManagerHostObject : NetworkBehaviour
{
    public static OnlineLobbyManagerHostObject instance;

    public NetworkVariable<int> timer = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        instance = this;
    }

    [Rpc(SendTo.Everyone)]
    public void UpdatePlayerListRpc()
    {
        LogSystem.Log("UpdatePlayerListRpc");
        
        OnlineLobbyManager.instance.FinishConnect();
        OnlineLobbyManager.instance.RefreshPlayerList().Forget();
    }

    [Rpc(SendTo.NotServer)]
    public void HostDisconnectRpc()
    {
        OnlineLobbyManager.instance.ShowNetworkError(OnlineLobbyManager.instance.HostLeftError);
        NetworkManager.Singleton.Shutdown();
    }

    [Rpc(SendTo.NotServer)]
    public void ShowWaitingForHostRpc()
    {
        OnlineLobbyManager.instance.ShowWaitingForHost();
    }

    [Rpc(SendTo.NotServer)]
    public void LoadGameSceneRpc()
    {
        OnlineLobbyManager.InOnlineGame = true;
        OnlineLobbyManager.instance.LoadGameScene();
    }

    public async UniTaskVoid TimerTick()
    {
        if (!OnlineLobbyManager.InOnlineGame)
        {
            return;
        }
        await UniTask.WaitForSeconds(1f);
        if (!OnlineLobbyManager.InOnlineGame)
        {
            return;
        }

        timer.Value--;

        if (timer.Value <= 0)
        {
            EndGameRpc();
            return;
        }
        
        TimerTick().Forget();
    }

    [Rpc(SendTo.Everyone)]
    private void EndGameRpc()
    {
        Game.instance.AllowUpdate = false;
        Game.instance.TimerRanOutScreen.SetActive(true);
        Game.instance.Disconnecting = true;
        OnlineLobbyManager.InOnlineGame = false;
        NetworkManager.Singleton.Shutdown();
    }
}