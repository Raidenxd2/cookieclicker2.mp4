using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexutresToggle : MonoBehaviour
{
    
    Renderer Rrenderer;
    public Texture blankTex;
    Texture orinigalTex;
    public AdvancedQualitySettings ad;
    public int TextureIndex;


    void Awake()
    {
        Rrenderer = gameObject.GetComponent<MeshRenderer>();
        orinigalTex = Rrenderer.materials[TextureIndex].GetTexture("_MainTex");
        StartCoroutine(Wait());
        //renderer.material.SetTexture("_MainTex", blankTex);
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        if (ad.Textures)
        {
            Rrenderer.sharedMaterials[TextureIndex].mainTexture = orinigalTex;
            Debug.Log("true");
        }
        else
        {
            Rrenderer.sharedMaterials[TextureIndex].mainTexture = blankTex;
            Debug.Log("false");
        }
    }
}
