using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizationDropDown : MonoBehaviour
{
    private Dropdown dropdown;
    private string identifier = "en";
    List<string> options = new List<string> { "en", "fr", "es" };
    void Start()
    {
        dropdown = GetComponent<Dropdown>();

        dropdown.ClearOptions();

        dropdown.AddOptions(options);


        dropdown.onValueChanged.AddListener(updateLanguage);
    }

    private void updateLanguage(int arg0)
    {
        identifier = dropdown.options[arg0].text;

        LoadLocale(identifier);
    }

    public void LoadLocale(string languageIdentifier)
    {
        LocalizationSettings settings = LocalizationSettings.Instance;
        LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);//can be "en" "de" "ja" etc.
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
            LocaleIdentifier anIdentifier = aLocale.Identifier;
            if (anIdentifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = aLocale;
            }
        }
    }
}
