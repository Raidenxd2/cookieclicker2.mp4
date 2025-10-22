using UnityEngine;

public class ForceZeroPosition : MonoBehaviour
{
#if !CC2_REMOVE_VR_SUPPORT
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
#endif
}