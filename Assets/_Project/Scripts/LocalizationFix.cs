using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationFix : MonoBehaviour
{
    private void Start()
    {
        StartAsync().Forget();
    }

    private async UniTask StartAsync()
    {
        Locale prevLocale = LocalizationSettings.SelectedLocale;

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].name == "Arabic (ar)")
            {
                await UniTask.WaitForSeconds(0.1f);
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
            }
        }

        await UniTask.WaitForSeconds(0.1f);
        LocalizationSettings.SelectedLocale = prevLocale;
    }
}