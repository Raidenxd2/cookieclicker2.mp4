using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;

public class BetaContent : MonoBehaviour
{

    public GameObject BetaContentWarning;
    public GameObject SideBar;
    public GameObject ModsBTN;
    public GameObject FPSLimitNoFocus;
    public GameObject DownloadingDataScreen;
    public GameObject DownloadMusicScreen;
    public GameObject FinishedDownloadingScreen;
    public SceneLoader sceneLoader;
    public GameObject CookieMonsterScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("HasPlayed", 0));
        if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
        {
            //PlayerPrefs.SetInt("HasPlayed", 1);
            PlayerPrefs.SetInt("BetaContent", 0);
            PlayerPrefs.SetInt("BETA_EnableSideBar", 0);
            PlayerPrefs.SetInt("BETA_Mods", 0);
            PlayerPrefs.SetInt("BETA_FPSLIMIT", 0);
            PlayerPrefs.SetInt("BETA_CookieMonster", 0);
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
            if (PlayerPrefs.GetInt("BETA_FPSLIMIT", 0) == 1)
            {
                FPSLimitNoFocus.SetActive(true);
            }
            if (PlayerPrefs.GetInt("BETA_CookieMonster", 0) == 1)
            {
                CookieMonsterScript.SetActive(true);
            }
        }
        if (scene.name != "Game")
        {
            LoadScene();
        }
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator GetText(string url, string filename)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            AsyncOperation operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            var data = www.downloadHandler.text;
            // LoggerSystem.Logger.Log("Data from server: " + data, LoggerSystem.LogTypes.Normal);
            // File.Create(Application.persistentDataPath + "/Cache/CookieStore.txt");
            File.WriteAllText(Application.persistentDataPath + "/Cache/" + filename, www.downloadHandler.text);
            // DownloadedFile = true;
        }
    }
}
