using UnityEngine;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour
{
    public GameObject DDOL;
    public SceneLoader sceneLoader;
    void Start()
    {
        Resources.UnloadUnusedAssets();
        DontDestroyOnLoad(DDOL);
        sceneLoader.LoadScene(1);
    }
}
