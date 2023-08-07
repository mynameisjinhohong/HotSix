using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class StagePopUpManager : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public StageButtonManager stageButtonManager;

    public List<FixedCard_MJW> curDeckCards;

    public TextMeshProUGUI stageInfoText;

    #endregion

    
    #region Methods

    private void OnEnable(){
        if(gameManager.currentStage == null) return;
        stageInfoText.text = "Stage " + gameManager.currentStage;

        for(int i = 0; i < 5; ++i){
            curDeckCards[i].GetData(gameManager.currentDeck.unitIDs[i]);
        }
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stageButtonManager = GameObject.Find("StageButtons").GetComponent<StageButtonManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
