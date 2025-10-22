using UnityEngine;

public class PlayerFade : MonoBehaviour
{
    [SerializeField] private Animator FadeAnimator;

#if !CC2_DISABLEBOSSCOOKIESVRMODE
    public void FadeIn()
    {
        FadeAnimator.Play("FadeIn");
    }

    public void FadeOut()
    {
        FadeAnimator.Play("FadeOut");
    }
#endif
}