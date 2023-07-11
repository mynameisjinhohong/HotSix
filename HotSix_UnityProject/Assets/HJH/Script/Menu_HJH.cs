using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_HJH : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider soundEffetcSlider;

    public GameObject settingPopup;
    public GameObject gameClearPopup;
    public GameObject gameOverPopup;
    public GameObject restartPopup;

    public Animator gameClearAni;
    public Animator gameOverAni;

    public Transform laneManager;

    public AudioSource buttonAudio;
    public AudioSource gameClearAudio;
    public AudioSource gameOverAudio;
    // Start is called before the first frame update
    void Start()
    {
        bgmSlider.onValueChanged.AddListener(BgmChange);
        soundEffetcSlider.onValueChanged.AddListener(SoundEffectChange);
        bgmSlider.value = GameManager.instance.BgmVolume;
        soundEffetcSlider.value = GameManager.instance.SoundEffectVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuButton();
        }
    }
    public void MenuButton()
    {
        if (settingPopup.activeInHierarchy)
        {
            Time.timeScale = 1f;
            settingPopup.SetActive(false);
            GameManager.instance.gameState = GameManager.GameState.GamePlay;
        }
        else
        {
            Time.timeScale = 0f;
            settingPopup.SetActive(true);
            GameManager.instance.gameState = GameManager.GameState.GameStop;
        }
    }

    void BgmChange(float value)
    {
        GameManager.instance.BgmVolume = value;
    }
    void SoundEffectChange(float value)
    {
        GameManager.instance.SoundEffectVolume = value;
    }

    public void RestartPopUp()
    {
        restartPopup.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
        buttonAudio.Play();
        StartCoroutine(MoveScene(SceneManager.GetActiveScene().name,0.1f));
    }

    IEnumerator MoveScene(string sceneName,float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        SceneManager.LoadScene(sceneName);
    }
    public void GoHome()
    {
        Time.timeScale = 1f;
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
        buttonAudio.Play();
        StartCoroutine(MoveScene("StageScene", 0.1f));
    }

    public void GameClear()
    {
        GameManager.instance.gameState = GameManager.GameState.GameStop;
        gameClearPopup.SetActive(true);
        Invincible();
        gameClearAudio.Play();
        gameClearAni.SetTrigger("GameClear");
        
    }
    public void GameOver()
    {
        GameManager.instance.gameState = GameManager.GameState.GameStop;
        Invincible();
        gameOverAudio.Play();
        gameOverPopup.SetActive(true);
    }

    public void Invincible()
    {
        for(int i = 0; i<laneManager.childCount; i++) 
        {
            for(int j = 0; j< laneManager.GetChild(i).childCount; j++)
            {
                laneManager.GetChild(i).GetChild(j).gameObject.GetComponent<UnitObject_MJW>().currentStat.attackDamage = 0;
            }
        }
    }
}
