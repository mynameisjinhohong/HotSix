using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_MJW : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public float[] spawnTime;
    private int count;
    private float timer;

    public void SpawnUnit(int index){
        GameObject unitInstance = Instantiate(unitPrefabs[index]);
        Unit_MJW unit = unitInstance.GetComponent<Unit_MJW>();
        unit.isEnemy = true;
        unit.unitStat.moveSpeed *= -1;
        unit.transform.position = new Vector3(30.0f, 0, 0);
    }

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
            SpawnUnit(count++);
        }
        else{
            timer += Time.deltaTime;
        }
    }
}
