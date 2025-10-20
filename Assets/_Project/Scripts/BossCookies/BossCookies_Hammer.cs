using UnityEngine;

public class BossCookies_Hammer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitArea"))
        {
            Debug.Log(other.name);
            other.GetComponentInParent<BossCookieObject>().HitCookie();
        }
    }
}