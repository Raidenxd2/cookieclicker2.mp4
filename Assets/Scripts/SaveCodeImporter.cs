using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using BreakInfinity;

public class SaveCodeImporter : MonoBehaviour
{

    public TMP_InputField saveInput;
    public Image RRProfileImage;
    public GameObject RRProfileScreen;
    public GameObject ApiBanned;
    public TMP_Text RRProfileName;
    public TMP_Text RRProfileDisplayName;
    public GameObject RRProfileJR;
    public string RRProfileJSON;
    public string[] variables;
    public Game game;
    public long HTTPError;
    

    public void StartSaveImport()
    {
        StartCoroutine(saveImport());
    }

    IEnumerator saveImport()
    {
        variables = saveInput.text.Split(',');
        //https://accounts.rec.net/account?username=raidenxd2
        Debug.Log("Save code data");
        for (int i = 0; i < variables.Length; i++)
        {
            Debug.Log(variables[i]);
        }
        try
        {
            StartCoroutine(GetText(Application.streamingAssetsPath + "/RecRoomProfileData.json", "https://accounts.rec.net/account?username=" + variables[0]));
        }
        catch
        {
            RRProfileScreen.SetActive(true);
            Debug.LogError("Could not fetch data from the Rec Room servers");
        }
        yield return new WaitForSeconds(1);
        if (HTTPError != 403)
        {
            try
            {
                var test = JsonConvert.DeserializeObject<RRProfile>(RRProfileJSON);
                Debug.Log(test.username);
                Debug.Log(RRProfileJSON);
                RRProfileJR.SetActive(test.isJunior);
                RRProfileName.text = "@" + test.username;
                RRProfileDisplayName.text = test.displayName;
                StartCoroutine(DownloadImage("https://img.rec.net/" + test.profileImage +"?cropSquare=true&width=192&height=192"));
                RRProfileScreen.SetActive(true);
            }
            catch
            {
                RRProfileScreen.SetActive(true);
                Debug.LogError("Could not fetch data from the Rec Room servers");
            }
        }
        
    }

    public void SetDataValues()
    {
        Debug.Log(variables[1]);
        game.Cookies = BigDouble.Parse(variables[1]);
        game.CPS = BigDouble.Parse(variables[2]);
        game.CPC = BigDouble.Parse(variables[3]);
        game.Autoclickers = BigDouble.Parse(variables[4]);
        game.AutoclickerPrice = BigDouble.Parse(variables[5]);
        game.Doublecookies = BigDouble.Parse(variables[6]);
        game.DoublecookiePrice = BigDouble.Parse(variables[7]);
        game.Grandmas = BigDouble.Parse(variables[8]);
        game.GrandmaPrice = BigDouble.Parse(variables[9]);
        game.SavePlayer();
    }

    IEnumerator DownloadImage(string MediaUrl)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        RRProfileImage.material.mainTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    } 

    IEnumerator GetText(string savePath, string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            AsyncOperation operation = www.SendWebRequest();
            while (!operation.isDone)
            {
                yield return null;
            }
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                UnityEngine.Debug.LogError(www.error);
            }
            if (www.responseCode == 403)
            {
                ApiBanned.SetActive(true);
                Debug.LogError("IP is banned from the API (" + www.error + ")");
                HTTPError = 403;
            }
            else
            {
                RRProfileJSON = www.downloadHandler.text;
            }
        }
    }
}