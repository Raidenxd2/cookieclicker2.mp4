using UnityEngine;

public class init : MonoBehaviour
{
    public GameObject DDOL;

    void Start()
    {
        Resources.UnloadUnusedAssets();
        DontDestroyOnLoad(DDOL);
    }
}