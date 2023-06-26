using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton_MJW : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public Button[] buttons;

    public void SpawnUnit(int index){
        GameObject unitInstance = Instantiate(unitPrefabs[index]);
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttons.Length; ++i){
            int index = i;
            buttons[index].onClick.AddListener(() => SpawnUnit(index));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
