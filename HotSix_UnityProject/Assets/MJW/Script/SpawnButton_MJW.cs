using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton_MJW : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public Button[] buttons;

    public GameObject selectedUnit;
    public Button selectedButton;

    public MoneyManager_HJH moneyManager;

    
    public void SelectButton(int index){
        selectedUnit = unitPrefabs[index];
        selectedButton = buttons[index];
        Unit_MJW unitStatus = selectedUnit.GetComponent<Unit_MJW>();
        if(moneyManager.money < unitStatus.unitStat.cost)
        {
            selectedUnit = null;
            selectedButton = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttons.Length; ++i){
            int index = i;
            buttons[index].onClick.AddListener(() => SelectButton(index));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
