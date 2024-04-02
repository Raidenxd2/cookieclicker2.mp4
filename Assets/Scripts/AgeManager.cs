using UnityEngine;

public class AgeManager : MonoBehaviour
{
    [SerializeField] private GameObject ageScreen;

    // 0: 12-9
    // 1: 13+
    private int age = 1;

    private void Start()
    {
        if (PlayerPrefs.GetInt("SetAge", 0) == 0)
        {
            ageScreen.SetActive(true);
        }
    }

    public void UpdateAgeSetting(int val)
    {
        age = val;
    }

    public void CompleteAgeSet()
    {
        PlayerPrefs.SetInt("SetAge", 1);
        PlayerPrefs.SetInt("Age", age);
    }
}