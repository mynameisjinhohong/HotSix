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
    public List<Image> images;
    public List<int> moneys;
    public List<float> cooldowns;
    public List<float> currentCooldowns;
    [HideInInspector]
    public int? selectedIndex;
    
    #endregion
    

    #region Methods

    public void SelectButton(int index){
        if(index >= unitPrefabsID.Count || currentCooldowns[index] > 0.0f || moneyManager.money < moneys[index]) return;
        selectedIndex = index;
    }

    public void CountCooldowns(float time){
        for(int i = 0; i < currentCooldowns.Count; ++i){
            currentCooldowns[i] -= time;
            if(currentCooldowns[i] < 0.0f) currentCooldowns[i] = 0.0f;
            images[i].fillAmount = 1.0f - (currentCooldowns[i] / cooldowns[i]);
        }
    }

    #endregion


    #region MonoBehavior Callbacks

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        selectedIndex = null;

        images = new List<Image>();
        unitPrefabsID = new List<int>();
        moneys = new List<int>();
        cooldowns = new List<float>();
        currentCooldowns = new List<float>();
        for(int i = 0; i < gameManager.currentDeck.unitIDs.Count; ++i){
            if(i >= buttons.Length) break;
            int index = gameManager.currentDeck.unitIDs[i];
            unitPrefabsID.Add(index);
            UnitObject_MJW unitObject = gameManager.unitPrefabManager.unitPrefabs[index].GetComponent<UnitObject_MJW>();
            moneys.Add(unitObject.unit.unitStat.cost);
            cooldowns.Add(unitObject.unit.unitStat.cooldown);
            currentCooldowns.Add(0.0f);
        }
        for(int i = 0; i < buttons.Length; ++i){
            int index = i;
            images.Add(buttons[i].transform.Find("Image").transform.GetComponent<Image>());
            buttons[index].onClick.AddListener(() => SelectButton(index));
        }
    }

    void FixedUpdate()
    {
        CountCooldowns(Time.deltaTime);
    }

    #endregion
}
