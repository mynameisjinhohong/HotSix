using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEngine;

[System.Serializable]
public class StageDataManager_MJW
{
    #region Properties

    [System.Serializable]
    public class StagePatterns{
        [System.Serializable]
        public struct Enemy{
            public int id;
            public int level;
        }

        public List<Enemy> enemyInfos;
        [HideInInspector]
        public EnemySpawnData_MJW.StagePattern stagePattern;
        public Enemy eliteEnemy;
        public List<List<int>> upgradeTowerConditions;

        public StagePatterns(){
            enemyInfos = new List<Enemy>();
            stagePattern = new EnemySpawnData_MJW.StagePattern();
            eliteEnemy = new Enemy();
            upgradeTowerConditions = new List<List<int>>();
        }
    }

    public TextAsset enemySpawnInfoData;
    public TextAsset enemySpawnPatternData;
    public TextAsset enemySpawnCycleData;

    [HideInInspector]
    public List<EnemySpawnData_MJW.Pattern> patterns;
    public List<StagePatterns> stagePatterns;

    #endregion


    #region Methods

    public void ParseData(){
        // 패턴 저장
        patterns = new List<EnemySpawnData_MJW.Pattern>();
        string[] line = enemySpawnPatternData.text.Substring(0, enemySpawnPatternData.text.Length - 1).Split('\n');
        int i = 0;
        int patternIndex = 0;
        int temp;
        while(i < line.Length){
            string[] row1 = line[i].Split('\t');
            string[] row2 = line[i + 1].Split('\t');
            EnemySpawnData_MJW.Line data = new EnemySpawnData_MJW.Line();
            int patternCount = patterns.Count;

            if(patternCount <= patternIndex){   // 부족한 만큼 patterns 리스트 크기 증가
                for(int j = 0; j < patternIndex - patternCount + 1; ++j){
                    patterns.Add(new EnemySpawnData_MJW.Pattern());
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
        stagePatterns = new List<StagePatterns>();
        line = enemySpawnCycleData.text.Substring(0, enemySpawnCycleData.text.Length - 1).Split('\n');
        int stageIndex = -1;
        int laneIndex = -1;
        int cycleIndex = -1;
        for(i = 0; i < line.Length; ++i){
            string[] row = line[i].Split('\t');
            if(row[0] == "Stage"){
                stageIndex = int.Parse(row[1]);
                int stageCount = stagePatterns.Count;
                if(stageCount <= stageIndex){   // 부족한 만큼 stage 리스트 크기 증가
                    for(int j = 0; j < stageIndex - stageCount + 1; ++j){
                        stagePatterns.Add(new StagePatterns());
                    }
                }
                laneIndex = -1;
                cycleIndex = -1;
            }
            else if(row[0] == "0"){
                ++laneIndex;
                cycleIndex = -1;
                stagePatterns[stageIndex].stagePattern.cycles.Add(new EnemySpawnData_MJW.Cycle());
                stagePatterns[stageIndex].stagePattern.repeatIndex.Add(int.Parse(row[1]));
            }
            else{
                ++cycleIndex;
                stagePatterns[stageIndex].stagePattern.cycles[laneIndex].patterns.Add(new EnemySpawnData_MJW.Cycle.PatternID());
                for(int j = 0; j < row.Length; ++j){
                    if(row[j] == "" || row[j] == "\r") break;
                    stagePatterns[stageIndex].stagePattern.cycles[laneIndex].patterns[cycleIndex].patternIDs.Add(int.Parse(row[j]));
                }
            }
        }

        // 적 정보 저장
        line = enemySpawnInfoData.text.Substring(0, enemySpawnInfoData.text.Length - 1).Split('\n');
        stageIndex = 0;
        for(i = 0; i < line.Length; i += 4){
            List<List<string>> rows = new();
            for(int j = 0; j < 4; ++j){
                string[] row = line[i + j].Split('\t');
                rows.Add(row.ToList());
            }
            ++stageIndex;
            for(int j = 0; j < rows[0].Count; j += 2){
                if(rows[0][j] == "" || rows[0][j] == "\r") break;
                stagePatterns[stageIndex].enemyInfos.Add(new StagePatterns.Enemy(){ id = int.Parse(rows[0][j]), level = int.Parse(rows[0][j + 1]) });
            }
            stagePatterns[stageIndex].eliteEnemy = new StagePatterns.Enemy(){ id = int.Parse(rows[1][0]), level = int.Parse(rows[1][1]) };
            for(int j = 0; j < 2; ++j){
                stagePatterns[stageIndex].upgradeTowerConditions.Add(new List<int>(){ int.Parse(rows[2 + j][0]), int.Parse(rows[2 + j][1]) });
            }
        }
    }

    #endregion

}
