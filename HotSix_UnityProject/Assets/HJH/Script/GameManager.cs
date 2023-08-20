using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
#region Class_HJH
[System.Serializable]
public class UserData_HJH
{

    public int porfileImg = 0;
    public string userName = "nullName"; //유저 이름
    public int stageProgress = 0; // 스테이지 진행도
    public int winCount = 0; //승리 횟수
    public int loseCount = 0; //패배 횟수
    public float stageClearTime = 0; //스테이지 클리어 하는데 걸린 시간의 총합
    public int solveCount = 0; //푼 수학 문제 수
    public int tryCount = 0; //문제 풀이 시도 횟수
    public int mathCoinAmount = 0; //얻은 전체 메스 코인량
    public int userLevel = 0;
    public int langaugeSet = 1;
    public bool vibration = true;
    [Serializable]
    public class stageStars
    {
        public bool[] stageStar = new bool[3];
        public stageStars()
        {
            stageStar = new bool[3];
        }
    }
    public stageStars[] stageStar = new stageStars[13]; //스테이지 당 별
    public UserData_HJH() 
    {
        porfileImg = 0;
        userName = string.Empty;
        stageProgress = 0;
        winCount = 0;
        loseCount = 0;
        stageClearTime = 0;
        solveCount = 0;
        mathCoinAmount = 0;
        tryCount = 0;
        userLevel = 0;
        langaugeSet = 1;
        vibration = true;
        stageStar = new stageStars[13];
    }
}
[System.Serializable]
public class RewardData_HJH
{
    [Tooltip("별 개수에 따라서 카드를 몇개 주는지 설정, 위에서 부터 별 1,2,3개")]
    public int[] startCardAmount;
    [Tooltip("최초로 클리어 했을 때 별을 몇개 줄지")]
    public int firstClearCard;
    [Tooltip("체크를 하면 자신이 보유한 모든 유닛중에서 랜덤으로 부여")]
    public bool random; // 랜덤으로 주는지 하나는 확정인지
    [Tooltip("Random 변수가 체크해제시만 작동, 이 유닛 idx")]
    public int confirmedUnitIdx; //확정인 유닛 인덱스

}

[System.Serializable]
public class StarSystem_HJH
{
    [Tooltip("어떠한 조건으로 별을 제공할지 설정, 밑의 변수 순서대로 0,1,2")]
    public int[] whatIsCondition = new int[3]; //어떤 조건으로 할지
    [Tooltip("게임을 클리어 했을 때 별 제공")]
    public bool gameClear = false; //게임이 클리어 했을 때
    [Tooltip("이 변수 초 이하로 클리어 했을 때 별 제공")]
    public int gameClearTime = 0; // 시간 제한
    [Tooltip("이 변수 보다 적게 메스코인을 사용했을 때 별 제공")]
    public int mathCoinAmount = 0; // 사용한 돈 제한
}
[System.Serializable]
public class MapElement
{
    public enum Bg 
    {
        Bg1,
        Bg2,
        Bg3,
    }
    [Tooltip("스테이지 배경이 몇번째 것인지(숲 - 1,다리 - 2, 어둠 - 3)")]
    public Bg stageBG;
    [Tooltip("다리의 개수가 몇개인지(현재는 1,2만 적용가능)")]
    public int lineCount;
    public enum Length
    {
        Short,
        Middle,
        Long,
    }
    [Tooltip("다리의 길이가 어떤지")]
    public Length lineLength;
}
#endregion
public class GameManager : MonoBehaviour
{
    public MapElement[] mapElements;
    public List<StarSystem_HJH> starCondition;
    public List<RewardData_HJH> rewardData;
    public static GameManager instance = null;
    public int stage = 0;
    public Sprite questionImage;
    public Sprite[] starImage;

    public UserData_HJH userData;
    
    [SerializeField]
    float bgmVolume;
    [SerializeField]
    float soundEffectVolume;

