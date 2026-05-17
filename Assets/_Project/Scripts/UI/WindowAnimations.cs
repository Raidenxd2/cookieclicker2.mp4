using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class WindowAnimations : MonoBehaviour
{
    [SerializeField] private Ease inEase;
    [SerializeField] private Ease outEase;
    [SerializeField] private float Time;

    private bool CurrentlyDoingAnimation;

    private void OnEnable()
    {
        LMotion.Create(Vector3.zero, Vector3.one, Time)
            .WithEase(inEase)
            .BindToLocalScale(transform);
        LMotion.Create(new Vector3(-90, 0, 0), Vector3.zero, Time)
            .WithEase(inEase)
            .BindToLocalEulerAngles(transform);
    }

    public void HideWindow()
    {
        if (CurrentlyDoingAnimation)
        {
            return;
        }
        
        CurrentlyDoingAnimation = true;
        
        HideWindowInternal().Forget();
    }

    private async UniTaskVoid HideWindowInternal()
    {
#pragma warning disable CS4014
        LMotion.Create(Vector3.one, Vector3.zero, Time)
            .WithEase(outEase)
            .WithOnComplete(() => gameObject.SetActive(false))
            .BindToLocalScale(transform);
#pragma warning restore CS4014
        await LMotion.Create(Vector3.zero, new Vector3(-90, 0, 0), Time)
            .WithEase(outEase)
            .BindToLocalEulerAngles(transform);

        CurrentlyDoingAnimation = false;
    }
}