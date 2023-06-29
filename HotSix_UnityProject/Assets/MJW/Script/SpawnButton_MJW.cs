using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton_MJW : MonoBehaviour
{
    #region Properties

    public MoneyManager_HJH moneyManager;
    public GameObject[] unitPrefabs;
    public Button[] buttons;
    [HideInInspector]
    public GameObject selectedUnit;
    [HideInInspector]
    public Button selectedButton;
    
    #endregion
    

    #region Methods

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

    #endregion


    #region MonoBehavior Callbacks

    void Start()
    {
        for(int i = 0; i < buttons.Length; ++i){
            int index = i;
            buttons[index].onClick.AddListener(() => SelectButton(index));
        }
    }

    void Update()
    {
        
    }

    #endregion
}
