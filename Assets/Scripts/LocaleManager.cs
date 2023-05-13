using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleManager : MonoBehaviour
{

    public int currentLoacle;

    public void ChangeLanguage(int index)
    {
        switch (index)
        {
            case 0:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                PlayerPrefs.SetInt("locale", 0);
                break;
            case 1:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
                PlayerPrefs.SetInt("locale", 1);
                break;
            case 2:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[3];
                PlayerPrefs.SetInt("locale", 2);
                break;
            case 3:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                PlayerPrefs.SetInt("locale", 3);
                break;
        }
    }

    void Start()
    {
        StartCoroutine(ChangeLanguage());
    }

    IEnumerator ChangeLanguage()
    {
        yield return new WaitForSeconds(1);
        currentLoacle = PlayerPrefs.GetInt("locale", 0);
        ChangeLanguage(currentLoacle);
        // LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLoacle];
    }
}
