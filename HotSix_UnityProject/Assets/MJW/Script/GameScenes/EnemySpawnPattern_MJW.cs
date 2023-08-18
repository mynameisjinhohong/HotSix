using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData_MJW{
    
    public struct Unit{
        public int id;
        public int level;

        public Unit(int id, int level){
            this.id = id;
            this.level = level;
        }
    }

    public List<Unit> units;
    public float totalTime;
    public float minTime;
    public float maxTime;

    public EnemySpawnData_MJW(){
        units = new List<Unit>();
        totalTime = 0;
        minTime = 0;
        maxTime = 0;
    }

    public void AddUnit(int id, int level){
        units.Add(new Unit(id, level));
    }

    public void SetTime(float totalTime, float minTime, float maxTime){
        this.totalTime = totalTime;
        this.minTime = minTime;
        this.maxTime = maxTime;
    }
}

[System.Serializable]
public class EnemySpawnPattern_MJW
{
    public List<EnemySpawnData_MJW> list;

    public int Count{
        get{ return list.Count; }
    }

    public EnemySpawnPattern_MJW(){
        list = new List<EnemySpawnData_MJW>();
    }

    public void Add(EnemySpawnData_MJW data){
        list.Add(data);
    }

    public IEnumerator StartRoutine(EnemySpawnManager_MJW enemySpawnManager, int laneIndex){
        enemySpawnManager.isPatternOver[laneIndex] = false;
        yield return null;
        foreach(EnemySpawnData_MJW cur in list){
            while(!enemySpawnManager.isActive){
                yield return null;
            }
            int randIndex = Random.Range(0, cur.units.Count);
            float randTime = Random.Range(cur.minTime, cur.maxTime);
            float time = 0;
            bool isSpawned = false;
            EnemySpawnData_MJW.Unit unit = cur.units[randIndex];
            while(time <= cur.totalTime){
                if(time >= randTime && !isSpawned){
                    if(unit.id != 0) enemySpawnManager.laneManager.SpawnEnemyUnit(laneIndex, unit.id, unit.level);
                    isSpawned = true;
                    Debug.Log("Spawn " + unit.id);
                }
                time += Time.deltaTime;
                yield return null;
            }
        }
        yield return null;
        Debug.Log("Pattern End");
        enemySpawnManager.isPatternOver[laneIndex] = true;
        yield break;
    }
}