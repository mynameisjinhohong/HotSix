using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileButton_HJH : MonoBehaviour
{
    public AudioSource audio;
    public GameObject proFile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
