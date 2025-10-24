using UnityEngine;

public class VRCanvas : MonoBehaviour
{
    public Vector3 newPos;
    [SerializeField] private Vector3 newScale = new(0.01f, 0.01f, 0.01f);
    public Vector3 newRot;

#if !CC2_REMOVE_VR_SUPPORT
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
#endif
}