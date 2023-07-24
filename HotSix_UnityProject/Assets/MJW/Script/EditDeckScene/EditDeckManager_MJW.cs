using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;

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
    public GameObject[] deckChangeButton;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public GameObject currentCard;
    public GameObject selectedCard;
    public GameObject targetCard;
    public int selectedButton = 0;  // 덱 선택 버튼
    public bool isCardInfoTabShown = false;
    public int selected = 0;

    #endregion


    #region Methods

    /// <summary>
    /// 버튼 클릭 감지
    /// </summary>
    public int ClickSlot(){
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; ++i){
            GameObject hit = results[i].gameObject;
            if(hit.tag == "Button"){
                for(int j = 0; j < deckChangeButton.Length; ++j){
                    if(System.Object.ReferenceEquals(deckChangeButton[j], hit)){
                        selectedButton = j;
                        break;
                    }
                }
                return 1;
            }
            else if(hit.tag == "Card"){
                return 2;
            }
        }

        return 0;
    }

    /// <summary>
    /// 유닛 정보 창에 선택한 유닛 정보 출력
    /// </summary>
    public void ShowCurrentUnit(GameObject card){
        currentCard = card;
        int id = currentCard.GetComponent<Card_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        Unit_MJW unit = gameManager.unitDataList[id];

        float unitMaxHP = unit.unitStat.maxHP + unit.upgradeStat.maxHP * (level - 1);
        float unitAttackDamage = unit.unitStat.attackDamage + unit.upgradeStat.attackDamage * (level - 1);
        float unitAttackSpeed = unit.unitStat.attackSpeed + unit.upgradeStat.attackSpeed * (level - 1);
        float unitAttackRange = unit.unitStat.attackRange + unit.upgradeStat.attackRange * (level - 1);
        float unitDefensive = unit.unitStat.defensive + unit.upgradeStat.defensive * (level - 1);
        float unitMoveSpeed = unit.unitStat.moveSpeed + unit.upgradeStat.moveSpeed * (level - 1);
        int unitCost = unit.unitStat.cost;
        int unitIndex = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == id);
        int unitNumber = gameManager.userInfo.userUnitInfo[unitIndex].number;
        int unitUpgradeNumber = unit.upgradeStat.uCost * level;
        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            cardInfoTabText.unitNameText.text = unit.unitInfo.k_name;
        }
        else
        {
            cardInfoTabText.unitNameText.text = unit.unitInfo.e_name;
        }
        cardInfoTabText.unitLevelText.text = level.ToString();
        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            cardInfoTabText.unitInfoText.text = unit.unitInfo.k_information;
        }
        else
        {
            cardInfoTabText.unitInfoText.text = unit.unitInfo.e_information;
        }
        cardInfoTabText.unitMaxHPText.text = unitMaxHP.ToString();
        cardInfoTabText.unitAttackDamageText.text = unitAttackDamage.ToString();
        cardInfoTabText.unitAttackSpeedText.text = unitAttackSpeed.ToString();
        cardInfoTabText.unitAttackRangeText.text = unitAttackRange.ToString();
        cardInfoTabText.unitDefensiveText.text = unitDefensive.ToString();
        cardInfoTabText.unitMoveSpeedText.text = unitMoveSpeed.ToString();
        cardInfoTabText.unitCostText.text = unitCost.ToString();
        cardInfoTabText.unitNumberText.text = unitNumber.ToString() + "/" + unitUpgradeNumber.ToString();
    }

    /// <summary>
    /// 유저 덱 목록 창 갱신
    /// </summary>
    public void ShowCurrentDeck(){
        GameObject deckCard = deckListTab.transform.Find("DeckCard").gameObject;
        gameManager.userInfo.selectedDeck = selectedButton;
        gameManager.currentDeck = gameManager.userInfo.userDecks[selectedButton];
        Deck_MJW currentDeck = gameManager.currentDeck;

        for(int i = 0; i < 8; ++i){
            Card_MJW card = deckCard.transform.GetChild(i).GetComponent<Card_MJW>();
            card.id = currentDeck.unitIDs[i];
            card.GetNameText();
        }
        gameManager.SaveData();
    }

    /// <summary>
    /// 유닛 카드 목록 창 생성
    /// </summary>
    public void MakeSlots(){
        GameObject parent = cardListTab.transform.GetChild(1).transform.GetChild(0).gameObject;

        for(int i = 1; i < gameManager.userInfo.userUnitInfo.Count; ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(parent.transform);
            Card_MJW card = slot.GetComponent<Card_MJW>();
            card.id = i;
            card.GetNameText();
        }
    }

    /// <summary>
    /// 창 전환
    /// </summary>
    /// <param name="getCardInfo">True : 유닛 정보 창을 보이게, False : 덱 목록 창을 보이게</param>
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

    /// <summary>
    /// 유닛 업그레이드
    /// </summary>
    public void UpgradeCard(){
        int id = currentCard.GetComponent<Card_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        Unit_MJW unit = gameManager.unitDataList[id];
        int index = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == id);
        int unitNumber = gameManager.userInfo.userUnitInfo[index].number;
        int unitUpgradeNumber = unit.upgradeStat.uCost * level;

        if(unitNumber >= unitUpgradeNumber){
            gameManager.userInfo.userUnitInfo[index].number -= unitUpgradeNumber;
            gameManager.userInfo.userUnitInfo[index].level++;
            gameManager.SaveData();
            ShowCurrentUnit(currentCard);
        }
        else{

        }
    }

    /// <summary>
    /// 이벤트 관리
    /// </summary>
    public void GetEvent(){
        if(selectedCard == null) return;
        if(targetCard == null){                                                     // 아무것도 없는 곳으로 드래그했을 때
            if(selectedCard.transform.parent.name == "Content"){         // CardListTab
                ShowCurrentUnit(selectedCard);
                isCardInfoTabShown = true;
                selected = 2;
            }
            selectedCard = null;
        }
        else{
            GameObject deckCard = deckListTab.transform.Find("DeckCard").gameObject;
            Deck_MJW currentDeck = gameManager.currentDeck;
            int selectedIndex = 0, targetIndex = 0, temp;
            if(selectedCard.transform.parent.name == "DeckCard"){         // DeckListTab
                if(selectedCard.transform.parent == targetCard.transform.parent){   // 덱 내에서 카드 교환
                    for(int i = 0; i < 8; ++i){
                        if(System.Object.ReferenceEquals(deckCard.transform.GetChild(i).gameObject, selectedCard)){
                            selectedIndex = i;
                        }
                        if(System.Object.ReferenceEquals(deckCard.transform.GetChild(i).gameObject, targetCard)){
                            targetIndex = i;
                        }
                    }
                    temp = currentDeck.unitIDs[selectedIndex];
                    currentDeck.unitIDs[selectedIndex] = currentDeck.unitIDs[targetIndex];
                    currentDeck.unitIDs[targetIndex] = temp;
                    gameManager.SaveData();
                    ShowCurrentDeck();
                }
            }
            else if(selectedCard.transform.parent.name == "Content"){     // CardListTab
                if(System.Object.ReferenceEquals(selectedCard, targetCard)){            // 카드 선택
                    ShowCurrentUnit(selectedCard);
                    isCardInfoTabShown = true;
                    selected = -1;
                }
                else if(selectedCard.transform.parent != targetCard.transform.parent){  // 카드를 덱에 추가
                    for(int i = 0; i < 8; ++i){
                        if(System.Object.ReferenceEquals(deckCard.transform.GetChild(i).gameObject, targetCard)){
                            targetIndex = i;
                        }
                    }
                    currentDeck.unitIDs[targetIndex] = selectedCard.GetComponent<Card_MJW>().id;
                    gameManager.SaveData();
                    ShowCurrentDeck();
                }
            }
            selectedCard = null;
            targetCard = null;
        }
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
            if(selected == 0){
                isCardInfoTabShown = false;
            }
            else if(selected == 1){
                ShowCurrentDeck();
            }
        }
        ChangeTab(isCardInfoTabShown);
    }

    #endregion
}
