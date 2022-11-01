using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{

    private Button button;
    private SoundManager soundManager;
    public AudioClip UIClick;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        soundManager = GameObject.FindGameObjectWithTag("audio").GetComponent<SoundManager>();
        button.onClick.AddListener(PlaySoundOnClick);
    }

    void PlaySoundOnClick()
    {
        soundManager.Play(UIClick);
    }
}
