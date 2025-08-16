#if !CC2_REMOVE_VR_SUPPORT
using UnityEngine;

public class VRCanvas : MonoBehaviour
{
    [SerializeField] private Vector3 newPos;
    [SerializeField] private Vector3 newScale = new(0.01f, 0.01f, 0.01f);
    [SerializeField] private Vector3 newRot;

    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            GetComponent<RectTransform>().sizeDelta = new(1920, 1080);
            transform.localScale = newScale;
            transform.rotation = Quaternion.Euler(newRot);
            transform.position = newPos;
        }
    }
}
#endif