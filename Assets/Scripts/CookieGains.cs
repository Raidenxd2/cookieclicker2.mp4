using UnityEngine;
using TMPro;

public class CookieGains : MonoBehaviour
{
    public Game game;
    private TMP_Text text;
    public int BossHealth;

    void OnEnable()
    {
        Destroy(gameObject, 1f);
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        text = gameObject.GetComponent<TMP_Text>();
        text.text = "+" + game.CPC;
    }
}