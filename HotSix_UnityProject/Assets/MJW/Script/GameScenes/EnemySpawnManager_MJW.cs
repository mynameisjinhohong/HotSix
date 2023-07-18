using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnManager_MJW : MonoBehaviour
{
    #region Properties

    public LaneManager_MJW laneManager;
    public TextAsset enemySpawnPatternData;
    private List<EnemySpawnPattern_MJW> patternData;
    [Tooltip("패턴 리스트")]
    public List<int> patternList;
    private int patternCount;
    private int indexCount;
    private float timer;
    [Tooltip("패턴 반복 시작 인덱스")]
    public int repeatIndex;

    #endregion


    #region Monobehavior Callbacks

    void Awake() {
        patternData = new List<EnemySpawnPattern_MJW>();

        string[] line = enemySpawnPatternData.text.Substring(0, enemySpawnPatternData.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; ++i){
            string[] row = line[i].Split('\t');
            int patternIndex = int.Parse(row[0]);
            int patternCount = patternData.Count;

            if(patternCount <= patternIndex){
                for(int j = 0; j < patternIndex - patternCount + 1; ++j){
                    patternData.Add(new EnemySpawnPattern_MJW());
                }
            }

            patternData[patternIndex].Add(new EnemySpawnData_MJW(int.Parse(row[1]), int.Parse(row[2]), float.Parse(row[3]), int.Parse(row[4])));
        }

        patternCount = 0;
        indexCount = 0;
        timer = 0.0f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if(timer >= patternData[patternList[patternCount]].patternList[indexCount].spawnTime){
            laneManager.SpawnEnemyUnit(patternData[patternList[patternCount]].patternList[indexCount].laneIndex,
                                    patternData[patternList[patternCount]].patternList[indexCount].id,
                                    patternData[patternList[patternCount]].patternList[indexCount].level);

            timer = 0.0f;
            ++indexCount;
            if(indexCount >= patternData[patternList[patternCount]].Count){
                ++patternCount;
                indexCount = 0;
            }
            if(patternCount >= patternList.Count){
                patternCount = repeatIndex;
            }
        }
    }

    #endregion
}
