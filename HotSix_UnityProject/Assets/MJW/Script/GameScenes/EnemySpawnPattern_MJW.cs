using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData_MJW{
    public int id;
    public int level;
    public float spawnTime;
    public int laneIndex;

    public EnemySpawnData_MJW(int id, int level, float spawnTime, int laneIndex){
        this.id = id;
        this.level = level;
        this.spawnTime = spawnTime;
        this.laneIndex = laneIndex;
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
