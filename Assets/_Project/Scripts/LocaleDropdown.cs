using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using TMPro;
using Cysharp.Threading.Tasks;

public class LocaleDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _localesDropdown;
 
    private void Start()
    {
        List<TMP_Dropdown.OptionData> options = new();
        int selectedLocale = 0;
        
        for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
            options.Add(new TMP_Dropdown.OptionData(locale.name));
            if (locale == LocalizationSettings.SelectedLocale) selectedLocale = i;
        }
        
        _localesDropdown.options = options;
        _localesDropdown.value = selectedLocale;
        _localesDropdown.onValueChanged.AddListener(OnLocaleChanged);
    }

    private static void OnLocaleChanged(int index)
    {
        Debug.Log(index);
        if (index == 3)
        {
            if (FontLoader.HasLoadedJapaneseFont)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            }
            else
            {
                FontLoader.instance.LoadJapaneseFont().Forget();
            }
            return;
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}