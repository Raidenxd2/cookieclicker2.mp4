using UnityEngine;

public class BuildConfig : MonoBehaviour
{
    public bool BetaBuild;
    public bool Beta_SaveLogsToFile;

    public static BuildConfig instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }
}