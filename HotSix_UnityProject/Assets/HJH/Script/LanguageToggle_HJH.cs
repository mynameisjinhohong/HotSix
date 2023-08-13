using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageToggle_HJH : MonoBehaviour
{
    public GameObject[] blackFilter;
    public Button[] buttons;

    void Start()
    {
        for(int i =0; i < blackFilter.Length; i++)
        {
            if(i == GameManager.instance.userData.langaugeSet)
            {
                blackFilter[i].SetActive(false);
            }
            else
            {
                blackFilter[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLanguage(int su)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[su];
        GameManager.instance.userData.langaugeSet = su;
        for(int i =0; i< blackFilter.Length; i++)
        {
            if(i == su)
            {
                blackFilter[i].SetActive(false);
            }
            else
            {
                blackFilter[i].SetActive(true);
            }
        }
        GameManager.instance.SaveUserData();
    }
}
