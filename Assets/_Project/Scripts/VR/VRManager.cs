using UnityEngine;
#if !CC2_REMOVE_VR_SUPPORT
using System;
using UnityEngine.XR.Management;
#endif

public class VRManager : MonoBehaviour
{
    public bool VREnabled;
    
#if UNITY_EDITOR
    public bool FakeVR;
#endif

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
#if UNITY_EDITOR
            if (!FakeVR)
            {
#endif
                Cursor.visible = false;
                InitXR();
#if UNITY_EDITOR
            }
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
            BeanShootoutURP.EnableXRRenderingSupport = false;
            Cursor.visible = true;
            return;
        }

        if (!currentLoader.Start())
        {
            Debug.LogError("(VRManager) Failed to start current loader.");
            currentLoader.Deinitialize();
            VREnabled = false;
            BeanShootoutURP.EnableXRRenderingSupport = false;
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