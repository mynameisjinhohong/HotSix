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
    public GameObject star;
    public GameObject levelStars;

    public bool isEnemy = false;
    public bool isShowingCost = false;
    public bool isShowingStars = false;
    public bool isActiveText = true;

    #endregion

    
    #region Methods

    public void GetData(UnitID unitID){
        this.unitID = unitID;
        int level = GameManager.instance.userInfo.userUnitInfo[unitID.id].level;
        int? stage = GameManager.instance.currentStage;
        if(unitID.unitTag == UnitTag.Special){
            unitImage.sprite = isShowingCost ? GameManager.instance.unitImages.specialUnitImages[unitID.id].moneySpace_Icon : GameManager.instance.unitImages.specialUnitImages[unitID.id].nomal_Icon;
            costText.text = isActiveText ? GameManager.instance.specialUnitTable.specialUnitData[unitID.id].entityInfos.cost.ToString() : "";
            
            if(isShowingStars){
                int count = levelStars.transform.childCount;
                
                for(int i = 0; i < count; ++i){
                    Destroy(levelStars.transform.GetChild(i).gameObject);
                }
                for(int i = 0; i < level; ++i){
                    GameObject starInstance = Instantiate(star);
                    starInstance.transform.SetParent(levelStars.transform);
                }
            }
            else{
                levelText.text = isActiveText ? "Lv." + level.ToString() : "";
            }
        }
        else if(isEnemy){
            unitImage.sprite = isShowingCost ? GameManager.instance.unitImages.enemyUnitImages[unitID.id].moneySpace_Icon : GameManager.instance.unitImages.enemyUnitImages[unitID.id].nomal_Icon;
            costText.text = "";
            levelText.text = "";
            if(stage != null){
                List<StageDataManager_MJW.StagePatterns.Enemy> enemyInfos = GameManager.instance.stageDataManager.stagePatterns[(int)stage].enemyInfos;
                foreach(StageDataManager_MJW.StagePatterns.Enemy enemy in enemyInfos){
                    if(unitID.id == enemy.id){
                        level = enemy.level;
                    }
                }
                if(isShowingStars){
                    int count = levelStars.transform.childCount;
                    
                    for(int i = 0; i < count; ++i){
                        Destroy(levelStars.transform.GetChild(i).gameObject);
                    }
                    for(int i = 0; i < level; ++i){
                        GameObject starInstance = Instantiate(star);
                        starInstance.transform.SetParent(levelStars.transform);
                    }
                }
                else{
                    levelText.text = isActiveText ? "Lv." + level.ToString() : "";
                }
            }
        }
        else{
            unitImage.sprite = isShowingCost ? GameManager.instance.unitImages.playerUnitImages[unitID.id].moneySpace_Icon : GameManager.instance.unitImages.playerUnitImages[unitID.id].nomal_Icon;
            costText.text = isActiveText ? GameManager.instance.playerUnitTable.unitData[unitID.id].entityInfos.cost.ToString() : "";
            if(isShowingStars){
                int count = levelStars.transform.childCount;
                
                for(int i = 0; i < count; ++i){
                    Destroy(levelStars.transform.GetChild(i).gameObject);
                }
                for(int i = 0; i < level; ++i){
                    GameObject starInstance = Instantiate(star);
                    starInstance.transform.SetParent(levelStars.transform);
                }
            }
            else{
                levelText.text = isActiveText ? "Lv." + level.ToString() : "";
            }
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
        levelStars = transform.Find("LevelStars").transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion
}
