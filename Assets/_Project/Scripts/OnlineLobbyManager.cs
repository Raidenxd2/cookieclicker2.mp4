using System;
using Cysharp.Threading.Tasks;
using LoggerSystem;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class OnlineLobbyManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField IPInput;
    [SerializeField] private TMP_InputField UsernameInput;

    [SerializeField] private TMP_Text HostIPText;
    
    [SerializeField] private Animator Fade;
    [SerializeField] private CanvasGroup FadeCanvasGroup;
    
    private AudioSource MusicAudioSource;
    private GameObject MusicSource;

    [SerializeField] private LocalizedString TransportError;
    [SerializeField] private LocalizedString ServerConnectError;
    [SerializeField] private LocalizedString ClientStartError;
    [SerializeField] private LocalizedString HostStartError;
    public LocalizedString HostLeftError;
    [SerializeField] private GameObject NetworkErrorScreen;
    [SerializeField] private TMP_Text NetworkErrorText;

    [SerializeField] private GameObject HelpScreen;
    [SerializeField] private GameObject ConnectingScreen;
    [SerializeField] private GameObject LobbyScreen;
    [SerializeField] private GameObject HostDisconnectWarningScreen;

    [SerializeField] private LocalizedString WaitingForHost;
    [SerializeField] private TMP_Text ConnectingScreen_Info;

    [SerializeField] private GameObject StartGameBTN;
    
    [SerializeField] private GameObject User;
    [SerializeField] private Transform UserParent;

    [SerializeField] private GameObject OnlineLobbyManagerHostObjectGO;
    
    public GameObject pp_normal;
    public UniversalAdditionalCameraData GameCamera_AdditionalData;
    
#if !CC2_REMOVE_VR_SUPPORT
    private GameObject VRPrefabGO;
    private VRPrefabObject vrpo;
#endif
    [SerializeField] private Transform VRPrefabParent;
    public GameObject XROrigin;
    [SerializeField] private VRFadeCanvas vrFade;
    public Camera gameCamera;

    private bool Connecting;
    private bool Disconnecting;

    public static bool InOnlineGame;
    
    public static OnlineLobbyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MusicManager.instance.useOnlineMusic = true;
        
        MusicSource = GameObject.FindGameObjectWithTag("music");
        MusicAudioSource = MusicSource.GetComponent<AudioSource>();
        
        UpdateAudio();
        
        MusicManager.instance.PlayRandomSong();

        NetworkManager.Singleton.OnTransportFailure += OnTransportFailure;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        NetworkManager.Singleton.ConnectionManager.OnDisconnect2 += OnDisconnect;
        
        UpdateUsernameTextInput();

        if (!BetterPrefs.GetBool("Online_SeenHelp", false))
        {
            BetterPrefs.SetBool("Online_SeenHelp", true);
            HelpScreen.SetActive(true);
        }
        
        if (BetterPrefs.GetBool("GRAPHICS_PostProcessing", true))
        {
            pp_normal.SetActive(true);
            GameCamera_AdditionalData.renderPostProcessing = true;
        }
        else
        {
            pp_normal.SetActive(false);
            GameCamera_AdditionalData.renderPostProcessing = false;
        }
        
#if !CC2_REMOVE_VR_SUPPORT
        if (VRManager.instance.VREnabled)
        {
            InitVR().Forget();
        }
#endif
        
        Fade.Play("FadeOut");
    }
    
#if !CC2_REMOVE_VR_SUPPORT
    public async UniTask InitVR()
    {
        if (VRManager.instance.VREnabled)
        {
            try
            {
                VRPrefabGO = Instantiate(await Resources.LoadAsync("OnlineLobby_VRPrefab") as GameObject);
                vrpo = VRPrefabGO.GetComponent<VRPrefabObject>();

                XROrigin = vrpo.XROrigin;

                vrFade.InitVR();
            }
            catch (Exception ex)
            {
                LogSystem.Log(ex.ToString(), LogTypes.Exception);
                LoadVRFallbackSceneAsync().Forget();
                return;
            }

            gameCamera.gameObject.SetActive(false);
        }

        UpdateMirrorCamera();
    }
#endif
    
#if !CC2_REMOVE_VR_SUPPORT
    private async UniTaskVoid LoadVRFallbackSceneAsync()
    {
        await SceneManager.LoadSceneAsync(AddressableHandles.vrFallbackSceneRef);
    }
#endif
    
#if !CC2_REMOVE_VR_SUPPORT
    private void UpdateMirrorCamera()
    {
        if (vrpo == null)
        {
            return;
        }

        vrpo.MirrorCamera.SetActive(BetterPrefs.GetBool("VR_MirrorCamera", false));
    }
