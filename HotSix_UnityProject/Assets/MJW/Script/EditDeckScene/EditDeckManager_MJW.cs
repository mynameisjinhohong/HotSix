using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class EditDeckManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;

    public GameObject canvas;
    public GameObject deckListTab;
    public GameObject cardListTab;
    public GameObject cardInfoTab;
    public GameObject slotPrefab;

    private RectTransform deckListTransform;
    private RectTransform cardListTransform;
    private RectTransform cardInfoTransform;

    public CardInfoTabText_MJW cardInfoTabText;
    public TextMeshProUGUI[] deckText;
    public GameObject[] deckChangeButton;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public GameObject selectedCard;
    public int selectedButton = 0;
    public bool isCardInfoTabShown = false;
    public int selected = 0;

    #endregion


    #region Methods

    public int ClickSlot(){
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; ++i){
            GameObject hit = results[i].gameObject;
            if(hit.tag == "Card"){
                selectedCard = hit;
                return 1;
            }
            if(hit.tag == "Button"){
                for(int j = 0; j < deckChangeButton.Length; ++j){
                    
                    if(System.Object.ReferenceEquals(deckChangeButton[j], hit)){
                        selectedButton = j;
                        break;
                    }
                }
                return 2;
            }
        }

        return 0;
    }

    public void MakeSlots(){
        GameObject parent = cardListTab.transform.GetChild(0).transform.GetChild(0).gameObject;

        for(int i = 1; i < gameManager.userInfo.userUnitInfo.Count; ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(parent.transform);
            Card_MJW card = slot.GetComponent<Card_MJW>();
            card.id = i;
            card.GetNameText();
        }
    }

    public void ChangeTab(bool getCardInfo){
        float t = Time.deltaTime;
        if(getCardInfo){
            deckListTransform.anchorMin = Vector3.Lerp(deckListTransform.anchorMin, new Vector3(-0.5f, 0.0f, 0.0f), t);
            deckListTransform.anchorMax = Vector3.Lerp(deckListTransform.anchorMax, new Vector3(0.0f, 1.0f, 0.0f), t);
            cardListTransform.anchorMin = Vector3.Lerp(cardListTransform.anchorMin, new Vector3(0.0f, 0.0f, 0.0f), t);
            cardListTransform.anchorMax = Vector3.Lerp(cardListTransform.anchorMax, new Vector3(0.5f, 1.0f, 0.0f), t);
            cardInfoTransform.anchorMin = Vector3.Lerp(cardInfoTransform.anchorMin, new Vector3(0.5f, 0.0f, 0.0f), t);
            cardInfoTransform.anchorMax = Vector3.Lerp(cardInfoTransform.anchorMax, new Vector3(1.0f, 1.0f, 0.0f), t);
        }
        else{
            deckListTransform.anchorMin = Vector3.Lerp(deckListTransform.anchorMin, new Vector3(0.0f, 0.0f, 0.0f), t);
            deckListTransform.anchorMax = Vector3.Lerp(deckListTransform.anchorMax, new Vector3(0.5f, 1.0f, 0.0f), t);
            cardListTransform.anchorMin = Vector3.Lerp(cardListTransform.anchorMin, new Vector3(0.5f, 0.0f, 0.0f), t);
            cardListTransform.anchorMax = Vector3.Lerp(cardListTransform.anchorMax, new Vector3(1.0f, 1.0f, 0.0f), t);
            cardInfoTransform.anchorMin = Vector3.Lerp(cardInfoTransform.anchorMin, new Vector3(1.0f, 0.0f, 0.0f), t);
            cardInfoTransform.anchorMax = Vector3.Lerp(cardInfoTransform.anchorMax, new Vector3(1.5f, 1.0f, 0.0f), t);
        }
    }

    public void ChangeScene(){
        SceneManager.LoadScene("StageScene");
    }

    public void ShowCurrentUnit(){
        int id = selectedCard.GetComponent<Card_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        Unit_MJW unit = gameManager.unitDataList[id];

        float unitMaxHP = unit.unitStat.maxHP + unit.upgradeStat.maxHP * level;
        float unitAttackDamage = unit.unitStat.attackDamage + unit.upgradeStat.attackDamage * level;
        float unitAttackSpeed = unit.unitStat.attackSpeed + unit.upgradeStat.attackSpeed * level;
        float unitAttackRange = unit.unitStat.attackRange + unit.upgradeStat.attackRange * level;
        float unitDefensive = unit.unitStat.defensive + unit.upgradeStat.defensive * level;
        float unitMoveSpeed = unit.unitStat.moveSpeed + unit.upgradeStat.moveSpeed * level;
        int unitCost = unit.unitStat.cost;

        cardInfoTabText.unitNameText.text = unit.unitInfo.k_name;
        cardInfoTabText.unitLevelText.text = level.ToString();
        cardInfoTabText.unitInfoText.text = unit.unitInfo.k_information;
        cardInfoTabText.unitMaxHPText.text = unitMaxHP.ToString();
        cardInfoTabText.unitAttackDamageText.text = unitAttackDamage.ToString();
        cardInfoTabText.unitAttackSpeedText.text = unitAttackSpeed.ToString();
        cardInfoTabText.unitAttackRangeText.text = unitAttackRange.ToString();
        cardInfoTabText.unitDefensiveText.text = unitDefensive.ToString();
        cardInfoTabText.unitMoveSpeedText.text = unitMoveSpeed.ToString();
        cardInfoTabText.unitCostText.text = unitCost.ToString();
    }

    public void ShowCurrentDeck(){
        gameManager.userInfo.selectedDeck = selectedButton;
        gameManager.currentDeck = gameManager.userInfo.userDecks[selectedButton];
        Deck_MJW currentDeck = gameManager.currentDeck;
        for(int i = 0; i < 8; ++i){
            deckText[i].text = gameManager.unitPrefabManager.unitPrefabs[currentDeck.unitIDs[i]].GetComponent<UnitObject_MJW>().unit.unitInfo.k_name;
        }
        gameManager.SaveData();
    }

    public void ChangeDeck(){

    }

    public void ChangeCard(){

    }

    #endregion


    #region Monobehavior Callbacks
    
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        
        deckListTransform = deckListTab.GetComponent<RectTransform>();
        cardListTransform = cardListTab.GetComponent<RectTransform>();
        cardInfoTransform = cardInfoTab.GetComponent<RectTransform>();

        isCardInfoTabShown = false;

        selectedButton = gameManager.userInfo.selectedDeck;

        MakeSlots();
        ShowCurrentDeck();
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            selected = ClickSlot();
            if(selected == 1){
                ShowCurrentUnit();
            }
            else if(selected == 2){
                ShowCurrentDeck();
            }
        }
        if(selected != 1){
            ChangeTab(false);
        }
        else if(selected == 1){
            ChangeTab(true);
        }
        
    }

    #endregion
}
