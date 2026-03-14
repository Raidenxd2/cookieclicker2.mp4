using UnityEngine;

public class BetaContent : MonoBehaviour
{
    [SerializeField] private GameObject ResearchFactoryButton;
    [SerializeField] private GameObject OnlineButton;

    private void Start()
    {
        UpdateBetaContent();
    }

    public void UpdateBetaContent()
    {
        if (BetterPrefs.GetBool("BetaContent", false))
        {
            ResearchFactoryButton.SetActive(BetterPrefs.GetBool("BETA_ResearchFactory", false));
            OnlineButton.SetActive(BetterPrefs.GetBool("BETA_OnlineMode", false));
        }
    }
}