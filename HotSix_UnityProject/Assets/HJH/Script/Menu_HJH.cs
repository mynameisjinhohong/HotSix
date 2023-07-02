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

    public Animator gameClearAni;
    public Animator gameOverAni;

    public Transform laneManager;
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

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
    }
    public void GoHome()
    {
        Time.timeScale = 1f;
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
        SceneManager.LoadScene("StageScene");
    }

    public void GameClear()
    {
        GameManager.instance.gameState = GameManager.GameState.GameStop;
        gameClearPopup.SetActive(true);
        Invincible();
        gameClearAni.SetTrigger("GameClear");
        
    }
    public void GameOver()
    {
        GameManager.instance.gameState = GameManager.GameState.GameStop;
        Invincible();
        gameOverPopup.SetActive(true);
        //추후에 애니메이션 추가할것
    }

    public void Invincible() //모든 유닛 무적
    {
        for(int i = 0; i<laneManager.childCount; i++) 
        {
            for(int j = 0; j< laneManager.GetChild(i).childCount; j++)
            {
                laneManager.GetChild(i).GetChild(j).gameObject.GetComponent<Unit_MJW>().unitStat.attackDamage = 0;
            }
        }
    }
}
