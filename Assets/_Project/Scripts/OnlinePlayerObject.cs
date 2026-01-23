using LoggerSystem;
using Unity.Collections;
using Unity.Netcode;

public class OnlinePlayerObject : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> Username = new("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> Cookies = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public static OnlinePlayerObject instance;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (IsOwner)
        {
            instance = this;
            
            Username.Value = BetterPrefs.GetString("Online_Username", "New001");
            LogSystem.Log(Username.Value.ToString());
        }
    }

    public override void OnDestroy()
    {
        if (IsOwner)
        {
            instance = null;
        }
        
        base.OnDestroy();
        Username.Dispose();
    }
}