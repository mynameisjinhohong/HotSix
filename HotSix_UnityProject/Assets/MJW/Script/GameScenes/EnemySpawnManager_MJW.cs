using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnManager_MJW : MonoBehaviour
{
    #region Properties

    [System.Serializable]
    public class Cycle{
        [System.Serializable]
        public class Pattern{
            public List<int> list;

            public Pattern(){
                list = new List<int>();
            }

            public string Print(){
                string str = "";
                foreach(int i in list){
                    str += i.ToString();
                    str += " ";
                }
                return str;
            }
        }

        public List<Pattern> list;

        public Cycle(){
            list = new List<Pattern>();
        }

        public string Print(){
            string str = "";
            for(int i = 0; i < list.Count; ++i){
                str += i + " ";
                str += list[i].Print();
                str += "\n";
            }
            return str;
        }
    }

    public GameManager gameManager;
    public LaneSpawnManager_MJW laneManager;
    public TextAsset enemySpawnPatternData;
    public TextAsset enemySpawnCycleData;

    private List<EnemySpawnPattern_MJW> patterns;
    private List<List<Cycle>> cycles;   // cycles[stage index][lane index].list[cycle index].list[pattern index]

    public bool isActive = false;
    public int currentStage;
    
    public List<int> repeatIndex;

    public List<bool> isFirst;
    public List<bool> isPatternOver;
    public List<bool> isCycleOver;

    #endregion


    #region Methods

    public void ParseData(){
        // 패턴 저장
        patterns = new List<EnemySpawnPattern_MJW>();
        string[] line = enemySpawnPatternData.text.Substring(0, enemySpawnPatternData.text.Length - 1).Split('\n');
        int i = 0;
        int patternIndex = 0;
        int temp;
        while(i < line.Length){
            string[] row1 = line[i].Split('\t');
            string[] row2 = line[i + 1].Split('\t');
            EnemySpawnData_MJW data = new EnemySpawnData_MJW();
            int patternCount = patterns.Count;

            if(patternCount <= patternIndex){   // 부족한 만큼 patterns 리스트 크기 증가
                for(int j = 0; j < patternIndex - patternCount + 1; ++j){
                    patterns.Add(new EnemySpawnPattern_MJW());
                }
            }

            if(row1.Length == 1 || row1[1] == "" || row1[1] == "\r"){
                temp = int.Parse(row1[0]);
                
                if(temp == 0){                  // 대기 시간 추가
                    data.AddUnit(0, 0);  
                }
                else{                           // 패턴 번호 확인
                    patternIndex = temp;
                    ++i;
                    continue;
                }
            }
            else{
                // 패턴에 노드 추가
                for(int j = 0; j < row1.Length; j += 2){
                    if(row1[j] == "" || row1[j] == "\r") break;
                    data.AddUnit(int.Parse(row1[j]), int.Parse(row1[j + 1]));
                }
            }
            
            data.totalTime = float.Parse(row2[0]);
            data.minTime = float.Parse(row2[1]);
            data.maxTime = float.Parse(row2[2]);
            
            patterns[patternIndex].Add(data);
            i += 2;
        }

        // 사이클 저장
        cycles = new List<List<Cycle>>();
        line = enemySpawnCycleData.text.Substring(0, enemySpawnCycleData.text.Length - 1).Split('\n');
        int stageIndex = -1;
        int laneIndex = -1;
        int cycleIndex = -1;
        for(i = 0; i < line.Length; ++i){
            string[] row = line[i].Split('\t');
            if(row[0] == "Stage"){
                stageIndex = int.Parse(row[1]);
                int stageCount = cycles.Count;
                if(stageCount <= stageIndex){   // 부족한 만큼 stage 리스트 크기 증가
                    for(int j = 0; j < stageIndex - stageCount + 1; ++j){
                        cycles.Add(new List<Cycle>());
                    }
                }
                laneIndex = -1;
                cycleIndex = -1;
            }
            else if(row[0] == "0"){
                ++laneIndex;
                cycleIndex = -1;
                cycles[stageIndex].Add(new Cycle());
                repeatIndex.Add(int.Parse(row[1]));
                isFirst.Add(true);
                isPatternOver.Add(true);
                isCycleOver.Add(true);
            }
            else{
                ++cycleIndex;
                cycles[stageIndex][laneIndex].list.Add(new Cycle.Pattern());
                for(int j = 0; j < row.Length; ++j){
                    if(row[j] == "" || row[j] == "\r") break;
                    cycles[stageIndex][laneIndex].list[cycleIndex].list.Add(int.Parse(row[j]));
                }
            }
        }
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ParseData();
        currentStage = (int)gameManager.currentStage;

        Debug.Log("Stage " + currentStage);
        isActive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isActive){
            for(int i = 0; i < cycles[currentStage].Count; ++i){
                if(isCycleOver[i]){
                    StartCoroutine(StartCycle(i));
                }
            }
        }
    }

    public IEnumerator StartCycle(int laneIndex){
        Debug.Log("Cycle Start");
        isCycleOver[laneIndex] = false;
        int i = isFirst[laneIndex] ? 0 : repeatIndex[laneIndex];
        for(; i < cycles[currentStage][laneIndex].list.Count; ++i){
            Debug.Log("Cycle " + laneIndex + " " + i);
            while(!isActive){
                yield return null;
            }
            int index = Random.Range(0, cycles[currentStage][laneIndex].list[i].list.Count);
            Debug.Log("Select Pattern " + cycles[currentStage][laneIndex].list[i].list[index]);
            StartCoroutine(patterns[cycles[currentStage][laneIndex].list[i].list[index]].StartRoutine(this, laneIndex));
            while(!isPatternOver[laneIndex]){
                yield return null;
            }
        }
        yield return null;
        Debug.Log("Cycle End");
        isCycleOver[laneIndex] = true;
        isFirst[laneIndex] = false;
        yield break;
    }

    #endregion
}
