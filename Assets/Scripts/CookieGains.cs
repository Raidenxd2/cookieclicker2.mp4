using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookieGains : MonoBehaviour
{
    public Game game;
    private TMP_Text text;
    private RectTransform atransform;
    public int BossHealth;

    void OnEnable()
    {
        Destroy(gameObject, 1f);
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        text = gameObject.GetComponent<TMP_Text>();
        text.text = "+" + game.CPC;
        // atransform = gameObject.GetComponent<RectTransform>();
        // atransform.localPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    
}
