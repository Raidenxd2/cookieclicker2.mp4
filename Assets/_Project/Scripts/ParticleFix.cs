using UnityEngine;

public class ParticleFix : MonoBehaviour
{
    void OnEnable()
    {
        var ParticleSystem = gameObject.GetComponent<ParticleSystem>();
        ParticleSystem.Play();
    }
}
