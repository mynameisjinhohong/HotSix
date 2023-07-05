using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_MJW : MonoBehaviour
{
    #region Properties

    public LaneManager_MJW laneManager;
    public int[] unitPrefabsID;
    public float[] spawnTime;
    public int[] laneIndex;
    
    private int count;
    private float timer;

    #endregion


    #region Monobehavior Callbacks

    void Awake() {
        count = 0;
        timer = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(count >= unitPrefabsID.Length || count >= spawnTime.Length || count >= laneIndex.Length){
            count = 0;
            timer = 0.0f;
        }
        else if(timer >= spawnTime[count]){
            timer = 0.0f;
            laneManager.SpawnEnemyUnit(laneIndex[count], unitPrefabsID[count]);
            count++;
        }
        else{
            timer += Time.deltaTime;
        }
    }

    #endregion
}
