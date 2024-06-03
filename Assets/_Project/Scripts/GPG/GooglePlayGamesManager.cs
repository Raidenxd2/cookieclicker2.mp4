#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine;
using LoggerSystem;
using BreakInfinity;
using System;

#if UNITY_ANDROID
public class GooglePlayGamesManager : MonoBehaviour
{
    public GameObject GPGLoggingIn;
    public GameObject GPGFailedToLogIn;
    public GameObject GPGButton;
    public bool LoggedInToGPG;

    public string Token;

    public static GooglePlayGamesManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
            LogSystem.Log("Google Play Games isnt supported on this platform. (Not signing in)", LogTypes.Error);
            return;
        }
      if (status == SignInStatus.Success) {

        PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => {
            Token = code;
        });
        // Continue with Play Games Services
        LoggedInToGPG = true;
      } else {
        // Disable your integration with Play Games Services or show a login button
        // to ask users to sign-in. Clicking it should call
        // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        LoggedInToGPG = false;
      }
    }

    public void GiveAchievement(string ID)
    {
        if (LoggedInToGPG)
        {
            PlayGamesPlatform.Instance.ReportProgress(ID, 100.0f, (bool success) => {
                if (success == true)
                {
                    LogSystem.Log("Gave Achievement.");
                }
                else
                {
                    LogSystem.Log("Failed to give Achievement.", LogTypes.Error);
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
                    LogSystem.Log("Set Achievement value.");
                }
                else
                {
                    LogSystem.Log("Failed to set Achievement value.", LogTypes.Error);
                }
            });
        }
        
    }

    public void SetLeaderboardValue(BigDouble val, string ID)
    {
        PlayGamesPlatform.Instance.ReportScore((long) val, ID, (bool success) => {
            if (success == true)
            {
                LogSystem.Log("Set leaderboard value.");
            }
            else
            {
                LogSystem.Log("Failed to set leaderboard value.", LogTypes.Error);
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
}
#endif