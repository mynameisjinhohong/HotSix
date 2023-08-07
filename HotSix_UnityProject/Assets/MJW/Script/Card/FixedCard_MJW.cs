using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class FixedCard_MJW : MonoBehaviour
{
    #region Properties

    public int id;
    public Image unitImage;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;

    public bool isFullImage = false;
    public bool isActiveText = true;

    #endregion

    
    #region Methods

    public void GetData(int id){
        unitImage.sprite = isFullImage ? GameManager.instance.unitImages.playerUnitImages[id].fullImage : GameManager.instance.unitImages.playerUnitImages[id].iconImage;
        costText.text = isActiveText ? GameManager.instance.playerUnitTable.unitData[id].unitStats.cost.ToString() : "";
        levelText.text = isActiveText ? "Lv." + GameManager.instance.userInfo.userUnitInfo[id].level.ToString() : "";
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
