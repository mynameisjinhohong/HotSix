using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileButton_HJH : MonoBehaviour
{
    public AudioSource audio;
    public GameObject proFile;
    public void MoveProfileScene()
    {
        audio.Play();
        Invoke("MoveScene",0.01f);
    }

    public void TurnOnProfile()
    {
        audio.Play();
        proFile.SetActive(true);
    }

    public void MoveScene()
    {
        LoadingManager_HJH.LoadScene("ProfileScene");

    }
}