#endif

    private void OnDisconnect(ulong obj)
    {
        if (Disconnecting)
        {
            return;
        }
        
        if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (obj == 0)
            {
                LogSystem.Log("Host left.", LogTypes.Warning);

                ShowNetworkError(HostLeftError);
            }
        }
    }

    private void UpdateUsernameTextInput()
    {
        UsernameInput.text = BetterPrefs.GetString("Online_Username", "New001");
    }

    public void UpdateUsername(string val)
    {
        BetterPrefs.SetString("Online_Username", val);
    }

    private void OnClientDisconnect(ulong obj)
    {
        if (Disconnecting)
        {
            return;
        }
        
        if (Connecting)
        {
            ShowNetworkError(ServerConnectError);
            return;
        }

        if (NetworkManager.Singleton.IsHost)
        {
            OnlineLobbyManagerHostObject.instance.UpdatePlayerListRpc();
        }
    }

    private void OnTransportFailure()
    {
        ShowNetworkError(TransportError);
    }

    private void UpdateAudio()
    {
        // music & sounds
        if (!BetterPrefs.GetBool("Music"))
        {
            MusicAudioSource.volume = 0;
        }
        else
        {
            MusicAudioSource.volume = 1;
        }
    }

    public void StartHost()
    {
        try
        {
            NetworkManager.Singleton.StartHost();
        
            GameObject tempGO = Instantiate(OnlineLobbyManagerHostObjectGO);
            tempGO.GetComponent<NetworkObject>().Spawn(false);

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        
            LobbyScreen.SetActive(true);
        
            OnlineLobbyManagerHostObject.instance.UpdatePlayerListRpc();
        }
        catch (Exception e)
        {
            LogSystem.Log("Error starting host", LogTypes.Exception);
            Debug.LogException(e);
            
            ShowNetworkError(HostLeftError);
        }
    }

    private void OnClientConnected(ulong obj)
    {
        LogSystem.Log("Client " + obj + " connected.");
        
        OnlineLobbyManagerHostObject.instance.UpdatePlayerListRpc();
    }

    public void StartClient()
    {
        try
        {
            ConnectingScreen.SetActive(true);
            Connecting = true;
        
            StartGameBTN.SetActive(false);
        
            NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = IPInput.text;

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            LogSystem.Log("Error starting client", LogTypes.Exception);
            Debug.LogException(e);
            
            ShowNetworkError(ClientStartError);
        }
    }

    public void FinishConnect()
    {
        ConnectingScreen.GetComponent<WindowAnimations>().HideWindow();
        Connecting = false;
        
        LobbyScreen.SetActive(true);
    }
    
    public async UniTaskVoid RefreshPlayerList()
    {
        await UniTask.WaitForSeconds(0.1f);
        
        for (int b = 0; b < UserParent.childCount; b++)
        {
            Destroy(UserParent.GetChild(b).gameObject);
        }

        foreach (var client in NetworkManager.Singleton.SpawnManager.PlayerObjects)
        {
            UserObject user = Instantiate(User, UserParent).GetComponent<UserObject>();
            
            LogSystem.Log(client.GetComponent<OnlinePlayerObject>().Username.Value.ToString());
            
            user.UserNameText.text = client.GetComponent<OnlinePlayerObject>().Username.Value.ToString();
        }
    }

    public void Disconnect()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostDisconnectWarningScreen.SetActive(true);
        }
        else
        {
            Disconnect2();
        }
    }

    public void Disconnect2()
    {
        Disconnecting = true;
        
        if (NetworkManager.Singleton.IsHost)
        {
            OnlineLobbyManagerHostObject.instance.HostDisconnectRpc();
        }
        
        NetworkManager.Singleton.Shutdown();
        
        LoadGameScene();
    }

    public void StartGame()
    {
        OnlineLobbyManagerHostObject.instance.ShowWaitingForHostRpc();

        InOnlineGame = true;
        LoadGameScene();
    }

    public void ShowWaitingForHost()
    {
        DontDestroyOnLoad(GameObject.Find("OnlineLobbyManagerHostObject(Clone)"));
        
        ConnectingScreen.SetActive(true);
        ConnectingScreen_Info.text = WaitingForHost.GetLocalizedString();
    }
    
    public void UpdateHostIPText()
    {
        UpdateHostIPTextAsync().Forget();
    }

    private async UniTask UpdateHostIPTextAsync()
    {
        HostIPText.text = (await UnityWebRequest.Get("https://api.ipify.org/").SendWebRequest()).downloadHandler.text;
    }

    public void LoadGameScene()
    {
        LoadGameSceneAsync().Forget();
    }

    private async UniTaskVoid LoadGameSceneAsync()
    {
        Fade.Play("FadeIn");
        FadeCanvasGroup.blocksRaycasts = true;
        await UniTask.WaitForSeconds(1);
        
        MusicManager.instance.UnloadSong();
        
        await SceneManager.LoadSceneAsync(AddressableHandles.initSceneRef);
    }

    public void ShowNetworkError(LocalizedString ls)
    {
        NetworkErrorScreen.SetActive(true);
        NetworkErrorText.text = ls.GetLocalizedString();
    }

    private void OnDestroy()
    {
        try
        {
            NetworkManager.Singleton.OnTransportFailure -= OnTransportFailure;
        }
        catch
        {
            
        }

        try
        {
            NetworkManager.Singleton.ConnectionManager.OnDisconnect2 -= OnDisconnect;
        }
        catch
        {
            
        }
        
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
    
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    public static void ResetValues()
    {
        InOnlineGame = false;
    }
#endif
}