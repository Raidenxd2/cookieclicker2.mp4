using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = color;
    }
}