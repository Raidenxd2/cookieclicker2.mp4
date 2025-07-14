#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine;
using UnityEngine.XR.Management;

public class VRManager : MonoBehaviour
{
    public bool VREnabled;
#if UNITY_ANDROID
    public bool IsMobileVR;
#endif

    public static bool VRBootEnabled;

    private XRLoader currentLoader;

    public static VRManager instance;

    [UnityCommandLineParser.CommandLineCommand("vr")]
    public static void EnableVR()
    {
        VRBootEnabled = true;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        if (VRBootEnabled)
        {
            VREnabled = true;
        }

        if (VREnabled)
        {
#if !UNITY_ANDROID
            InitXR();
#endif
        }
    }

    private void InitXR()
    {
        currentLoader = XRGeneralSettings.Instance.Manager.activeLoaders[0];

        if (!currentLoader.Initialize())
        {
            Debug.LogError("(VRManager) Failed to init current loader.");
            VREnabled = false;
            return;
        }

        if (!currentLoader.Start())
        {
            Debug.LogError("(VRManager) Failed to start current loader.");
            currentLoader.Deinitialize();
            VREnabled = false;
            return;
        }

        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }

    private void OnDestroy()
    {
        if (VREnabled)
        {
            currentLoader.Stop();
            currentLoader.Deinitialize();
            currentLoader = null;
        }
    }
}
#endif