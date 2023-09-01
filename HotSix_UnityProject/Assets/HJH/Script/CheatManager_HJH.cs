using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager_HJH : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheatButton()
    {
        GameManager.instance.SaveData();
        if(PlayerPrefs.GetInt("Cheat",0) == 0)
        {
            string data = JsonUtility.ToJson(GameManager.instance.userData);
            PlayerPrefs.SetString("CheatData", data);
            GameManager.instance.userData.stageProgress = 12;
            for (int i = 0; i < GameManager.instance.userData.stageStar.Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GameManager.instance.userData.stageStar[i].stageStar[j] = true;
                }
            }
            PlayerPrefs.SetInt("Cheat", 1);
            GameManager.instance.SaveUserData();
        }
        else
        {
            PlayerPrefs.SetInt("Cheat", 0);
            UserData_HJH userdata = JsonUtility.FromJson<UserData_HJH>(PlayerPrefs.GetString("CheatData"));
            GameManager.instance.userData = userdata;
            GameManager.instance.SaveUserData();
        }
        GameManager.instance.LoadData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
