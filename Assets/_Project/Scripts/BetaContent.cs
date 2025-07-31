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
        if (BetterPrefs.GetBool("HasPlayed", false))
        {
            BetterPrefs.SetBool("BetaContent", false);
            BetterPrefs.SetBool("BETA_ResearchFactory", false);
        }

        UpdateBetaContent();
    }

    public void UpdateBetaContent()
    {
        if (BetterPrefs.GetBool("BetaContent", false))
        {
            if (BetterPrefs.GetBool("BETA_ResearchFactory", false))
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