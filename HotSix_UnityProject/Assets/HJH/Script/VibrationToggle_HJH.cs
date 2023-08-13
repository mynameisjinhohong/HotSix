using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class VibrationToggle_HJH : MonoBehaviour
{
    public Sprite[] toggleImage;
    public Button onOffToggle;

    // Start is called before the first frame update
    void Start()
    {
        onOffToggle.onClick.AddListener(ChangeVibration);
    }

    // Update is called once per frame
    public void ChangeVibration()
    {
        if (GameManager.instance.userData.vibration)
        {
            GameManager.instance.userData.vibration = false;
            onOffToggle.gameObject.GetComponent<Image>().sprite = toggleImage[0];
        }
        else
        {
            GameManager.instance.userData.vibration=true;
            onOffToggle.gameObject.GetComponent<Image>().sprite = toggleImage[1];
        }
        GameManager.instance.SaveUserData();
    }
}
