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
    public AudioSource[] audios;  //첫번째 덱 번호 선택 사운드, 두번째 화면 전환 사운드


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
    public Transform backGround;
    public Transform deckListTab;
    public Transform cardListTab;
    public Transform cardInfoTab;
    public GameObject slotPrefab;

    private RectTransform backGroundTransform;
    private RectTransform deckListTransform;
    private RectTransform cardListTransform;
    private RectTransform cardInfoTransform;

    public CardInfoTabObject cardInfoTabObject;
    public GameObject[] deckChangeButton;
    public Sprite[] deckEnableButton;
    public Sprite[] deckDisableButton;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public GameObject currentCard;
    public GameObject selectedCard;
    public GameObject targetCard;
    public int selectedButton = 0;  // 덱 선택 버튼
    public bool isCardInfoTabShown = false;
    public int selected;

    public Vector3 startPos;
    public Vector3 endPos;

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
                        audios[0].Play();
                        break;
                    }
                }
                return 1;
            }
            else if(hit.CompareTag("Card") || hit.transform.parent.CompareTag("Card")){
                return 2;
            }
            else if(hit.CompareTag("InfoTab")){
                return -1;
            }
        }

        return 0;
    }

    public string SetValueText(string stat, float value){
        string str;
        if(stat == "AttackSpeed"){
            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                if(value >= 1.2f) str = "빠름";
                else if(value >= 0.8f) str = "보통";
                else str = "느림";
            }
            else
            {
                if(value >= 1.2f) str = "Fast";
                else if(value >= 0.8f) str = "Normal";
                else str = "Slow";
            }
        }
        else if(stat == "MoveSpeed"){
            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                if(value >= 4.0f) str = "빠름";
                else if(value >= 3.0f) str = "보통";
                else if(value >= 2.0f) str = "느림";
                else str = "매우 느림";
            }
            else
            {
                if(value >= 1.2f) str = "Fast";
                else if(value >= 1.2f) str = "Normal";
                else if(value >= 2.0f) str = "Slow";
                else str = "Very Slow";
            }
        }
        else if(stat == "AttackRange"){
            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                if(value >= 3.0f) str = "긺";
                else if(value >= 2.0f) str = "중간";
                else str = "짧음";
            }
            else
            {
                if(value >= 3.0f) str = "Long";
                else if(value >= 2.0f) str = "Normal";
                else str = "Short";
            }
        }
        else{
            str = "";
        }
        return str;
    }

    /// <summary>
    /// 유닛 정보 창에 선택한 유닛 정보 출력
    /// </summary>
    public void ShowCurrentUnit(GameObject card){
        currentCard = card;
        UnitID unitID = currentCard.GetComponent<DeckCard_MJW>().unitID;
        int level = 0;
        int unitIndex = 0;
        int unitNumber = 0;
        int unitUpgradeNumber = 0;

        if(unitID.unitTag == UnitTag.Unit){
            level = gameManager.userInfo.userUnitInfo[unitID.id].level;
            UnitData unit = gameManager.playerUnitTable.unitData[unitID.id];

            float unitMaxHP = unit.unitStats.maxHP + unit.unitStats.uMaxHP * (level - 1);
            float unitAttackDamage = unit.actionBehaviors[unit.attackAction].value + unit.actionBehaviors[unit.attackAction].upgradeValue * (level - 1);
            float unitAttackRange = unit.actionBehaviors[unit.attackAction].range;
            float unitAttackSpeed = System.MathF.Round(1.0f / unit.actionBehaviors[unit.attackAction].cooldown, 1);
            float unitDefensive = unit.unitStats.defensive + unit.unitStats.uDefensive * (level - 1);
            float unitMoveSpeed = unit.unitStats.moveSpeed;
            int unitCost = unit.entityInfos.cost;
            float unitCooldown = unit.entityInfos.cooldown;
            string unitSecondAction;

            unitIndex = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == unitID.id);
            unitNumber = gameManager.userInfo.userUnitInfo[unitIndex].number;
            unitUpgradeNumber = 10 * level;

            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                cardInfoTabObject.unitNameText.text = unit.entityInfos.k_name;

                cardInfoTabObject.unitInfoText.text = unit.entityInfos.k_information;

                cardInfoTabObject.unitStatObject[0].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = "체력";
                cardInfoTabObject.unitStatObject[1].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = "공격력";

                unitSecondAction = unit.secondAction.k_name;
            }
            else
            {
                cardInfoTabObject.unitNameText.text = unit.entityInfos.e_name;

                cardInfoTabObject.unitInfoText.text = unit.entityInfos.e_information;

                cardInfoTabObject.unitStatObject[0].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = "HP";
                cardInfoTabObject.unitStatObject[1].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = "Damage";

                unitSecondAction = unit.secondAction.e_name;
            }

            cardInfoTabObject.unitImage.sprite = gameManager.unitImages.playerUnitImages[unitID.id].moneySpace_Icon;

            cardInfoTabObject.unitLevelText.text = "Lv." + level.ToString();
            cardInfoTabObject.unitCostText.text = unitCost.ToString();

            cardInfoTabObject.unitStatObject[0].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitMaxHP.ToString();
            cardInfoTabObject.unitStatObject[1].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitAttackDamage.ToString();
            cardInfoTabObject.unitStatObject[2].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = SetValueText("AttackSpeed", unitAttackSpeed);
            cardInfoTabObject.unitStatObject[3].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = SetValueText("AttackRange", unitAttackRange);
            cardInfoTabObject.unitStatObject[4].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitDefensive.ToString();
            cardInfoTabObject.unitStatObject[5].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = SetValueText("MoveSpeed", unitMoveSpeed);
            cardInfoTabObject.unitStatObject[6].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitCooldown.ToString();
            
            cardInfoTabObject.unitStatObject[0].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unitMaxHP + unit.unitStats.uMaxHP).ToString();
            cardInfoTabObject.unitStatObject[1].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unitAttackDamage + unit.actionBehaviors[unit.attackAction].upgradeValue).ToString();
            cardInfoTabObject.unitStatObject[2].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = SetValueText("AttackSpeed", unitAttackSpeed);
            cardInfoTabObject.unitStatObject[3].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = SetValueText("AttackRange", unitAttackRange);
            cardInfoTabObject.unitStatObject[4].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unitDefensive + unit.unitStats.uDefensive).ToString();
            cardInfoTabObject.unitStatObject[5].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = SetValueText("MoveSpeed", unitMoveSpeed);
            cardInfoTabObject.unitStatObject[6].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitCooldown.ToString();

            cardInfoTabObject.unitStatObject[0].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);
            cardInfoTabObject.unitStatObject[1].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);
            cardInfoTabObject.unitStatObject[4].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);

            for(int i = 0; i < cardInfoTabObject.unitStatObject.Count; ++i){
                cardInfoTabObject.unitStatObject[i].gameObject.SetActive(true);
            }

            cardInfoTabObject.unitStatObject[1].gameObject.SetActive(unitAttackDamage > 0.0f);
            cardInfoTabObject.unitStatObject[2].gameObject.SetActive(unitAttackDamage > 0.0f);
            cardInfoTabObject.unitStatObject[3].gameObject.SetActive(unitAttackDamage > 0.0f);

            if(unit.secondAction.index >= 0){
                cardInfoTabObject.unitStatObject[7].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = unitSecondAction;
                cardInfoTabObject.unitStatObject[7].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unit.actionBehaviors[unit.secondAction.index].value.ToString();
                cardInfoTabObject.unitStatObject[7].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unit.actionBehaviors[unit.secondAction.index].value + unit.actionBehaviors[unit.secondAction.index].upgradeValue).ToString();

                Vector3 temp = cardInfoTabObject.unitStatObject[7].GetComponent<RectTransform>().anchoredPosition;

                if(unitAttackDamage > 0.0f){
                    cardInfoTabObject.unitStatObject[7].GetComponent<RectTransform>().anchoredPosition = new Vector3(temp.x, -318, temp.z);
                }
                else{
                    cardInfoTabObject.unitStatObject[7].GetComponent<RectTransform>().anchoredPosition = new Vector3(temp.x, -110, temp.z);
                }

                cardInfoTabObject.unitStatObject[7].gameObject.SetActive(true);
                cardInfoTabObject.unitStatObject[7].Find("Value").Find("UpgradeValue").gameObject.SetActive(unitNumber >= unitUpgradeNumber);
            }
            else{
                cardInfoTabObject.unitStatObject[7].gameObject.SetActive(false);
            }
        }
        else if(unitID.unitTag == UnitTag.Special){
            level = gameManager.userInfo.userSpecialUnitInfo[unitID.id].level;
            SpecialUnitData unit = gameManager.specialUnitTable.specialUnitData[unitID.id];

            int unitCost = unit.entityInfos.cost;
            float unitCooldown = unit.entityInfos.cooldown;
            string specialAction;

            unitIndex = gameManager.userInfo.userSpecialUnitInfo.FindIndex(x => x.id == unitID.id);
            unitNumber = gameManager.userInfo.userSpecialUnitInfo[unitIndex].number;
            unitUpgradeNumber = 10 * level;

            if (LocalizationSettings.SelectedLocale.ToString().Contains("ko"))
            {
                cardInfoTabObject.unitNameText.text = unit.entityInfos.k_name;

                cardInfoTabObject.unitInfoText.text = unit.entityInfos.k_information;

                cardInfoTabObject.unitStatObject[0].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = "대기시간";
                specialAction = unit.action.k_name;
            }
            else
            {
                cardInfoTabObject.unitNameText.text = unit.entityInfos.e_name;

                cardInfoTabObject.unitInfoText.text = unit.entityInfos.e_information;

                cardInfoTabObject.unitStatObject[0].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = "Cooldown";
                specialAction = unit.action.e_name;
            }

            cardInfoTabObject.unitImage.sprite = gameManager.unitImages.specialUnitImages[unitID.id].moneySpace_Icon;

            cardInfoTabObject.unitLevelText.text = "Lv." + level.ToString();
            cardInfoTabObject.unitCostText.text = unitCost.ToString();

            for(int i = 0; i < cardInfoTabObject.unitStatObject.Count; ++i){
                cardInfoTabObject.unitStatObject[i].gameObject.SetActive(i < 2);
            }

            cardInfoTabObject.unitStatObject[0].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unitCooldown.ToString();

            cardInfoTabObject.unitStatObject[1].Find("Name").Find("Text").GetComponent<TextMeshProUGUI>().text = specialAction;
            cardInfoTabObject.unitStatObject[1].Find("Value").Find("CurValue").Find("Text").GetComponent<TextMeshProUGUI>().text = unit.actionBehavior.value.ToString();
            cardInfoTabObject.unitStatObject[1].Find("Value").Find("UpgradeValue").Find("Text").GetComponent<TextMeshProUGUI>().text = (unit.actionBehavior.value + unit.actionBehavior.upgradeValue).ToString();
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
            card.unitID = currentDeck.unitIDs[i];
            if(card.unitID.unitTag == UnitTag.Unit){
                total += gameManager.playerUnitTable.unitData[card.unitID.id].entityInfos.cost;
            }
            else{
                total += gameManager.specialUnitTable.specialUnitData[card.unitID.id].entityInfos.cost;
            }
            
            card.GetData();
        }

        for(int i = 0; i < deckChangeButton.Length; ++i){
            if(i == selectedButton){
                RectTransform button = deckChangeButton[i].GetComponent<RectTransform>();
                Vector3 pos = button.anchoredPosition;
                pos.y = 0.0f;
                button.anchoredPosition = pos;

                deckChangeButton[i].GetComponent<Image>().sprite = deckEnableButton[i];
            }
            else{
                RectTransform button = deckChangeButton[i].GetComponent<RectTransform>();
                Vector3 pos = button.anchoredPosition;
                pos.y = -20.0f;
                button.anchoredPosition = pos;

                deckChangeButton[i].GetComponent<Image>().sprite = deckDisableButton[i];
            }
            
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
            card.unitID.unitTag = UnitTag.Unit;
            card.unitID.id = i;
            card.GetData();
        }
        for(int i = 1; i < gameManager.userInfo.userSpecialUnitInfo.Count; ++i){
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(parent);
            DeckCard_MJW card = slot.GetComponent<DeckCard_MJW>();
            card.unitID.unitTag = UnitTag.Special;
            card.unitID.id = i;
            card.GetData();
        }
    }

    /// <summary>
    /// 창 전환
    /// </summary>
    public IEnumerator ChangeTab(){
        isCardInfoTabShown = !isCardInfoTabShown;
        yield return null;
        audios[1].Play();
        float t = Time.deltaTime;
        Vector3[] minV = new Vector3[3];
        Vector3[] maxV = new Vector3[3];
        for(int i = 0; i < 3; ++i){
            minV[i] = new Vector3(0.0f + (0.5f * i) - (isCardInfoTabShown ? 0.0f : 0.5f), 0.0f, 0.0f);
            maxV[i] = new Vector3(0.5f + (0.5f * i) - (isCardInfoTabShown ? 0.0f : 0.5f), 1.0f, 0.0f);
        }

        while((isCardInfoTabShown && minV[0].x > -0.5f) || (!isCardInfoTabShown && minV[0].x < 0.0f)){
            backGroundTransform.anchorMin = minV[0];
            backGroundTransform.anchorMax = maxV[2];
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
        UnitID unitID = currentCard.GetComponent<DeckCard_MJW>().unitID;
        if(unitID.unitTag == UnitTag.Unit){
            int level = gameManager.userInfo.userUnitInfo[unitID.id].level;
            UnitData unit = gameManager.playerUnitTable.unitData[unitID.id];
            int index = gameManager.userInfo.userUnitInfo.FindIndex(x => x.id == unitID.id);
            int unitNumber = gameManager.userInfo.userUnitInfo[index].number;
            int unitUpgradeNumber = 5 * level;

            if(unitNumber >= unitUpgradeNumber){
                gameManager.userInfo.userUnitInfo[index].number -= unitUpgradeNumber;
                gameManager.userInfo.userUnitInfo[index].level++;
                gameManager.SaveData();
            }
            else{

            }
        }
        else if(unitID.unitTag == UnitTag.Special){
            int level = gameManager.userInfo.userSpecialUnitInfo[unitID.id].level;
            SpecialUnitData unit = gameManager.specialUnitTable.specialUnitData[unitID.id];
            int index = gameManager.userInfo.userSpecialUnitInfo.FindIndex(x => x.id == unitID.id);
            int unitNumber = gameManager.userInfo.userSpecialUnitInfo[index].number;
            int unitUpgradeNumber = 5 * level;

            if(unitNumber >= unitUpgradeNumber){
                gameManager.userInfo.userSpecialUnitInfo[index].number -= unitUpgradeNumber;
                gameManager.userInfo.userSpecialUnitInfo[index].level++;
                gameManager.SaveData();
                
            }
            else{

            }
        }
        ShowCurrentUnit(currentCard);
        Transform parent = cardListTab.Find("CardList").Find("Viewport").Find("Content");

        int count = parent.transform.childCount;
            
        for(int i = 0; i < count; ++i){
            DeckCard_MJW child = parent.transform.GetChild(i).GetComponent<DeckCard_MJW>();
            if(child.unitID.id == unitID.id){
                child.GetData();
            }
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
            int selectedIndex = 0, targetIndex = 0;
            UnitID temp;
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
                    bool check = false;
                    for(int i = 0; i < 5; ++i){
                        if(selectedCard.GetComponent<DeckCard_MJW>().unitID.Equals(deckCard.GetChild(i).gameObject.GetComponent<DeckCard_MJW>().unitID)){
                            check = true;
                        }
                        if(System.Object.ReferenceEquals(deckCard.GetChild(i).gameObject, targetCard)){
                            targetIndex = i;
                        }
                    }
                    if(!check){
                        currentDeck.unitIDs[targetIndex] = selectedCard.GetComponent<DeckCard_MJW>().unitID;
                        gameManager.SaveData();
                        ShowCurrentDeck();
                    }
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
        gameManager = GameManager.instance;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        
        backGroundTransform = backGround.GetComponent<RectTransform>();
        deckListTransform = deckListTab.GetComponent<RectTransform>();
        cardListTransform = cardListTab.GetComponent<RectTransform>();
        cardInfoTransform = cardInfoTab.GetComponent<RectTransform>();

        isCardInfoTabShown = false;

        selectedButton = gameManager.userInfo.selectedDeck;

        MakeSlots();
        ShowCurrentDeck();
    }

    private void Start()
    {
        for(int i = 0; i<audios.Length; i++)
        {
            audios[i].volume = gameManager.SoundEffectVolume;
            gameManager.soundEffects.Add(audios[i]);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            startPos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0)){
            endPos = Input.mousePosition;
            endPos -= startPos;
            Debug.Log(endPos.magnitude);
            if(endPos.magnitude < 20.0f){
                selected = ClickSlot();
                if(selected == 0){
                    if(isCardInfoTabShown) StartCoroutine(ChangeTab());
                }
                else if(selected == 1){
                    ShowCurrentDeck();
                }
            }
            
        }
    }

    #endregion
}
