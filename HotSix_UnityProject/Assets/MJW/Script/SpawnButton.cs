using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    public Button spawnButton; // assign in the inspector
    public UnitSpawner unitSpawner; // assign in the inspector

    // Start is called before the first frame update
    void Start()
    {
        // Link the button click to the SpawnUnit method
        spawnButton.onClick.AddListener(unitSpawner.SpawnUnit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
