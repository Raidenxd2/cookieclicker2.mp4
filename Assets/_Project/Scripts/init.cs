using UnityEngine;

public class init : MonoBehaviour
{
    public GameObject DDOL;

    private void Start()
    {
        DontDestroyOnLoad(DDOL);
    }
}