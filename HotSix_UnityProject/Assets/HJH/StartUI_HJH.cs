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
        int tuto = PlayerPrefs.GetInt("Tutorial", 0);
        if (tuto == 0)
        {
            SceneManager.LoadScene("StageScene");
            // GameManager.instance.currentStage = 0;
            // zSceneManager.LoadScene("TutorialScene");

        }
        else
        {
            SceneManager.LoadScene("StageScene");
        }
    }
}
