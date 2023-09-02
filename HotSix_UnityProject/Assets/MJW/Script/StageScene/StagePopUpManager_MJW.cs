using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class StagePopUpManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public StageButtonManager_MJW stageButtonManager;

    public List<FixedCard_MJW> curDeckCards;
    public GameObject slotPrefab;

    public TextMeshProUGUI stageInfoText;

    public int currentStage;

    #endregion

    
    #region Methods

    private void OnEnable(){
        if(gameManager.currentStage == null) return;
        currentStage = (int)gameManager.currentStage;
        stageInfoText.text = "Stage " + currentStage;

        for(int i = 0; i < 5; ++i){
            curDeckCards[i].isShowingCost = true;
            curDeckCards[i].GetData(gameManager.currentDeck.unitIDs[i]);
        }

        Transform parent = transform.Find("EnemyInfo").Find("EnemyList");
        int count = parent.childCount;
        for(int i = 0; i < count; ++i)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        StageDataManager_MJW.StagePatterns stagePattern = gameManager.stageDataManager.stagePatterns[currentStage];
        for(int i = 0; i < stagePattern.enemyInfos.Count; ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(parent);
            FixedCard_MJW card = slot.GetComponent<FixedCard_MJW>();
            UnitID unit = new(){
                unitTag = UnitTag.Unit,
                id = stagePattern.enemyInfos[i].id
            };
            card.isEnemy = true;
            card.isShowingCost = false;
            card.isShowingStars = true;
            card.isActiveText = false;
            card.GetData(unit);
        }
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageButtonManager = GameObject.Find("StageButtons").GetComponent<StageButtonManager_MJW>();
    }

    #endregion
}
