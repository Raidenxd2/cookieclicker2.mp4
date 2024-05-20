using UnityEngine;

public class CookieVFX : MonoBehaviour
{
    void OnEnable()
    {
        ParticleSystem VFX = gameObject.GetComponent<ParticleSystem>();
        float totalDuration = VFX.duration + VFX.startLifetime;
        Destroy(gameObject, totalDuration);
    }
}