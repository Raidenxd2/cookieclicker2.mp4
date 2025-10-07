using UnityEngine;
using UnityEngine.XR.Management;

public class VRManager : MonoBehaviour
{
    public bool VREnabled;

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
            Cursor.visible = false;
            InitXR();
        }
    }

    private void InitXR()
    {
        currentLoader = XRGeneralSettings.Instance.Manager.activeLoaders[0];

        if (!currentLoader.Initialize())
        {
            Debug.LogError("(VRManager) Failed to init current loader.");
            VREnabled = false;
            Cursor.visible = true;
            return;
        }

        if (!currentLoader.Start())
        {
            Debug.LogError("(VRManager) Failed to start current loader.");
            currentLoader.Deinitialize();
            VREnabled = false;
            Cursor.visible = true;
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