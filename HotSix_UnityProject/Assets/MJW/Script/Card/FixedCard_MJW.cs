using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class FixedCard_MJW : MonoBehaviour
{
    #region Properties

    public UnitID unitID;
    public Image unitImage;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;

    public bool isEnemy = false;
    public bool isShowingCost = false;
    public bool isActiveText = true;

    #endregion

    
    #region Methods

    public void GetData(UnitID unitID){
        this.unitID = unitID;
        if(unitID.unitTag == UnitTag.Special){
            unitImage.sprite = isShowingCost ? GameManager.instance.unitImages.specialUnitImages[unitID.id].moneySpace_Icon : GameManager.instance.unitImages.specialUnitImages[unitID.id].nomal_Icon;
            costText.text = isActiveText ? GameManager.instance.specialUnitTable.specialUnitData[unitID.id].entityInfos.cost.ToString() : "";
            levelText.text = isActiveText ? "Lv." + GameManager.instance.userInfo.userSpecialUnitInfo[unitID.id].level.ToString() : "";
        }
        else if(isEnemy){
            unitImage.sprite = isShowingCost ? GameManager.instance.unitImages.enemyUnitImages[unitID.id].moneySpace_Icon : GameManager.instance.unitImages.enemyUnitImages[unitID.id].nomal_Icon;
            costText.text = "";
            levelText.text = "";
        }
        else{
            unitImage.sprite = isShowingCost ? GameManager.instance.unitImages.playerUnitImages[unitID.id].moneySpace_Icon : GameManager.instance.unitImages.playerUnitImages[unitID.id].nomal_Icon;
            costText.text = isActiveText ? GameManager.instance.playerUnitTable.unitData[unitID.id].entityInfos.cost.ToString() : "";
            levelText.text = isActiveText ? "Lv." + GameManager.instance.userInfo.userUnitInfo[unitID.id].level.ToString() : "";
        }
    }

    #endregion


    #region Monobehavior Callbacks

    // Start is called before the first frame update
    void Awake()
    {
        unitImage = transform.Find("Image").GetComponent<Image>();
        levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
