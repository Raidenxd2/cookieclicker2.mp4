using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetaContent : MonoBehaviour
{

    public GameObject BetaContentWarning;
    public GameObject SideBar;
    public GameObject ModsBTN;
    public SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("HasPlayed", 0));
        if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
        {
            PlayerPrefs.SetInt("HasPlayed", 1);
            PlayerPrefs.SetInt("BetaContent", 0);
            PlayerPrefs.SetInt("BETA_EnableSideBar", 0);
            PlayerPrefs.SetInt("BETA_Mods", 0);
            PlayerPrefs.Save();
        }
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + scene.name + "'.");
        //Debug.Log(SceneManager.GetSceneByName("Init").ToString());
        if (scene.name == "Init" && PlayerPrefs.GetInt("BetaContent", 0) == 1)
        {
            BetaContentWarning.SetActive(true);
            return;
        }
        if (PlayerPrefs.GetInt("BetaContent", 0) == 1)
        {
            if (PlayerPrefs.GetInt("BETA_EnableSideBar", 0) == 1)
            {
                SideBar.SetActive(true);
            }
            if (PlayerPrefs.GetInt("BETA_Mods", 0) == 1)
            {
                ModsBTN.SetActive(true);
            }
        }
        //sceneLoader.LoadScene(1);
        StartCoroutine(sceneLoader.LoadScene(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
