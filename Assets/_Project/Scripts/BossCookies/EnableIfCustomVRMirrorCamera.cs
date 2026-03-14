using UnityEngine;

public class EnableIfCustomVRMirrorCamera : MonoBehaviour
{
    [SerializeField] private GameObject go;

    private void Awake()
    {
        if (BetterPrefs.GetBool("VR_MirrorCamera", false) && VRManager.instance.VREnabled)
        {
            go.SetActive(true);
        }
    }
}