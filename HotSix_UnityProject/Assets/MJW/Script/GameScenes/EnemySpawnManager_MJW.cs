using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public LaneSpawnManager_MJW laneManager;
    public TowerHPManager_HJH towerManager;
    public MathProblem_HJH mathManager;

    public List<EnemySpawnData_MJW.Pattern> patterns;
    public StageDataManager_MJW.StagePatterns stageData;
    public EnemySpawnData_MJW.StagePattern stagePattern;

    public bool isActive = false;
    public int currentStage;

    public List<bool> isFirst;
    public List<bool> isPatternOver;
    public List<bool> isCycleOver;

    public int spawnCount = 0;
    public float gameTimer = 0.0f;
    public int totalMoney = 0;

    #endregion


    #region Methods

    public void Init(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        laneManager = GameObject.Find("LaneManager").GetComponent<LaneSpawnManager_MJW>();
        towerManager = GameObject.Find("TowerHPManager").GetComponent<TowerHPManager_HJH>();
        mathManager = GameObject.Find("API").GetComponent<MathProblem_HJH>();
        
        currentStage = (int)gameManager.currentStage;
        
        patterns = gameManager.stageDataManager.patterns;
        stageData = gameManager.stageDataManager.stagePatterns[currentStage];
        stagePattern = stageData.stagePattern;

        isFirst = new();
        isPatternOver = new();
        isCycleOver = new();

        for(int i = 0; i < stagePattern.cycles.Count; ++i){
            isFirst.Add(true);
            isPatternOver.Add(true);
            isCycleOver.Add(true);
        }

        isActive = true;
    }

    public bool CheckTowerUpgradeCondition(){
        if(towerManager.enemyTowerLevel >= 2) return false;

        int condition = stageData.upgradeTowerConditions[towerManager.enemyTowerLevel][0];
        int value = stageData.upgradeTowerConditions[towerManager.enemyTowerLevel][1];

        if(condition == 0) return false;
        else if(condition == 1){
            return spawnCount >= value;
        }
        else if(condition == 2){
            return gameTimer >= value;
        }
        else if(condition == 3){
            return towerManager.enemyTowerHP / towerManager.enemyMaxHP * 100.0f <= value;
        }
        else if(condition == 4){
            return totalMoney >= value;
        }
        else if(condition == 5){
            return mathManager.correctCount >= value;
        }
        return false;
    }

    public void SpawnEliteEnemy(){
        int laneIndex = Random.Range(0, laneManager.lanes.Length);
        StageDataManager_MJW.StagePatterns.Enemy unit = stageData.eliteEnemy;
        laneManager.SpawnEnemyUnit(laneIndex, unit.id, unit.level);
        ++spawnCount;
    }

    #endregion


    #region Monobehavior Callbacks

    void Start() {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isActive){
            gameTimer += Time.deltaTime;
            for(int i = 0; i < stagePattern.cycles.Count; ++i){
                if(isCycleOver[i]){
                    StartCoroutine(StartCycle(i));
                }
            }
            if(CheckTowerUpgradeCondition()){
                Debug.Log("Upgarde");
                towerManager.EnemyTowerUpgrade();
            }
        }
    }

    public IEnumerator StartCycle(int laneIndex){
        // Debug.Log("Cycle Start");
        isCycleOver[laneIndex] = false;
        int i = isFirst[laneIndex] ? 0 : stagePattern.repeatIndex[laneIndex];
        for(; i < stagePattern.cycles[laneIndex].patterns.Count; ++i){
            // Debug.Log("Cycle " + laneIndex + " " + i);
            while(!isActive){
                yield return null;
            }
            int index = Random.Range(0, stagePattern.cycles[laneIndex].patterns[i].patternIDs.Count);
            // Debug.Log("Select Pattern " + stagePattern.cycles[laneIndex].patterns[i].patternIDs[index]);
            StartCoroutine(patterns[stagePattern.cycles[laneIndex].patterns[i].patternIDs[index]].StartRoutine(this, laneIndex));
            while(!isPatternOver[laneIndex]){
                yield return null;
            }
        }
        yield return null;
        // Debug.Log("Cycle End");
        isCycleOver[laneIndex] = true;
        isFirst[laneIndex] = false;
        yield break;
    }

    #endregion
}
