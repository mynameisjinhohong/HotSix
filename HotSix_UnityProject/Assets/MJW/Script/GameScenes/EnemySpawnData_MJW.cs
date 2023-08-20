using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData_MJW
{
    [System.Serializable]
    public struct Unit
    {
        public int id;
        public int level;

        public Unit(int id, int level){
            this.id = id;
            this.level = level;
        }
    }

    [System.Serializable]
    public class Line
    {
        public List<Unit> units;
        public float totalTime;
        public float minTime;
        public float maxTime;

        public Line(){
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
    public class Pattern
    {
        public List<Line> lines;

        public int Count{
            get{ return lines.Count; }
        }

        public Pattern(){
            lines = new List<Line>();
        }

        public void Add(Line data){
            lines.Add(data);
        }

        public IEnumerator StartRoutine(EnemySpawnManager_MJW enemySpawnManager, int laneIndex){
            enemySpawnManager.isPatternOver[laneIndex] = false;
            yield return null;
            foreach(Line cur in lines){
                while(!enemySpawnManager.isActive){
                    yield return null;
                }
                int randIndex = Random.Range(0, cur.units.Count);
                float randTime = Random.Range(cur.minTime, cur.maxTime);
                float time = 0;
                bool isSpawned = false;
                Unit unit = cur.units[randIndex];
                while(time <= cur.totalTime){
                    if(time >= randTime && !isSpawned){
                        if(unit.id != 0) enemySpawnManager.laneManager.SpawnEnemyUnit(laneIndex, unit.id, unit.level);
                        isSpawned = true;
                        ++enemySpawnManager.spawnCount;
                        // Debug.Log("Spawn " + unit.id);
                    }
                    time += Time.deltaTime;
                    yield return null;
                }
            }
            yield return null;
            // Debug.Log("Pattern End");
            enemySpawnManager.isPatternOver[laneIndex] = true;
            yield break;
        }
    }

    [System.Serializable]
    public class Cycle{
        [System.Serializable]
        public class PatternID{
            public List<int> patternIDs;

            public PatternID(){
                patternIDs = new List<int>();
            }

            public string Print(){
                string str = "";
                foreach(int i in patternIDs){
                    str += i.ToString();
                    str += " ";
                }
                return str;
            }
        }

        public List<PatternID> patterns;

        public Cycle(){
            patterns = new List<PatternID>();
        }

        public string Print(){
            string str = "";
            for(int i = 0; i < patterns.Count; ++i){
                str += i + " ";
                str += patterns[i].Print();
                str += "\n";
            }
            return str;
        }
    }

    [System.Serializable]
    public class StagePattern{
        public List<Cycle> cycles;
        public List<int> repeatIndex;

        public StagePattern(){
            cycles = new List<Cycle>();
            repeatIndex = new List<int>();
        }
    }
}



