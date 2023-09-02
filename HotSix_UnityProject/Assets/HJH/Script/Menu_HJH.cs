using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_HJH : MonoBehaviour
{
    #region reward + gameClear + gameOver
    public Image[] unitImages;
    public TMP_Text[] unitText;
    public Sprite[] conditionImages;
    public Image condition1Img;
    public Image condition2Img;
    public GameObject[] condition1Clear;
    public GameObject[] condition2Clear;
    public TMP_Text condition2Text;
    public TMP_Text condition1Text;
    public TMP_Text stageText;
    public TMP_Text gameOverText;
    public string[] gameOverStringsKor;
    public string[] gameOverStringsEng;
    public SettingsWindow clearAni;
    public SettingsWindow overAni;
    #endregion
    #region StarSystem
    public Image[] stars;
    #endregion
    public Slider bgmSlider;
    public Slider soundEffetcSlider;

    public GameObject settingPopup;
    public GameObject gameClearPopup;
    public GameObject gameOverPopup;
    public GameObject restartPopup;

    public LaneSpawnManager_MJW laneManager;
    public EnemySpawnManager_MJW enemySpawnManager;

    public AudioSource buttonAudio;
    public AudioSource gameClearAudio;
    public AudioSource gameOverAudio;

    public MoneyManager_HJH moneyManager;
    public TowerHPManager_HJH hpManager;
    public MathProblem_HJH mathProblem;

    public BossManager_HJH boss;

    public TutorialManager_HJH tutorialManager;

    #region 엔딩
    public GameObject endingCanvas;
    #endregion

    #region 타이머
    public TMP_Text timer;
    #endregion

    #region �÷��̾� ���� ���忡 �ʿ��� �͵�
    bool gamePlay = false;
    float playTime = 0;
    bool gameEnd = false;
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
            if (timer != null)
            {
                timer.text = ((int)playTime / 60).ToString() + " : " + ((int)playTime % 60).ToString();
            }
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
        StartCoroutine(MoveScene(SceneManager.GetActiveScene().name, 0.01f));
    }

    IEnumerator MoveScene(string sceneName, float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        LoadingManager_HJH.LoadScene(sceneName);
    }
    public void GoHome()
    {
        Time.timeScale = 1f;
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
        GameManager.instance.currentStage = null;
        buttonAudio.Play();
        StartCoroutine(MoveScene("StageScene", 0.01f));
    }

    public void NextStage()
    {
        Time.timeScale = 1f;
        GameManager.instance.gameState = GameManager.GameState.GamePlay;
        GameManager.instance.currentStage++;
        GameManager.instance.stage++;
        if(GameManager.instance.stage > 12)
        {
            GameManager.instance.stage = 12;
            GameManager.instance.currentStage = 12;
            StartCoroutine(MoveScene("StageScene", 0.01f));
        }
        else
        {
            StartCoroutine(MoveScene("GameScene", 0.01f));
        }
        buttonAudio.Play();
    }

    public void GameClear()
    {
        if (gameEnd == true)
        {
            return;
        }
        if (GameManager.instance.stage == 12 && !GameManager.instance.userData.ending)
        {
            GameManager.instance.userData.ending = true;
            endingCanvas.gameObject.SetActive(true);
        }
        else
        {
            gameClearAudio.Play();
        }
        clearAni.Open();
        gameEnd = true;
        gamePlay = false;
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            stageText.text = "Stage " + GameManager.instance.stage;
        }
        gameClearPopup.SetActive(true);
        Invincible();
        bool firstClear = true;
        if (GameManager.instance.userData.stageProgress < GameManager.instance.stage)
        {
            firstClear = true;
        }
        else
        {
            firstClear = false;
        }
        int star = CheckStar();
        CheckReward(star, firstClear);
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
        overAni.Open();
        Invincible();
        int ran = Random.Range(0, gameOverStringsKor.Length);
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
        {
            gameOverText.text = gameOverStringsKor[ran];
        }
        else
        {
            gameOverText.text = gameOverStringsEng[ran];
        }
        gameOverAudio.Play();
        gameOverPopup.SetActive(true);
        UserDataUpdate(false);
        GameManager.instance.gameState = GameManager.GameState.GameStop;
    }

    public void Invincible()
    {
        laneManager.isActive = false;
        enemySpawnManager.isActive = false;
        StopAllCoroutines();
    }

    public void UserDataUpdate(bool win)
    {
        mathProblem.SaveData();
        if (win)
        {
            GameManager.instance.userData.winCount += 1;
            GameManager.instance.userData.stageClearTime += playTime;
            if (GameManager.instance.userData.stageProgress < GameManager.instance.stage)
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
    #region StarSystem
    public int CheckStar()
    {
        int star = 0;
        int stage = (int)GameManager.instance.stage;
        if (stage == 0)
        {
            TutorialStar();
            return 3;
        }
        for (int i = 0; i < 3; i++)
        {
            int beforeStar = star;
            switch (GameManager.instance.starCondition[stage].whatIsCondition[i])
            {
                case 0:
                    star++;
                    break;
                case 1:
                    int time = Mathf.CeilToInt(playTime);
                    int min = time / 60;
                    int sec = time % 60;
                    if(i == 1)
                    {
                        condition1Img.sprite = conditionImages[0];
                        condition1Text.text = min.ToString() + " : " + sec.ToString();
                    }
                    else if(i == 2)
                    {
                        condition2Img.sprite = conditionImages[0];
                        condition2Text.text = min.ToString() + " : " + sec.ToString();
                    }
                    if (playTime < GameManager.instance.starCondition[stage].gameClearTime)
                    {
                        star++;
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(true);
                            condition1Clear[1].SetActive(false);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(true);
                            condition2Clear[1].SetActive(false);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(false);
                            condition1Clear[1].SetActive(true);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(false);
                            condition2Clear[1].SetActive(true);
                        }
                    }
                    break;
                case 2:
                    if (i == 1)
                    {
                        condition1Img.sprite = conditionImages[1];
                        condition1Text.text = (moneyManager.moneyAmount - moneyManager.money).ToString();
                    }
                    else if (i == 2)
                    {
                        condition2Img.sprite = conditionImages[1];
                        condition2Text.text = (moneyManager.moneyAmount - moneyManager.money).ToString();
                    }
                    if (moneyManager.moneyAmount - moneyManager.money < GameManager.instance.starCondition[stage].mathCoinAmount)
                    {
                        star++;
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(true);
                            condition1Clear[1].SetActive(false);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(true);
                            condition2Clear[1].SetActive(false);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(false);
                            condition1Clear[1].SetActive(true);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(false);
                            condition2Clear[1].SetActive(true);
                        }
                    }
                    break;
                case 3:
                    if (i == 1)
                    {
                        condition1Img.sprite = conditionImages[2];
                        condition1Text.text = (Mathf.Floor((float)hpManager.playerTowerHP / (float)hpManager.playerMaxHP * 1000) / 10f).ToString() + "%";
                    }
                    else if (i == 2)
                    {
                        condition2Img.sprite = conditionImages[2];
                        condition2Text.text = (Mathf.Floor((float)hpManager.playerTowerHP / (float)hpManager.playerMaxHP * 1000) / 10f).ToString() + "%";
                    }
                    if (Mathf.Floor((float)hpManager.playerTowerHP / (float)hpManager.playerMaxHP * 1000)/10f > GameManager.instance.starCondition[stage].playrTowerHpPercentage)
                    {
                        star++;
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(true);
                            condition1Clear[1].SetActive(false);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(true);
                            condition2Clear[1].SetActive(false);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(false);
                            condition1Clear[1].SetActive(true);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(false);
                            condition2Clear[1].SetActive(true);
                        }
                    }
                    break;
                case 4:
                    if (i == 1)
                    {
                        condition1Img.sprite = conditionImages[3];
                        condition1Text.text = (Mathf.Floor((float)mathProblem.correctCount / (float)mathProblem.tryCount * 1000) / 10).ToString() + "%";
                    }
                    else if (i == 2)
                    {
                        condition2Img.sprite = conditionImages[3];
                        condition2Text.text = (Mathf.Floor((float)mathProblem.correctCount / (float)mathProblem.tryCount * 1000) / 10).ToString() + "%";
                    }
                    if ((Mathf.Floor((float)mathProblem.correctCount / (float)mathProblem.tryCount * 1000)/10) >= GameManager.instance.starCondition[stage].mathProblemCorrectness)
                    {
                        star++;
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(true);
                            condition1Clear[1].SetActive(false);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(true);
                            condition2Clear[1].SetActive(false);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(false);
                            condition1Clear[1].SetActive(true);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(false);
                            condition2Clear[1].SetActive(true);
                        }
                    }
                    break;
                case 5:
                    if (i == 1)
                    {
                        condition1Img.sprite = conditionImages[4];
                        condition1Text.text = (hpManager.towerLevel + 1).ToString();
                    }
                    else if (i == 2)
                    {
                        condition2Img.sprite = conditionImages[4];
                        condition2Text.text = (hpManager.towerLevel + 1).ToString();
                    }
                    if (hpManager.towerLevel + 1 <= GameManager.instance.starCondition[stage].playerTowerUpgrade)
                    {
                        star++;
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(true);
                            condition1Clear[1].SetActive(false);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(true);
                            condition2Clear[1].SetActive(false);
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            condition1Clear[0].SetActive(false);
                            condition1Clear[1].SetActive(true);
                        }
                        else if (i == 2)
                        {
                            condition2Clear[0].SetActive(false);
                            condition2Clear[1].SetActive(true);
                        }
                    }
                    break;
            }
            if (star != beforeStar)
            {
                GameManager.instance.userData.stageStar[stage].stageStar[i] = true;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (i < star)
            {
                stars[i].sprite = GameManager.instance.starImage[1];
            }
            else
            {
                stars[i].sprite = GameManager.instance.starImage[0];
            }
        }
        return star;
    }

    private void TutorialStar()
    {
        for (int i = 0; i < 3; i++)
        {
            stars[i].sprite = GameManager.instance.starImage[1];
        }
        int time = Mathf.CeilToInt(playTime);
        int min = time / 60;
        int sec = time % 60;
        condition1Text.text = min.ToString() + " : " + sec.ToString();
        condition2Text.text = (moneyManager.moneyAmount - moneyManager.money).ToString();
    }
    #endregion
    #region Reward System
    public void CheckReward(int star, bool firstClear)
    {
        int stage = GameManager.instance.stage;
        if (stage == 0)
        {
            TutorialReward();
            return;
        }
        RewardData_HJH reward = GameManager.instance.rewardData[stage];
        UserInfo_MJW unitInfo = GameManager.instance.userInfo;
        List<int> unitList = new List<int>();
        List<int> countList = new List<int>();
        int maxCount = reward.startCardAmount[star - 1];
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
            if (maxCount > 2)
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
            unitText[i].text = "X " + countList[i];
            if (unitList[i] > 8)
            {
                int test = unitList[i] - 8;
                unitImages[i].sprite = GameManager.instance.unitImages.specialUnitImages[test].nomal_Icon;
                if(GameManager.instance.userInfo.userSpecialUnitInfo[test].level < 1)
                {
                    GameManager.instance.userInfo.userSpecialUnitInfo[test].level++;
                }
                GameManager.instance.userInfo.userSpecialUnitInfo[test].number += countList[i];
            }
            else
            {
                unitImages[i].sprite = GameManager.instance.unitImages.playerUnitImages[unitList[i]].nomal_Icon;
                if (GameManager.instance.userInfo.userUnitInfo[unitList[i]].level < 1)
                {
                    GameManager.instance.userInfo.userUnitInfo[unitList[i]].level++;
                }
                GameManager.instance.userInfo.userUnitInfo[unitList[i]].number += countList[i];
            }
        }
        GameManager.instance.SaveData();
    }

    #endregion

    #region 튜토리얼
    public void TutorialReward()
    {
        GameManager.instance.userInfo.userUnitInfo[3].number += 5;
        GameManager.instance.SaveData();
    }

    public void TutorialEnd()
    {
        tutorialManager.gameClear = true;
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
