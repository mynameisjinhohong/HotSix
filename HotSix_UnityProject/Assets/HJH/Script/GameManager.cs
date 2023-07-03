using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int stage = 0;
    [SerializeField]
    float bgmVolume;
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
            for(int i = 0; i<soundEffects.Length; i++)
            {
                soundEffects[i].volume = soundEffectVolume;
            }
        }
    }

    public AudioSource bgm;
    public AudioClip[] bgmSources;

    public AudioSource[] soundEffects;


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
                                        float.Parse(row[6]), float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10]), float.Parse(row[11]), int.Parse(row[12]),
                                        float.Parse(row[13]), float.Parse(row[14]), float.Parse(row[15]), float.Parse(row[16]), float.Parse(row[17]), float.Parse(row[18]), int.Parse(row[19])));
        }
        unitPrefabManager.LinkPrefabs(unitDataList);

        LoadData();
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState = GameState.GamePlay;
    }
}
