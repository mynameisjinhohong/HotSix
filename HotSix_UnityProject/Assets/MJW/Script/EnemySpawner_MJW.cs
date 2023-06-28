using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_MJW : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public float[] spawnTime;
    public int[] laneIndex;
    public LaneManager_MJW laneManager;
    private int count;
    private float timer;

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
        if(count >= unitPrefabs.Length){
            count = 0;
            timer = 0.0f;
        }
        else if(timer >= spawnTime[count]){
            timer = 0.0f;
            laneManager.SpawnEnemyUnit(laneIndex[count], unitPrefabs[count]);
            count++;
        }
        else{
            timer += Time.deltaTime;
        }
    }
}
