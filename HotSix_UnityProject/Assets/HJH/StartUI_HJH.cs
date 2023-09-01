using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI_HJH : MonoBehaviour
{
    public GameObject quitPopUp;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            quitPopUp.SetActive(true);
        }
    }

    public void StartButton()
    {
        audio.Play();
        Invoke("MoveScene", 0.01f);
    }
    public void MoveScene()
    {
        int tuto = PlayerPrefs.GetInt("Tutorial", 0);
        if (tuto == 0)
        {
            GameManager.instance.currentStage = 0;
            string filePath = Application.persistentDataPath;
            System.IO.File.Delete(filePath + "/UserData.txt");
            GameManager.instance.InitData(false);
            LoadingManager_HJH.LoadScene("TutorialScene");
        }
        else
        {
            LoadingManager_HJH.LoadScene("StageScene");
        }
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
