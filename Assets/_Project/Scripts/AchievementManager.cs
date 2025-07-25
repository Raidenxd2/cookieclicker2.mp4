using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private GameObject AchievementPrefab;
    [SerializeField] private Transform AchievementParent;

    [SerializeField] private AchievementSO[] Achievements;

    public static AchievementManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateAchievements();
    }

    public void UpdateAchievements()
    {
        foreach (Transform transform in AchievementParent)
        {
            Destroy(transform.gameObject);
        }

        foreach (var achievement in Achievements)
        {
            AchievementObject ao = Instantiate(AchievementPrefab, AchievementParent).GetComponent<AchievementObject>();

            ao.AchievementNameText.text = achievement.AchievementName;

            if (BetterPrefs.GetBool(achievement.AchievementInternalName, false))
            {
                ao.AchievementUnlocked.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}