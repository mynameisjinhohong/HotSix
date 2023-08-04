using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_HJH : MonoBehaviour
{
    #region 보상시스템 + 게임 클리어에 쓰는 것륻
    public Image[] unitImages;
    public TMP_Text[] unitText;
    public TMP_Text[] timeText;
    public TMP_Text moneyText;
    #endregion

    public Slider bgmSlider;
    public Slider soundEffetcSlider;

    public GameObject settingPopup;
    public GameObject gameClearPopup;
    public GameObject gameOverPopup;
    public GameObject restartPopup;

    public Animator gameClearAni;
    public Animator gameOverAni;

    public LaneSpawnManager_MJW laneManager;
    public EnemySpawnManager_MJW enemySpawnManager;

    public AudioSource buttonAudio;
    public AudioSource gameClearAudio;
    public AudioSource gameOverAudio;

    public MoneyManager_HJH moneyManager;
    public MathProblem_HJH mathProblem;



    #region �÷��̾� ���� ���忡 �ʿ��� �͵�
    bool gamePlay = false; //�÷��� �ϴ� ���ȸ� �ð�����
    float playTime = 0; //���� �÷��� �� �ð�
    bool gameEnd = false; //���� Ŭ���� �Լ� �ѹ��� ����
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
        bool firstClear = true;
        if (GameManager.instance.userData.stageProgress < GameManager.instance.stage)
        {
            firstClear = true;
        }
        else
        {
            firstClear=false;
        }
        UserDataUpdate(true);
        int time = Mathf.CeilToInt((GameManager.instance.userData.stageClearTime / (float)(GameManager.instance.userData.winCount)));
        int min = time / 60;
        int sec = time % 60;
        timeText[0].text = min.ToString();
        timeText[1].text = sec.ToString();
        moneyText.text = (moneyManager.moneyAmount - moneyManager.money).ToString();
        CheckReward(firstClear);

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
        for (int i = 0; i < laneManager.lanes.Length; i++)
        {
            Transform[] allChildren = laneManager.lanes[i].GetComponentsInChildren<Transform>();
            foreach(Transform child in allChildren){
                if(child.CompareTag("Unit")){
                    child.gameObject.GetComponent<Unit>().isActive = false;
                }
                else if(child.CompareTag("Projectile")){
                    child.gameObject.GetComponent<Projectile>().isActive = false;
                }
            }
            enemySpawnManager.isActive = false;
        }
    }

    public void UserDataUpdate(bool win)
    {
        mathProblem.SaveData();
        if (win)
        {
            GameManager.instance.userData.winCount += 1;
            GameManager.instance.userData.stageClearTime += playTime;
            if(GameManager.instance.userData.stageProgress < GameManager.instance.stage)
            {
                GameManager.instance.userData.stageProgress = GameManager.instance.stage;
            }
        }
        else
        {
            GameManager.instance.userData.loseCount += 1;
        }
        GameManager.instance.userData.mathCoinAmount += moneyManager.moneyAmount - moneyManager.money;
        GameManager.instance.SaveUserData();
    }
    #region 보상 시스템
    public void CheckReward(bool firstClear)
    {
        int stage = GameManager.instance.stage;
        RewardData_HJH reward = GameManager.instance.rewardData[stage];
        UserInfo_MJW unitInfo = GameManager.instance.userInfo;
        List<int> unitList = new List<int>();
        List<int> countList = new List<int>();
        //별 받아와서 저 [2]에다가 별 숫자 -1 해서 넣어줄 것
        int maxCount = reward.startCardAmount[2];
        if (firstClear)
        {
            maxCount += reward.firstClearCard;
        }
        if (!reward.random)
        {
            unitList.Add(reward.confirmedUnitIdx);
        }
        while (unitList.Count < 3)
        {
            int unit = Random.Range(1, unitInfo.userUnitInfo.Count);
            if (!unitList.Contains(unit))
            {
                if (unitInfo.userUnitInfo[unit].level > 0)
                {
                    unitList.Add(unit);
                }
            }
        }
        for (int i = 0; i < 2; i++)
        {
            int ran = 0;
            if(maxCount > 2)
            {
                ran = Random.Range(1, maxCount - 1);
            }
            else
            {
                ran = 1;
            }
            countList.Add(ran);
            maxCount -= ran;
        }
        countList.Add(maxCount);
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i + ": " + countList[i]);
            unitImages[i].sprite = GameManager.instance.unitImage[unitList[i]];
            unitText[i].text = "X " + countList[i];
            GameManager.instance.userInfo.userUnitInfo[unitList[i]].number += countList[i];
        }
        GameManager.instance.SaveData();
    }

    #endregion

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
