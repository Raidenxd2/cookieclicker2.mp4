using TMPro;
using UnityEngine;

public class BossCookieObject : MonoBehaviour
{
    public TMP_Text CookieNameText;
    public TMP_Text HealthText;
    public TMP_Text CookiesText;
    public MeshRenderer CookieRenderer;

    public float StartingHealth;
    public float Health;
    public int CookiesAmount;

    private void FixedUpdate()
    {
        HealthText.text = "Health: " + Health;
    }
}