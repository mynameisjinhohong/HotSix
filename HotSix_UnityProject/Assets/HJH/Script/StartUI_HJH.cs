using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI_HJH : MonoBehaviour
{
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        audio.Play();
        Invoke("MoveScene", 0.1f);
    }
    public void MoveScene()
    {
        SceneManager.LoadScene("StageScene");
    }
}
