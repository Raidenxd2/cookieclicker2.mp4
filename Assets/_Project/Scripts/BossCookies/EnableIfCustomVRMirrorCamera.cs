using UnityEngine;

public class EnableIfCustomVRMirrorCamera : MonoBehaviour
{
    [SerializeField] private GameObject go;

#if !CC2_REMOVE_VR_SUPPORT
    private void Awake()
    {
        if (BetterPrefs.GetBool("VR_MirrorCamera", false) && VRManager.instance.VREnabled)
        {
            go.SetActive(true);
        }
    }
#endif
}