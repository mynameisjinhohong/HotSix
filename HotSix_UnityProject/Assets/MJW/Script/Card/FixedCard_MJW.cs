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
    public bool isFullImage = false;
    public bool isActiveText = true;

    #endregion

    
    #region Methods

    public void GetData(UnitID unitID){
        this.unitID = unitID;
        if(unitID.unitTag == UnitTag.Special){
            unitImage.sprite = isFullImage ? GameManager.instance.unitImages.specialUnitImages[unitID.id].fullImage : GameManager.instance.unitImages.specialUnitImages[unitID.id].iconImage;
            costText.text = isActiveText ? GameManager.instance.specialUnitTable.specialUnitData[unitID.id].entityInfos.cost.ToString() : "";
            levelText.text = isActiveText ? "Lv." + GameManager.instance.userInfo.userSpecialUnitInfo[unitID.id].level.ToString() : "";
        }
        else if(isEnemy){
            unitImage.sprite = isFullImage ? GameManager.instance.unitImages.enemyUnitImages[unitID.id].fullImage : GameManager.instance.unitImages.enemyUnitImages[unitID.id].iconImage;
            costText.text = "";
            levelText.text = "";
        }
        else{
            unitImage.sprite = isFullImage ? GameManager.instance.unitImages.playerUnitImages[unitID.id].fullImage : GameManager.instance.unitImages.playerUnitImages[unitID.id].iconImage;
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
