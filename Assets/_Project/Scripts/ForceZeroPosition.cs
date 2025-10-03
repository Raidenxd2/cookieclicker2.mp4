using UnityEngine;

public class ForceZeroPosition : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
}