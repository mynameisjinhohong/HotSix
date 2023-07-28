using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageToggle_HJH : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle[] toggles;
    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i < toggles.Length; i++)
        {
            toggles[i].onValueChanged.AddListener((value) => { ChangeLanguage(value); });
        }
        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            toggles[1].isOn = true;
        }
        else
        {
            toggles[0].isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLanguage(bool bo)
    {
        Toggle to = toggleGroup.ActiveToggles().FirstOrDefault();
        for(int i = 0;i < toggles.Length;i++)
        {
            if(to == toggles[i])
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
            }
        }
    }
}
