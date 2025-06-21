using UnityEngine;

public class CookieVFX : MonoBehaviour
{
    void OnEnable()
    {
        ParticleSystem VFX = gameObject.GetComponent<ParticleSystem>();
#pragma warning disable CS0618
        float totalDuration = VFX.duration + VFX.startLifetime;
#pragma warning restore CS0618
        Destroy(gameObject, totalDuration);
    }
}