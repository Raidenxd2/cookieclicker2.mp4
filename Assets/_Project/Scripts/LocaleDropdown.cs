using UnityEngine;
using TMPro;

public class LocaleDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _localesDropdown;

    [SerializeField] private GameObject RestartRequiredScreen;
 
    private void Start()
    {
        _localesDropdown.onValueChanged.AddListener(OnLocaleChanged);
        
        _localesDropdown.value = PlayerPrefs.GetInt("BeanLocalization_CurrentLanguage", 0);
    }

    private void OnLocaleChanged(int index)
    {
        Debug.Log(index);
        PlayerPrefs.SetInt("BeanLocalization_CurrentLanguage", index);
        
        RestartRequiredScreen.SetActive(true);
    }
}