using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserDataTest_HJH : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMP_Text>().text = PlayerPrefs.GetString("UserData");
    }
}
