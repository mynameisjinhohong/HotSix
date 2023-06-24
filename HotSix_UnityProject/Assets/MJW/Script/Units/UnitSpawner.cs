using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;

    public void SpawnUnit(){
        GameObject unitInstance = Instantiate(unitPrefab);
        // Set the unit instance's position or other properties as you need
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
