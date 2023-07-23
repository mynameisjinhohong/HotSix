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

    public MoneyManager_HJH moneyManager;
    public MathProblem_HJH mathProblem;

    #region 플레이어 정보 저장에 필요한 것들
    bool gamePlay = false; //플레이 하는 동안만 시간측정
    float playTime = 0; //게임 플레이 한 시간
    bool gameEnd = false; //게임 클리어 함수 한번만 실행
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        gameEnd = false;
        gamePlay = true;
        bgmSlider.onValueChanged.AddListener(BgmChange);
        soundEffetcSlider.onValueChanged.AddListener(SoundEffectChange);
        bgmSlider.value = GameManager.instance.BgmVolume;
        soundEffetcSlider.value = GameManager.instance.SoundEffectVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePlay)
        {
            playTime += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "GameScene")
        {
            MenuButton();
        }
    }
    public void MenuButton()
    {
        if (settingPopup.activeInHierarchy)
        {
            Time.timeScale = 1f;
            gamePlay = true;
            settingPopup.SetActive(false);
            GameManager.instance.gameState = GameManager.GameState.GamePlay;
        }
        else
        {
            gamePlay = false;
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
        StartCoroutine(MoveScene(SceneManager.GetActiveScene().name, 0.1f));
    }

    IEnumerator MoveScene(string sceneName, float waitSeconds)
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
        if (gameEnd == true)
        {
            return;
        }
        gameEnd = true;
        gamePlay = false;

        gameClearPopup.SetActive(true);
        Invincible();
        gameClearAudio.Play();
        gameClearAni.SetTrigger("GameClear");
        UserDataUpdate(true);

        GameManager.instance.gameState = GameManager.GameState.GameStop;
    }
    public void GameOver()
    {
        if (gameEnd == true)
        {
            return;
        }
        gameEnd = true;
        gamePlay = false;
        Invincible();
        gameOverAudio.Play();
        gameOverPopup.SetActive(true);
        UserDataUpdate(false);
        GameManager.instance.gameState = GameManager.GameState.GameStop;
    }

    public void Invincible()
    {
        for (int i = 0; i < laneManager.childCount; i++)
        {
            for (int j = 0; j < laneManager.GetChild(i).childCount; j++)
            {
                laneManager.GetChild(i).GetChild(j).gameObject.GetComponent<UnitObject_MJW>().currentStat.attackDamage = 0;
            }
        }
    }

    public void UserDataUpdate(bool win)
    {
        mathProblem.SaveData();
        if (win)
        {
            GameManager.instance.userData.winCount += 1;
            GameManager.instance.userData.stageClearTime += playTime;
            GameManager.instance.userData.stageProgress = GameManager.instance.stage;
        }
        else
        {
            GameManager.instance.userData.loseCount += 1;
        }
        GameManager.instance.userData.mathCoinAmount += moneyManager.money;
        GameManager.instance.SaveUserData();
    }

    public void TurnOnMenuButton(GameObject menuPopUp)
    {
        buttonAudio.Play();
        menuPopUp.SetActive(true);
    }
    public void TurnOffMenuButton(GameObject menuPopUp)
    {
        buttonAudio.Play();
        menuPopUp.SetActive(false);
    }
}
