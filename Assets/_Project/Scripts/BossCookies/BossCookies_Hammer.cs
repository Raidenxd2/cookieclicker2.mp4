using UnityEngine;

public class BossCookies_Hammer : MonoBehaviour
{
#if !CC2_DISABLEBOSSCOOKIESVRMODE
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitArea"))
        {
            other.GetComponentInParent<BossCookieObject>().HitCookie();
        }
    }
#endif
}