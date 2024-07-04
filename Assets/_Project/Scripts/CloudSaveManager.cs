using System;
using System.Threading.Tasks;
using LoggerSystem;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Localization;

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private TMP_Text PlayerIDText;

    [SerializeField] private TMP_Text StatusText;
    [SerializeField] private GameObject StatusScreen;

    [SerializeField] private LocalizedString CloudSaveDisabledError;
    [SerializeField] private LocalizedString Preparing;
    [SerializeField] private LocalizedString DeletingAllKeys;
    [SerializeField] private LocalizedString DeletingFileFromCloud;
    [SerializeField] private LocalizedString DeletingAccount;
    [SerializeField] private LocalizedString DeleteDataSuccess;
    [SerializeField] private LocalizedString FailedToDelete;
    [SerializeField] private LocalizedString EnabledCloudSave;
    [SerializeField] private LocalizedString DisabledCloudSave;

    [SerializeField] private LocalizedString PlayerID;

    public async void DeleteCloudData()
    {
        if (PlayerPrefs.GetInt("EnableCloudSave", 0) == 2)
        {
            StatusScreen.SetActive(true);
            StatusText.text = CloudSaveDisabledError.GetLocalizedString();
            await Task.Delay(3500);
            StatusScreen.SetActive(false);
            return;
        }

        try
        {
            StatusScreen.SetActive(true);
            StatusText.text = Preparing.GetLocalizedString();

            StatusText.text = DeletingAllKeys.GetLocalizedString();
            await CloudSaveService.Instance.Data.Player.DeleteAllAsync();
            StatusText.text = DeletingFileFromCloud.GetLocalizedString();
            await CloudSaveService.Instance.Files.Player.DeleteAsync("cookie2");

            StatusText.text = DeletingAccount.GetLocalizedString();
            await AuthenticationService.Instance.DeleteAccountAsync();

            StatusText.text = DeleteDataSuccess.GetLocalizedString();
            await Task.Delay(5000);
            StatusScreen.SetActive(false);
        }
        catch (Exception ex)
        {
            StatusText.text = FailedToDelete.GetLocalizedString() + "\n" + ex.ToString();
            LogSystem.Log(ex.ToString(), LogTypes.Exception);
            await Task.Delay(10000);
            StatusScreen.SetActive(false);
        }
    }

    public async void EnableCloudSave()
    {
        PlayerPrefs.SetInt("EnableCloudSave", 1);

        StatusScreen.SetActive(true);
        StatusText.text = EnabledCloudSave.GetLocalizedString();
        await Task.Delay(3500);
        StatusScreen.SetActive(false);
    }

    public async void DisableCloudSave()
    {
        PlayerPrefs.SetInt("EnableCloudSave", 2);
        
        StatusScreen.SetActive(true);
        StatusText.text = DisabledCloudSave.GetLocalizedString();
        await Task.Delay(3500);
        StatusScreen.SetActive(false);
    }

    public void DisableCloudSaveNoStatusScreen()
    {
        PlayerPrefs.SetInt("EnableCloudSave", 2);
    }

    public void VisitDataDeletionWebsite()
    {
        OpenURLManager.instance.OpenURL("https://raidenxd2.github.io/cookieclicker2.mp4/DataDeletion.html");
    }

    public void OpenPlayerIDScreen()
    {
        PlayerIDText.text = PlayerID.GetLocalizedString() + AuthenticationService.Instance.PlayerId;
    }

    public void CopyPlayerID()
    {
        GUIUtility.systemCopyBuffer = AuthenticationService.Instance.PlayerId;
    }
}