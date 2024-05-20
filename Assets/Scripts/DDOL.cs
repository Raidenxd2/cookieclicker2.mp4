using UnityEngine;

public class DDOL : MonoBehaviour
{
    private static DDOL instance;

    void Awake() {
        if (instance != null) 
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}