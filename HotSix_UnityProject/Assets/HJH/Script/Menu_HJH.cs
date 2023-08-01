using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_HJH : MonoBehaviour
{
    [System.Serializable]
    public class RewardData_HJH
    {
        public int cardAmount; // 주는 카드의 수
        public bool random; // 랜덤으로 주는지 하나는 확정인지
        public int confirmedUnitIdx;
    }

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

    public List<RewardData_HJH> rewardData;

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
                laneManager.GetChild(i).GetChild(j).gameObject.GetComponent<Unit>().curStat.attackDamage = 0;
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
            if(GameManager.instance.userData.stageProgress < GameManager.instance.stage)
            {
                GameManager.instance.userData.stageProgress = GameManager.instance.stage;
            }
        }
        else
        {
            GameManager.instance.userData.loseCount += 1;
        }
        GameManager.instance.userData.mathCoinAmount += moneyManager.money;
        GameManager.instance.SaveUserData();
    }
    #region 보상 시스템
    public void CheckReward()
    {
        int stage = GameManager.instance.stage;
        RewardData_HJH reward = rewardData[stage];
        if(reward.random)
        {
            EverythingRandom(reward.cardAmount);
        }
        else
        {

        }
    }

    public void EverythingRandom(int count)
    {
        UserInfo_MJW unitInfo = GameManager.instance.userInfo;
        List<int> unitList = new List<int>();
        List<int> countList = new List<int>();
        int maxCount = count;
        while(unitList.Count < 3)
        {
            int unit = Random.Range(1, unitInfo.userUnitInfo.Count);
            if (unitList.Contains(unit))
            {
                return;
            }
            else
            {
                unitList.Add(unit);
            }
        }
        for(int i =0; i<2; i++)
        {
            int ran = Random.Range(1, maxCount - 1);
            countList.Add(ran);
            maxCount -= ran;
        }
        countList.Add(maxCount);
        
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
