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

    [System.Serializable]
    public struct CardInfoTabObject
    {
        public TextMeshProUGUI unitNameText;
        public TextMeshProUGUI unitInfoText;

        public TextMeshProUGUI unitLevelText;
        public TextMeshProUGUI unitCostText;

        public List<Transform> unitStatObject;

        public TextMeshProUGUI unitNumberText;

        public Image unitImage;
    }

    public GameManager gameManager;

    public GameObject canvas;
    public Transform deckListTab;
    public Transform cardListTab;
    public Transform cardInfoTab;
    public GameObject slotPrefab;

    private RectTransform deckListTransform;
    private RectTransform cardListTransform;
    private RectTransform cardInfoTransform;

    public CardInfoTabObject cardInfoTabObject;
    public GameObject[] deckChangeButton;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public GameObject currentCard;
    public GameObject selectedCard;
    public GameObject targetCard;
    public int selectedButton = 0;  // 덱 선택 버튼
    public bool isCardInfoTabShown = false;
    public int selected;

    #endregion


    #region Methods

    /// <summary>
    /// 버튼 클릭 감지
    /// </summary>
    public int ClickSlot(){
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();

        raycaster.Raycast(pointerEventData, results);     
        for(int i = 0; i < results.Count; ++i){
            GameObject hit = results[i].gameObject;
            if(hit.CompareTag("Button")){
                for(int j = 0; j < deckChangeButton.Length; ++j){
                    if(System.Object.ReferenceEquals(deckChangeButton[j], hit)){
                        selectedButton = j;
                        break;
                    }
                }
                return 1;
            }
            else if(hit.CompareTag("Card") || hit.transform.parent.CompareTag("Card")){
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
        int id = currentCard.GetComponent<DeckCard_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        UnitData unit = gameManager.playerUnitTable.unitData[id];

        float unitMaxHP = unit.unitStats.maxHP + unit.unitStats.uMaxHP * (level - 1);
        float unitAttackDamage = unit.actionBehaviors[unit.attackAction].value + unit.actionBehaviors[unit.attackAction].upgradeValue * (level - 1);
        float unitAttackRange = unit.actionBehaviors[unit.attackAction].range;
        float unitAttackSpeed = System.MathF.Round(1.0f / unit.actionBehaviors[unit.attackAction].cooldown, 1);
        float unitDefensive = unit.unitStats.defensive + unit.unitStats.uDefensive * (level - 1);
        float unitMoveSpeed = unit.unitStats.moveSpeed;
        int unitCost = unit.entityInfos.cost;
        float unitCooldown = unit.entityInfos.cooldown;
        string unitSecondAction;

        int unitIndex = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == id);
        int unitNumber = gameManager.userInfo.userUnitInfo[unitIndex].number;
        int unitUpgradeNumber = 10 * level;

        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            cardInfoTabObject.unitNameText.text = unit.entityInfos.k_name;

            cardInfoTabObject.unitInfoText.text = unit.entityInfos.k_information;

            unitSecondAction = unit.secondAction.k_name;
        }
        else
        {
            cardInfoTabObject.unitNameText.text = unit.entityInfos.e_name;

            cardInfoTabObject.unitInfoText.text = unit.entityInfos.e_information;

            unitSecondAction = unit.secondAction.e_name;
        }

        cardInfoTabObject.unitImage.sprite = gameManager.unitImages.playerUnitImages[id].fullImage;

        cardInfoTabObject.unitLevelText.text = "Lv." + level.ToString();
        cardInfoTabObject.unitCostText.text = unitCost.ToString();

        cardInfoTabObject.unitStatObject[0].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitMaxHP.ToString();
        cardInfoTabObject.unitStatObject[1].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitAttackDamage.ToString();
        cardInfoTabObject.unitStatObject[2].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitAttackRange.ToString();
        cardInfoTabObject.unitStatObject[3].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitAttackSpeed.ToString();
        cardInfoTabObject.unitStatObject[4].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitDefensive.ToString();
        cardInfoTabObject.unitStatObject[5].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitMoveSpeed.ToString();
        cardInfoTabObject.unitStatObject[6].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitCooldown.ToString();
        
        cardInfoTabObject.unitStatObject[0].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unitMaxHP + unit.unitStats.uMaxHP).ToString();
        cardInfoTabObject.unitStatObject[1].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unitAttackDamage + unit.actionBehaviors[unit.attackAction].upgradeValue).ToString();
        cardInfoTabObject.unitStatObject[2].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitAttackRange.ToString();
        cardInfoTabObject.unitStatObject[3].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitAttackSpeed.ToString();
        cardInfoTabObject.unitStatObject[4].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unitDefensive + unit.unitStats.uDefensive).ToString();
        cardInfoTabObject.unitStatObject[5].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitMoveSpeed.ToString();
        cardInfoTabObject.unitStatObject[6].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitCooldown.ToString();
        
        cardInfoTabObject.unitStatObject[0].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);
        cardInfoTabObject.unitStatObject[1].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);
        cardInfoTabObject.unitStatObject[4].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);

        cardInfoTabObject.unitStatObject[1].gameObject.SetActive(unit.attackAction >= 0);
        cardInfoTabObject.unitStatObject[2].gameObject.SetActive(unit.attackAction >= 0);
        cardInfoTabObject.unitStatObject[3].gameObject.SetActive(unit.attackAction >= 0);

        if(unit.secondAction.index >= 0){
            cardInfoTabObject.unitStatObject[7].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = unitSecondAction;
            cardInfoTabObject.unitStatObject[7].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unit.actionBehaviors[unit.secondAction.index].value.ToString();
            cardInfoTabObject.unitStatObject[7].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unit.actionBehaviors[unit.secondAction.index].value + unit.actionBehaviors[unit.secondAction.index].upgradeValue).ToString();

            Vector3 temp = cardInfoTabObject.unitStatObject[7].GetComponent<RectTransform>().anchoredPosition;

            if(unit.attackAction >= 0){
                cardInfoTabObject.unitStatObject[7].GetComponent<RectTransform>().anchoredPosition = new Vector3(temp.x, -330, temp.z);
            }
            else{
                cardInfoTabObject.unitStatObject[7].GetComponent<RectTransform>().anchoredPosition = new Vector3(temp.x, -90, temp.z);
            }

            cardInfoTabObject.unitStatObject[7].gameObject.SetActive(true);
            cardInfoTabObject.unitStatObject[7].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);
        }
        else{
            cardInfoTabObject.unitStatObject[7].gameObject.SetActive(false);
        }

        cardInfoTabObject.unitNumberText.text = unitNumber.ToString() + "/" + unitUpgradeNumber.ToString();
    }

    /// <summary>
    /// 유저 덱 목록 창 갱신
    /// </summary>
    public void ShowCurrentDeck(){
        Transform deckCard = deckListTab.Find("DeckCard").Find("CardList");
        gameManager.userInfo.selectedDeck = selectedButton;
        gameManager.currentDeck = gameManager.userInfo.userDecks[selectedButton];
        Deck_MJW currentDeck = gameManager.currentDeck;

        float total = 0;

        for(int i = 0; i < 5; ++i){
            DeckCard_MJW card = deckCard.GetChild(i).GetComponent<DeckCard_MJW>();
            card.id = currentDeck.unitIDs[i];
            total += gameManager.playerUnitTable.unitData[card.id].entityInfos.cost;
            card.GetData();
        }

        deckListTab.Find("AverageCost").Find("Average").Find("Text").GetComponent<TextMeshProUGUI>().text = (total / 5.0f).ToString();

        gameManager.SaveData();
    }

    /// <summary>
    /// 유닛 카드 목록 창 생성
    /// </summary>
    public void MakeSlots(){
        Transform parent = cardListTab.Find("CardList").Find("Viewport").Find("Content");

        for(int i = 1; i < gameManager.userInfo.userUnitInfo.Count; ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(parent);
            DeckCard_MJW card = slot.GetComponent<DeckCard_MJW>();
            card.id = i;
            card.GetData();
        }
    }

    /// <summary>
    /// 창 전환
    /// </summary>
    public IEnumerator ChangeTab(){
        isCardInfoTabShown = !isCardInfoTabShown;
        yield return null;

        float t = Time.deltaTime;
        Vector3[] minV = new Vector3[3];
        Vector3[] maxV = new Vector3[3];
        for(int i = 0; i < 3; ++i){
            minV[i] = new Vector3(0.0f + (0.5f * i) - (isCardInfoTabShown ? 0.0f : 0.5f), 0.0f, 0.0f);
            maxV[i] = new Vector3(0.5f + (0.5f * i) - (isCardInfoTabShown ? 0.0f : 0.5f), 1.0f, 0.0f);
        }

        while((isCardInfoTabShown && minV[0].x > -0.5f) || (!isCardInfoTabShown && minV[0].x < 0.0f)){
            deckListTransform.anchorMin = minV[0];
            deckListTransform.anchorMax = maxV[0];
            cardListTransform.anchorMin = minV[1];
            cardListTransform.anchorMax = maxV[1];
            cardInfoTransform.anchorMin = minV[2];
            cardInfoTransform.anchorMax = maxV[2];

            for(int i = 0; i < 3; ++i){
                minV[i].x += isCardInfoTabShown ? -t : t;
                maxV[i].x += isCardInfoTabShown ? -t : t;
            }

            yield return null;
        }

        yield break;
    }

    public void ChangeScene(){
        SceneManager.LoadScene("StageScene");
    }

    /// <summary>
    /// 유닛 업그레이드
    /// </summary>
    public void UpgradeCard(){
        int id = currentCard.GetComponent<DeckCard_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        UnitData unit = gameManager.playerUnitTable.unitData[id];
        int index = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == id);
        int unitNumber = gameManager.userInfo.userUnitInfo[index].number;
        int unitUpgradeNumber = 10 * level;

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
                selected = 2;

                if(!isCardInfoTabShown) StartCoroutine(ChangeTab());
            }
            selectedCard = null;
        }
        else{
            Transform deckCard = deckListTab.Find("DeckCard").Find("CardList");
            Deck_MJW currentDeck = gameManager.currentDeck;
            int selectedIndex = 0, targetIndex = 0, temp;
            if(selectedCard.transform.parent.name == "CardList"){         // DeckListTab
                if(selectedCard.transform.parent == targetCard.transform.parent){   // 덱 내에서 카드 교환
                    for(int i = 0; i < 5; ++i){
                        if(System.Object.ReferenceEquals(deckCard.GetChild(i).gameObject, selectedCard)){
                            selectedIndex = i;
                        }
                        if(System.Object.ReferenceEquals(deckCard.GetChild(i).gameObject, targetCard)){
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
                    selected = -1;
                    
                    if(!isCardInfoTabShown) StartCoroutine(ChangeTab());   
                }
                else if(selectedCard.transform.parent != targetCard.transform.parent){  // 카드를 덱에 추가
                    for(int i = 0; i < 5; ++i){
                        if(System.Object.ReferenceEquals(deckCard.GetChild(i).gameObject, targetCard)){
                            targetIndex = i;
                        }
                    }
                    currentDeck.unitIDs[targetIndex] = selectedCard.GetComponent<DeckCard_MJW>().id;
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
                if(isCardInfoTabShown) StartCoroutine(ChangeTab());
            }
            else if(selected == 1){
                ShowCurrentDeck();
            }
        }
    }

    #endregion
}
