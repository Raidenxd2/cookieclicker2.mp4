using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{

    private Button button;
    public AudioClip UIClick;

    private void Awake()
    {
        try
        {
            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(PlaySoundOnClick);
        }
        catch
        {

        }
    }

    public void PlaySoundOnClick()
    {
        SoundManager.Instance.Play(UIClick);
    }
}