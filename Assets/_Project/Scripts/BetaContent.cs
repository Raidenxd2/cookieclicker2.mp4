using UnityEngine;

public class BetaContent : MonoBehaviour
{
    [SerializeField] private GameObject ResearchFactoryButton;

    public static BetaContent instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HasPlayed", 0) == 0)
        {
            PlayerPrefs.SetInt("BetaContent", 0);
            PlayerPrefs.SetInt("BETA_ResearchFactory", 0);
            PlayerPrefs.Save();
        }

        UpdateBetaContent();
    }

    public void UpdateBetaContent()
    {
        if (PlayerPrefs.GetInt("BetaContent", 0) == 1)
        {
            if (PlayerPrefs.GetInt("BETA_ResearchFactory", 0) == 1)
            {
                ResearchFactoryButton.SetActive(true);
            }
            else
            {
                ResearchFactoryButton.SetActive(false);
            }
        }
    }
}