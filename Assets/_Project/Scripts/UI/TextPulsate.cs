using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

public class TextPulsate : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;

    [SerializeField] private float duration;
    
    private void Start()
    {
        Anim().Forget();
    }

    private async UniTaskVoid Anim()
    {
        if (!cg)
        {
            return;
        }
        
        await LMotion.Create(1, 0.2f, duration)
            .WithEase(Ease.Linear)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .Bind(x => cg.alpha = x);

        await LMotion.Create(0.2f, 1, duration)
            .WithEase(Ease.Linear)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .Bind(x => cg.alpha = x);
        
        Anim().Forget();
    }
}