    public GameState gameState;
    public enum GameState
    {
        GamePlay,
        GameStop,
    }

    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        { 
            bgmVolume = value; 
            if(bgmVolume < 0)
            {
                bgmVolume = 0;
            }
            else if(bgmVolume > 1)
            {
                bgmVolume = 1;
            }
            bgm.volume = bgmVolume;
        }
    }

    public float SoundEffectVolume
    {
        get
        {
            return soundEffectVolume;
        }
        set
        {
            soundEffectVolume = value;
            if (soundEffectVolume < 0)
            {
                soundEffectVolume = 0;
            }
            else if (soundEffectVolume > 1)
            {
                soundEffectVolume = 1;
            }
            for(int i = 0; i<soundEffects.Count; i++)
            {
                soundEffects[i].volume = soundEffectVolume;
            }
        }
    }

    public AudioSource bgm;
    public AudioClip[] bgmSources;

    public List<AudioSource> soundEffects;


    #region properties_MJW

    public TextAsset playerUnitDatabase;
    public TextAsset enemyUnitDatabase;
    public TextAsset specialUnitDatabase;
    
    public UnitTable playerUnitTable;
    public UnitTable enemyUnitTable;
    public SpecialUnitTable specialUnitTable;

    public SOUnitImages unitImages;
    public UserInfo_MJW userInfo;
    public Deck_MJW currentDeck;
    public int? currentStage = null;
    public int testInt = 0;

    public UnitPrefabManager_MJW unitPrefabManager;
    public StageDataManager_MJW stageDataManager;
    private string filePath;

    #endregion


    #region methods_MJW

    public void ParseUnitTable(TextAsset database, UnitTable table){
        string[] line = database.text.Substring(0, database.text.Length - 1).Split('\n');
        int i = 0;
        while(i < line.Length){
            string[] infoRow = line[i].Split('\t');
            string[] statRow = line[i + 1].Split('\t');
            string[] actionRow = line[i + 2].Split('\t');
            
            int id = int.Parse(infoRow[0]);
            int numAction = int.Parse(actionRow[0]);

            UnitData data = table.unitData[id];

            data.entityInfos.id = int.Parse(infoRow[0]);
            data.entityInfos.uClass = int.Parse(infoRow[1]);
            data.entityInfos.k_name = infoRow[2];
            data.entityInfos.e_name = infoRow[3];
            data.entityInfos.k_information = infoRow[4];
            data.entityInfos.e_information = infoRow[5];
            
            data.entityInfos.cost = int.Parse(infoRow[6]);
            data.entityInfos.cooldown = float.Parse(infoRow[7]);

            data.unitStats.maxHP = float.Parse(statRow[0]);
            data.unitStats.uMaxHP = float.Parse(statRow[1]);
            data.unitStats.defensive = float.Parse(statRow[2]);
            data.unitStats.uDefensive = float.Parse(statRow[3]);
            data.unitStats.moveSpeed = float.Parse(statRow[4]);
            
            data.attackAction = int.Parse(actionRow[1]);
            data.secondAction.index = int.Parse(actionRow[2]);
            data.secondAction.k_name = actionRow[3];
            data.secondAction.e_name = actionRow[4];
            
            int j = 0;
            for(; j < numAction; ++j){
                string[] row = line[i + 3 + j].Split('\t');
                data.actionBehaviors[j].value = float.Parse(row[0]);
                data.actionBehaviors[j].upgradeValue = float.Parse(row[1]);
                data.actionBehaviors[j].range = float.Parse(row[2]);
                data.actionBehaviors[j].cooldown = float.Parse(row[3]);
            }

            table.unitData[id] = data;

            i += 3 + j;
        }
    }

    public void ParseSpecialUnitTable(TextAsset database, SpecialUnitTable table){
        string[] line = database.text.Substring(0, database.text.Length - 1).Split('\n');
        int i = 0;
        while(i < line.Length){
            string[] infoRow = line[i].Split('\t');
            string[] aInfoRow = line[i + 1].Split('\t');
            string[] actionRow = line[i + 2].Split('\t');
            
            int id = int.Parse(infoRow[0]);

            SpecialUnitData data = table.specialUnitData[id];

            data.entityInfos.id = int.Parse(infoRow[0]);
            data.entityInfos.uClass = int.Parse(infoRow[1]);
            data.entityInfos.k_name = infoRow[2];
            data.entityInfos.e_name = infoRow[3];
            data.entityInfos.k_information = infoRow[4];
            data.entityInfos.e_information = infoRow[5];
            data.entityInfos.cost = int.Parse(infoRow[6]);
            data.entityInfos.cooldown = float.Parse(infoRow[7]);

            data.action.index = 0;
            data.action.k_name = aInfoRow[0];
            data.action.e_name = aInfoRow[1];

            data.actionBehavior.value = float.Parse(actionRow[0]);
            data.actionBehavior.upgradeValue = float.Parse(actionRow[1]);
            data.actionBehavior.range = float.Parse(actionRow[2]);
            data.actionBehavior.cooldown = float.Parse(actionRow[3]);

            table.specialUnitData[id] = data;

            i += 3;
        }
    }

    public void InitData(){
        userInfo = new UserInfo_MJW();
        currentDeck = userInfo.GetSelectedDeck();
        SaveData();
        LoadData();
    }

    public void SaveData(){
        string jdata = JsonUtility.ToJson(userInfo);

        File.WriteAllText(filePath + "/UserData.txt", jdata);
    }

    public void LoadData(){
        if(!File.Exists(filePath + "/UserData.txt")){InitData(); return;}

        string jdata = File.ReadAllText(filePath + "/UserData.txt");
        userInfo = JsonUtility.FromJson<UserInfo_MJW>(jdata);
        currentDeck = userInfo.GetSelectedDeck();
    }

    #endregion

    private void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        bgmVolume = bgm.volume;

        SceneManager.sceneLoaded += OnSceneLoaded;

        filePath = Application.persistentDataPath;
        Debug.Log("" + filePath);

        // 전체 유닛 리스트 불러오기
        ParseUnitTable(playerUnitDatabase, playerUnitTable);
        ParseUnitTable(enemyUnitDatabase, enemyUnitTable);
        ParseSpecialUnitTable(specialUnitDatabase, specialUnitTable);
        unitPrefabManager.LinkPrefabs(playerUnitTable, enemyUnitTable, specialUnitTable);

        stageDataManager.ParseData();

        LoadData();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        string data = PlayerPrefs.GetString("UserData");
        if (data.Length >1)
        {
            userData = JsonUtility.FromJson<UserData_HJH>(data);
        }
        else
        {
            userData = new UserData_HJH();
            if (userData.stageStar[0] == null)
            {
                for (int i = 0; i < userData.stageStar.Length; i++)
                {
                    userData.stageStar[i] = new UserData_HJH.stageStars();
                }
            }
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[userData.langaugeSet];
        //Debug.Log(userData.porfileImg);
        //Debug.Log(LocalizationSettings.SelectedLocale.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //if (currentStage != null)
        //{
        //    currentStage = testInt;
        //}
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState = GameState.GamePlay;
        if(scene.name == "StageScene")
        {
            bgm.clip = bgmSources[0];
            bgm.Play();
        }
        else if(scene.name == "GameScene")
        {
            bgm.clip = bgmSources[1];
            bgm.Play();
        }
        FindAudioSource();

    }
    private void FindAudioSource()
    {
        List<AudioSource> audioSources = new List<AudioSource>();
        GameObject[] all = FindObjectsOfType<GameObject>();
        AudioSource myAudio = GetComponent<AudioSource>();
        foreach (GameObject obj in all)
        {
            AudioSource audio;
            if (obj.TryGetComponent<AudioSource>(out audio))
            {
                if (audio != myAudio)
                {
                    audioSources.Add(audio);
                    audio.volume = soundEffectVolume;
                }
            }
        }
        soundEffects = audioSources;
    }

    public void SaveUserData()
    {
        for(int i =0; i< LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i])
            {
                userData.langaugeSet = i;
            }
        }
        string data = JsonUtility.ToJson(userData,true);
        //Debug.Log(data);
        PlayerPrefs.SetString("UserData", data);
    }

    public void Vibrate()
    {
        if (userData.vibration)
        {
            Handheld.Vibrate();
        }
    }
}
