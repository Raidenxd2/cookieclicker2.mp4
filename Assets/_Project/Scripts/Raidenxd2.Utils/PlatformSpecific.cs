using UnityEngine;

public class PlatformSpecific : MonoBehaviour
{
    public GameObject[] AndroidOnly;
    public GameObject[] PCOnly;

    void Start()
    {
        if (!Application.isMobilePlatform)
        {
            foreach (var item in AndroidOnly)
            {
                item.SetActive(false);
            }
        }
        if (Application.isMobilePlatform)
        {
            foreach (var item in PCOnly)
            {
                item.SetActive(false);
            }
        }
    }
}