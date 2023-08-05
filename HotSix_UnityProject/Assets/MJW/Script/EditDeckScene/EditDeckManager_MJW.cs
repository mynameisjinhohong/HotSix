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
    public struct CardInfoTabText_MJW
    {
        public TextMeshProUGUI unitNameText;
        public TextMeshProUGUI unitInfoText;

        public List<TextMeshProUGUI> unitStatText;
        public List<TextMeshProUGUI> unitUStatText;

        public TextMeshProUGUI unitNumberText;
    }

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
            else if(hit.CompareTag("Card")){
                return 2;
            }
        }

        return 0;
    }

    /// <summary>
    /// 유닛 정보 창에 선택한 유닛 정보 출력
    /// </summary>
    public void ShowCurrentUnit(GameObject card){
        StartCoroutine(ChangeTab(isCardInfoTabShown));

        currentCard = card;
        int id = currentCard.GetComponent<Card_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        UnitData unit = gameManager.playerUnitTable.unitData[id];

        float unitMaxHP = unit.unitStats.maxHP + unit.upgradeStats.uMaxHP * (level - 1);
        float unitAttackDamage = unit.unitStats.attackDamage + unit.upgradeStats.uAttackDamage * (level - 1);
        float unitAttackRange = unit.unitStats.attackRange;
        float unitAttackSpeed = unit.unitStats.attackSpeed;
        float unitDefensive = unit.unitStats.defensive + unit.upgradeStats.uDefensive * (level - 1);
        float unitMoveSpeed = unit.unitStats.moveSpeed;
        int unitCost = unit.unitStats.cost;
        float unitCooldown = unit.unitStats.cooldown;

        int unitIndex = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == id);
        int unitNumber = gameManager.userInfo.userUnitInfo[unitIndex].number;
        int unitUpgradeNumber = unit.upgradeStats.upgradeCost * level;

        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            cardInfoTabText.unitNameText.text = unit.unitInfos.k_name;
        }
        else
        {
            cardInfoTabText.unitNameText.text = unit.unitInfos.e_name;
        }
        if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
        {
            cardInfoTabText.unitInfoText.text = unit.unitInfos.k_information;
        }
        else
        {
            cardInfoTabText.unitInfoText.text = unit.unitInfos.e_information;
        }

        cardInfoTabText.unitStatText[0].text = unitMaxHP.ToString();
        cardInfoTabText.unitStatText[1].text = unitAttackDamage.ToString();
        cardInfoTabText.unitStatText[2].text = unitAttackRange.ToString();
        cardInfoTabText.unitStatText[3].text = unitAttackSpeed.ToString();
        cardInfoTabText.unitStatText[4].text = unitDefensive.ToString();
        cardInfoTabText.unitStatText[5].text = unitMoveSpeed.ToString();
        cardInfoTabText.unitStatText[6].text = unitCost.ToString();
        cardInfoTabText.unitStatText[7].text = unitCooldown.ToString();

        cardInfoTabText.unitUStatText[0].text = (unitMaxHP + unit.upgradeStats.uMaxHP).ToString();
        cardInfoTabText.unitUStatText[1].text = (unitAttackDamage + unit.upgradeStats.uAttackDamage).ToString();
        cardInfoTabText.unitUStatText[2].text = unitAttackRange.ToString();
        cardInfoTabText.unitUStatText[3].text = unitAttackSpeed.ToString();
        cardInfoTabText.unitUStatText[4].text = (unitDefensive + unit.upgradeStats.uDefensive).ToString();
        cardInfoTabText.unitUStatText[5].text = unitMoveSpeed.ToString();
        cardInfoTabText.unitUStatText[6].text = unitCost.ToString();
        cardInfoTabText.unitUStatText[7].text = unitCooldown.ToString();


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

        for(int i = 0; i < 5; ++i){
            Card_MJW card = deckCard.transform.GetChild(i).GetComponent<Card_MJW>();
            card.id = currentDeck.unitIDs[i];
            card.GetText();
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
            card.GetText();
        }
    }

    /// <summary>
    /// 창 전환
    /// </summary>
    /// <param name="getCardInfo">True : 유닛 정보 창을 보이게, False : 덱 목록 창을 보이게</param>
    public IEnumerator ChangeTab(bool getCardInfo){
        float t = 0.5f * Time.deltaTime;
        Vector3[] minV = new Vector3[3];
        Vector3[] maxV = new Vector3[3];
        for(int i = 0; i < 3; ++i){
            minV[i] = new Vector3(0.0f + (0.5f * i) - (getCardInfo ? 0.0f : 0.5f), 0.0f, 0.0f);
            maxV[i] = new Vector3(0.5f + (0.5f * i) - (getCardInfo ? 0.0f : 0.5f), 1.0f, 0.0f);
        }

        while((getCardInfo && minV[0].x > 0.5f) || (!getCardInfo && minV[0].x < 0.0f)){
            deckListTransform.anchorMin = minV[0];
            deckListTransform.anchorMin = maxV[0];
            cardListTransform.anchorMin = minV[1];
            cardListTransform.anchorMax = maxV[1];
            cardInfoTransform.anchorMin = minV[2];
            cardInfoTransform.anchorMax = maxV[2];

            for(int i = 0; i < 3; ++i){
                minV[i].x += getCardInfo ? -t : t;
                maxV[i].x += getCardInfo ? -t : t;
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
        int id = currentCard.GetComponent<Card_MJW>().id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        UnitData unit = gameManager.playerUnitTable.unitData[id];
        int index = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == id);
        int unitNumber = gameManager.userInfo.userUnitInfo[index].number;
        int unitUpgradeNumber = unit.upgradeStats.upgradeCost * level;

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
    }

    #endregion
}
