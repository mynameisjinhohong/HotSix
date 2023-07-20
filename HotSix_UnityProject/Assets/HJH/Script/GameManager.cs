using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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
}
public class GameManager : MonoBehaviour
{
    
    public static GameManager instance = null;
    public int stage = 0;
    
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

    public TextAsset unitDatabase;
    public List<Unit_MJW> unitDataList;
    public UserInfo_MJW userInfo;
    public Deck_MJW currentDeck;

    public UnitPrefabManager_MJW unitPrefabManager;
    private string filePath;

    #endregion


    #region methods_MJW

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
        string[] line = unitDatabase.text.Substring(0, unitDatabase.text.Length - 1).Split('\n');
        unitDataList = new List<Unit_MJW>();
        for(int i = 0; i < line.Length; ++i){
            string[] row = line[i].Split('\t');

            unitDataList.Add(new Unit_MJW(int.Parse(row[0]), row[1], row[2], row[3], row[4], row[5],
                                        float.Parse(row[6]), float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10]), float.Parse(row[11]), int.Parse(row[12]), float.Parse(row[13]), 
                                        float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17]), float.Parse(row[18]), float.Parse(row[19]), int.Parse(row[20])));
        }
        unitPrefabManager.LinkPrefabs(unitDataList);

        LoadData();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        string data = PlayerPrefs.GetString("UserData");
        if (userData != null)
        {
            userData = JsonUtility.FromJson<UserData_HJH>(data);
        }
        else
        {
            userData = new UserData_HJH(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        //SaveUserData();
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
        string data = JsonUtility.ToJson(userData);
        // Debug.Log(data);
        PlayerPrefs.SetString("UserData", data);
    }
}
