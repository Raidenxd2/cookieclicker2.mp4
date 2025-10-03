using UnityEngine;

public class SingletonVariables : MonoBehaviour
{
    public GameObject FPSDisplayRoot;

    private bool ignoreDestroy;

    public static SingletonVariables instance;

    private void Awake()
    {
        if (instance != null)
        {
            ignoreDestroy = true;
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnDestroy()
    {
        if (ignoreDestroy)
        {
            return;
        }
        instance = null;
    }
}