using SerialPackage.Runtime;
using TMPro;
using UnityEngine;

public class BeanLocalizationText : MonoBehaviour
{
    private TMP_Text text;
    
    public string KeyName;
    
    private void Start()
    {
#if UNITY_EDITOR
        BeanLocalization.LoadAsset();
#endif
        if (!text)
        {
#if UNITY_EDITOR
            BeanLogger.LogWarning("BeanLocalizationText.text was null!", this);
#endif
            
            text = GetComponent<TMP_Text>();
        }
        
        text.text = BeanLocalization.GetString(KeyName);
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        text = GetComponent<TMP_Text>();
        
        BeanLocalization.LoadAsset();
        Start();
    }
#endif
}