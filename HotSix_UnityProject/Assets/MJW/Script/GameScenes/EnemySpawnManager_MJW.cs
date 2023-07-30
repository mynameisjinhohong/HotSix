using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnManager_MJW : MonoBehaviour
{
    #region Properties

    public LaneSpawnManager_MJW laneManager;
    public TextAsset enemySpawnPatternData;
    public TextAsset enemySpawnCycleData;

    private List<EnemySpawnPattern_MJW> patternData;
    private List<List<List<int>>> cycleData;
    private int laneCount;

    private int[] randomCount;
    private int[] patternCount;
    private int[] nodeCount;
    private float[] selectedSpawnTime;
    private float[] selectedTotalTime;
    private int[] selectedUnit;
    private int[] selectedPattern;
    private bool[] isSpawned;
    private float[] timer;
    
    [Tooltip("패턴 반복 시작 인덱스, 라인 개수만큼 설정할 것")]
    public List<int> repeatIndex;

    #endregion


    #region Methods

    public void GetPattern(int laneIndex){
        selectedPattern[laneIndex] = Random.Range(0, cycleData[laneIndex][patternCount[laneIndex]].Count);
        return;
    }

    public void GetNode(int laneIndex){
        EnemySpawnData_MJW node = patternData[cycleData[laneIndex][patternCount[laneIndex]][selectedPattern[laneIndex]]].patternList[nodeCount[laneIndex]];
        if(node.units[0].id == 0){
            selectedUnit[laneIndex] = -1;
            selectedTotalTime[laneIndex] = node.totalTime;
            selectedSpawnTime[laneIndex] = selectedTotalTime[laneIndex] = 0;
            return;
        }
        selectedUnit[laneIndex] = Random.Range(0, node.units.Count);
        selectedTotalTime[laneIndex] = node.totalTime;
        selectedSpawnTime[laneIndex] = Random.Range(node.minTime, node.maxTime);
        return;
    }

    public void IncreaseCount(int laneIndex){
        timer[laneIndex] = 0.0f;
        ++nodeCount[laneIndex];
        if(nodeCount[laneIndex] >= patternData[cycleData[laneIndex][patternCount[laneIndex]][selectedPattern[laneIndex]]].Count){
            ++patternCount[laneIndex];
            if(patternCount[laneIndex] >= cycleData[laneIndex].Count){
                patternCount[laneIndex] = repeatIndex[laneIndex];
            }
            GetPattern(laneIndex);
            nodeCount[laneIndex] = 0;
        }
        GetNode(laneIndex);
        isSpawned[laneIndex] = false;
        return;
    }

    public void SpawnUnit(int laneIndex){
        laneManager.SpawnEnemyUnit(laneIndex,
                                    patternData[cycleData[laneIndex][patternCount[laneIndex]][selectedPattern[laneIndex]]].patternList[nodeCount[laneIndex]].units[selectedUnit[laneIndex]].id,
                                    patternData[cycleData[laneIndex][patternCount[laneIndex]][selectedPattern[laneIndex]]].patternList[nodeCount[laneIndex]].units[selectedUnit[laneIndex]].level);

        isSpawned[laneIndex] = true;
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake() {
        // 패턴 저장
        patternData = new List<EnemySpawnPattern_MJW>();
        string[] line = enemySpawnPatternData.text.Substring(0, enemySpawnPatternData.text.Length - 1).Split('\n');
        int i = 0;
        int patternIndex = 0;
        int temp = 0;
        while(i < line.Length){
            string[] row1 = line[i].Split('\t');
            string[] row2 = line[i + 1].Split('\t');
            EnemySpawnData_MJW data = new EnemySpawnData_MJW();
            int patternCount = patternData.Count;

            if(patternCount <= patternIndex){   // 부족한 만큼 patternData 리스트 크기 증가
                for(int j = 0; j < patternIndex - patternCount + 1; ++j){
                    patternData.Add(new EnemySpawnPattern_MJW());
                }
            }

            if(row1[1] == "" || row1[1] == "\r"){
                temp = int.Parse(row1[0]);
                
                if(temp == 0){                  // 대기 시간 추가
                    data.AddUnit(0, 0);
                    patternData[patternIndex].Add(data);
                    i += 2;
                    
                }
                else{                           // 패턴 번호 확인
                    patternIndex = temp;
                    ++i;
                }
                continue;
            }

            // 패턴에 노드 추가
            for(int j = 0; j < row1.Length; j += 2){
                if(row1[j] == "" || row1[j] == "\r") break;
                data.AddUnit(int.Parse(row1[j]), int.Parse(row1[j + 1]));
            }
            data.totalTime = float.Parse(row2[0]);
            data.minTime = float.Parse(row2[1]);
            data.maxTime = float.Parse(row2[2]);
            
            patternData[patternIndex].Add(data);
            i += 2;
        }

        // 사이클 저장
        cycleData = new List<List<List<int>>>();
        line = enemySpawnCycleData.text.Substring(0, enemySpawnCycleData.text.Length - 1).Split('\n');
        int firstIndex = -1;
        int secondIndex = -1;
        for(i = 0; i < line.Length; ++i){
            string[] row = line[i].Split('\t');
            if(row[0] == "0"){
                ++firstIndex;
                secondIndex = -1;
                cycleData.Add(new List<List<int>>());
            }
            else{
                ++secondIndex;
                cycleData[firstIndex].Add(new List<int>());
                for(int j = 0; j < row.Length; ++j){
                    if(row[j] == "" || row[j] == "\r") break;
                    cycleData[firstIndex][secondIndex].Add(int.Parse(row[j]));
                }
            }
        }

        // 기타 변수 초기화
        laneCount = cycleData.Count;
        randomCount = new int[laneCount];
        patternCount = new int[laneCount];
        nodeCount = new int[laneCount];
        selectedSpawnTime = new float[laneCount];
        selectedTotalTime = new float[laneCount];
        selectedUnit = new int[laneCount];
        selectedPattern = new int[laneCount];
        timer = new float[laneCount];
        isSpawned = new bool[laneCount];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < laneCount; ++i){
            timer[i] += Time.deltaTime;

            if(selectedUnit[i] != -1 && !isSpawned[i] && timer[i] >= selectedSpawnTime[i]){
                SpawnUnit(i);
            }
            if(timer[i] >= selectedTotalTime[i]){
                IncreaseCount(i);
            }
        }
    }

    #endregion
}
