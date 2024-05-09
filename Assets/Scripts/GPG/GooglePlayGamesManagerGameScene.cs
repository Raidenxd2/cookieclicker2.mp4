#if UNITY_ANDROID
using UnityEngine;

public class GooglePlayGamesManagerGameScene : MonoBehaviour
{
    public void LoginButton()
    {
        GooglePlayGamesManager.instance.LoginManual();
    }

    public void ShowLeaderboardButton()
    {
        GooglePlayGamesManager.instance.ViewLeaderboard();
    }

    public void ShowAchievementsButton()
    {
        GooglePlayGamesManager.instance.ViewAchievements();
    }
}
#endif