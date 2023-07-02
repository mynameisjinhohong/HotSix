using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton_MJW : MonoBehaviour
{
    #region Properties

    [HideInInspector]
    public GameManager gameManager;
    public MoneyManager_HJH moneyManager;
    public List<int> unitPrefabsID;
    public Button[] buttons;
    [HideInInspector]
    public int? selectedUnitID;
    [HideInInspector]
    public Button selectedButton;
    
    #endregion
    

    #region Methods

    public void SelectButton(int index){
        if(index >= unitPrefabsID.Count) return;
        selectedUnitID = (int?)unitPrefabsID[index];
        selectedButton = buttons[index];
        UnitObject_MJW unit = gameManager.unitPrefabManager.unitPrefabs[(int)selectedUnitID].GetComponent<UnitObject_MJW>();
        if(moneyManager.money < unit.unit.unitStat.cost)
        {
            selectedUnitID = null;
            selectedButton = null;
        }
    }

    #endregion


    #region MonoBehavior Callbacks

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        unitPrefabsID = new List<int>();
        for(int i = 0; i < gameManager.currentDeck.unitIDs.Count; ++i){
            if(i >= buttons.Length) break;
            unitPrefabsID.Add(gameManager.currentDeck.unitIDs[i]);
        }
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
