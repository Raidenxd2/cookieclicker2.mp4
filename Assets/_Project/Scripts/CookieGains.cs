using UnityEngine;
using TMPro;

public class CookieGains : MonoBehaviour
{
    private Game game;
    [SerializeField] private TMP_Text text;

    void OnEnable()
    {
        Destroy(gameObject, 1f);
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        text.text = "+" + game.CPC;
    }
}