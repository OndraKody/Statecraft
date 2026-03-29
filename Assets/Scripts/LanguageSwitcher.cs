using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
    private bool isChangingLanguage = false;

    // Tuto funkci napojíme na obì tlaèítka
    public void SetLanguage(int languageIndex)
    {
        if (isChangingLanguage) return;
        StartCoroutine(ChangeLocale(languageIndex));
    }

    private IEnumerator ChangeLocale(int index)
    {
        isChangingLanguage = true;

        // Poèkáme, než se celý lokalizaèní systém naète (aby nedošlo k chybì)
        yield return LocalizationSettings.InitializationOperation;

        // Pøepneme jazyk na ten, který odpovídá èíslu z tlaèítka
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        isChangingLanguage = false;
    }
}