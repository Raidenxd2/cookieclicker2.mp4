using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using TMPro;

public class LocaleDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _localesDropdown;
 
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        List<TMP_Dropdown.OptionData> options = new();
        int selectedLocale = 0;
        
        for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].name != "cc2 localization fix (cc2_localization_fix)")
            {
                Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
                options.Add(new TMP_Dropdown.OptionData(locale.name));
                if (locale == LocalizationSettings.SelectedLocale) selectedLocale = i;
            }
        }
        
        _localesDropdown.options = options;
        _localesDropdown.value = selectedLocale;
        _localesDropdown.onValueChanged.AddListener(OnLocaleChanged);
    }

    private static void OnLocaleChanged(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}