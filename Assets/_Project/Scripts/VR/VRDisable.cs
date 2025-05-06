using UnityEngine;

public class VRDisable : MonoBehaviour
{
    private void Start()
    {
        if (VRManager.instance.VREnabled)
        {
            gameObject.SetActive(false);
        }
    }
}