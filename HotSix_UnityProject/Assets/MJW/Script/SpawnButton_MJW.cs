using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton_MJW : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public Button[] buttons; // assign in the inspector

    public void SpawnUnit(int index){
        GameObject unitInstance = Instantiate(unitPrefabs[index]);
        // Set the unit instance's position or other properties as you need
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttons.Length; ++i){
            int index = i;
            // Link the button click to the SpawnUnit method
            buttons[index].onClick.AddListener(() => SpawnUnit(index));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
