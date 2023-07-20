using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData_MJW{
    
    public class Unit{
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
    public List<EnemySpawnData_MJW> patternList;
    public int Count{
        get{ return patternList.Count; }
    }

    public EnemySpawnPattern_MJW(){
        patternList = new List<EnemySpawnData_MJW>();
    }

    public void Add(EnemySpawnData_MJW data){
        patternList.Add(data);
    }
}
