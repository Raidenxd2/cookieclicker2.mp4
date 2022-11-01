using UnityEngine;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour
{
    public GameObject DDOL;
    void Start()
    {
        DontDestroyOnLoad(DDOL);
        SceneManager.LoadScene("Game");
    }
}
