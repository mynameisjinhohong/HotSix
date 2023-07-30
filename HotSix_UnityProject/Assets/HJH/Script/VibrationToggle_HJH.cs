using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class VibrationToggle_HJH : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle[] toggles;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].onValueChanged.AddListener((value) => { ChangeVibration(value); });
        }
        if (GameManager.instance.userData.vibration)
        {
            toggles[0].isOn = true;
        }
        else
        {
            toggles[1].isOn = true;
        }
    }

    // Update is called once per frame
    public void ChangeVibration(bool bo)
    {
        Toggle to = toggleGroup.ActiveToggles().FirstOrDefault();
        for (int i = 0; i < toggles.Length; i++)
        {
            if (to == toggles[i])
            {
                if( i == 0)
                {
                    GameManager.instance.userData.vibration = true;
                }
                else
                {
                    GameManager.instance.userData.vibration = false;
                }
            }
        }
        GameManager.instance.SaveUserData();
    }
}
