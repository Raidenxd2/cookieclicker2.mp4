using UnityEngine;
using TMPro;

public class CookieGains : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void OnEnable()
    {
        Destroy(gameObject, 1f);
        text.text = "+" + Game.instance.CPC;
    }
}