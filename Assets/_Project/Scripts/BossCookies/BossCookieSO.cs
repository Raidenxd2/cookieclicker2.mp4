using UnityEngine;

[CreateAssetMenu(fileName = "BossCookie", menuName = "Cookieclicker2.mp4/Boss Cookie", order = 0)]
public class BossCookieSO : ScriptableObject
{
    public string CookieName;
    public Color CookieColor;
    public int StartingHealth;
    public int CookiesAmount;
}