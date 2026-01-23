using UnityEngine;
#if !CC2_REMOVE_VR_SUPPORT
using System;
using UnityEngine.XR.Management;
#endif

public class VRManager : MonoBehaviour
{
    public bool VREnabled;

    public static bool VRBootEnabled;

#if !CC2_REMOVE_VR_SUPPORT
    private XRLoader currentLoader;

    public static VRManager instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void EnableVR()
    {
        if (Environment.CommandLine.Contains("-vr"))
        {
            VRBootEnabled = true;
        }
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
            BeanShootoutURP.EnableXRRenderingSupport = true;
            VREnabled = true;
        }
        
#if UNITY_WEBGL && !CC2_REMOVE_VR_SUPPORT
        VREnabled = true;
        return;
#endif        
        
        if (VREnabled)
        {
            BeanShootoutURP.EnableXRRenderingSupport = true;
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
#endif
}