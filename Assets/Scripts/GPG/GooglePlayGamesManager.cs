using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using LoggerSystem;
using BreakInfinity;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public class GooglePlayGamesManager : MonoBehaviour
{

    public GameObject GPGLoggingIn;
    public GameObject GPGFailedToLogIn;
    public GameObject GPGButton;
    public bool LoggedInToGPG;

    // Start is called before the first frame update
    void Start()
    {
        //GPGLoggingIn.SetActive(true);
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status) {
        if (!Application.isMobilePlatform)
        {
            // GPGButton.SetActive(false);
            LoggerSystem.Logger.Log("Google Play Games isnt supported on this platform. (Not signing in)", LogTypes.Error);
            return;
        }
      if (status == SignInStatus.Success) {
        // Continue with Play Games Services
        LoggedInToGPG = true;
        GPGLoggingIn.SetActive(false);
      } else {
        // Disable your integration with Play Games Services or show a login button
        // to ask users to sign-in. Clicking it should call
        // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        LoggedInToGPG = false;
        GPGLoggingIn.SetActive(false);
        GPGFailedToLogIn.SetActive(true);
      }
    }

    public void GiveAchievement(string ID)
    {
        if (LoggedInToGPG)
        {
            PlayGamesPlatform.Instance.ReportProgress(ID, 100.0f, (bool success) => {
                if (success == true)
                {
                    LoggerSystem.Logger.Log("Gave Achievement.", LogTypes.Normal);
                }
                else
                {
                    LoggerSystem.Logger.Log("Failed to give Achievement.", LogTypes.Error);
                }
            });
        }
    }

    public void IncreseAchievement(string ID, int val)
    {
        if (LoggedInToGPG)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(ID,val, (bool success) => {
                if (success == true)
                {
                    LoggerSystem.Logger.Log("Set Achievement value.", LogTypes.Normal);
                }
                else
                {
                    LoggerSystem.Logger.Log("Failed to set Achievement value.", LogTypes.Error);
                }
            });
        }
        
    }

    public void SetLeaderboardValue(BigDouble val, string ID)
    {
        PlayGamesPlatform.Instance.ReportScore((long) val, ID, (bool success) => {
            if (success == true)
            {
                LoggerSystem.Logger.Log("Set leaderboard value.", LogTypes.Normal);
            }
            else
            {
                LoggerSystem.Logger.Log("Failed to set leaderboard value.", LogTypes.Error);
            }
        });
    }

    public void ViewAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void ViewLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }

    public void LoginManual()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    public void Logout()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
