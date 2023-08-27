using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeckCard_MJW: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Properties

    public GameManager gameManager;
    private EditDeckManager_MJW editDeckManager;
    private Transform canvas;
    private CanvasGroup canvasGroup;

    public ScrollRect parentScroll;

    public GameObject copyObject;
    private GameObject tempObject;

    public UnitID unitID;
    public Image iconImage;
    public GameObject arrow;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public GameObject glow;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    public bool isClicked = false;
    public bool isLongClicked = false;
    public bool isScrollDrag = false;
    private float longClickTimer = 0.0f;

    #endregion 


    #region Methods

    public void GetData(){
        int level = 0;
        int unitNumber = 0;
        int unitUpgradeNumber = 0;
        if(unitID.unitTag == UnitTag.Unit){
            level = gameManager.userInfo.userUnitInfo[unitID.id].level;
            unitNumber = gameManager.userInfo.userUnitInfo[unitID.id].number;
            unitUpgradeNumber = editDeckManager.upgradeCost * level;

            iconImage.sprite = gameManager.unitImages.playerUnitImages[unitID.id].moneySpace_Icon;
            levelText.text = "Lv." + level.ToString();
            costText.text = gameManager.playerUnitTable.unitData[unitID.id].entityInfos.cost.ToString();
        }
        else if(unitID.unitTag == UnitTag.Special){
            level = gameManager.userInfo.userSpecialUnitInfo[unitID.id].level;
            unitNumber = gameManager.userInfo.userSpecialUnitInfo[unitID.id].number;
            unitUpgradeNumber = editDeckManager.upgradeCost * level;

            iconImage.sprite = gameManager.unitImages.specialUnitImages[unitID.id].moneySpace_Icon;
            levelText.text = "Lv." + level.ToString();
            costText.text = gameManager.specialUnitTable.specialUnitData[unitID.id].entityInfos.cost.ToString();
        }
        
        arrow.SetActive(unitNumber >= unitUpgradeNumber && level < 5);
    }

    public GameObject GetTargetCard(){
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();

        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; ++i){
            GameObject hit = results[i].gameObject;
            if(hit.transform.parent.CompareTag("Card")){
                return hit.transform.parent.gameObject;
            }
        }

        return null;
    }

    public bool CheckMouseInCard(){
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();

        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; ++i){
            GameObject hit = results[i].gameObject;
            if(System.Object.ReferenceEquals(transform, hit.transform.parent)){
                return true;
            }
        }

        return false;
    }

    public void ChangeColor(bool darker){
        RectTransform[] allChildren = transform.GetComponentsInChildren<RectTransform>();
        foreach(RectTransform child in allChildren){
            if(child.TryGetComponent<Image>(out var image))
            {
                Color tmp = image.color;
                if(darker){
                    tmp -= new Color(0.5f, 0.5f, 0.5f, 0);
                }
                else{
                    tmp += new Color(0.5f, 0.5f, 0.5f, 0);
                }
                image.color = tmp;
            }
        }
    }

    public void EnableTemp(){
        editDeckManager.selectedCard = transform.gameObject;

        tempObject = Instantiate(copyObject);
        RectTransform rect = tempObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(250.0f, 250.0f);

        tempObject.transform.position = Input.mousePosition;
        tempObject.transform.SetParent(canvas);
        tempObject.transform.SetAsLastSibling();

        canvasGroup = tempObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        editDeckManager.isDragable = false;
    }

    public void ResetClick(){
        if(!isClicked) return;

        ChangeColor(false);

        isClicked = false;
        longClickTimer = 0.0f;
        isLongClicked = false;
        isScrollDrag = false;
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        editDeckManager = GameObject.Find("EditDeckManager").GetComponent<EditDeckManager_MJW>();
        canvas = FindObjectOfType<Canvas>().transform;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        iconImage = transform.Find("Image").GetComponent<Image>();
        arrow = transform.Find("Arrow").gameObject;
        levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
        glow = transform.Find("Glow_effect").gameObject;

        glow.SetActive(false);
        if(transform.parent != null) parentScroll = transform.parent.parent.GetComponent<ScrollRect>();
    }

    void Update(){
        if(isClicked){
            longClickTimer += Time.deltaTime;
            if(!CheckMouseInCard()){
                ResetClick();
            }
            if(longClickTimer > 0.5f) isLongClicked = true;
        }
        if(isLongClicked){
            if(!editDeckManager.isCardInfoTabShown && tempObject == null) EnableTemp();
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        ChangeColor(true);
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        ResetClick();

        if(tempObject != null) Destroy(tempObject);
    }
    
    public void OnPointerClick(PointerEventData eventData){
        if(!isScrollDrag){
            editDeckManager.selectedCard = transform.gameObject;
            editDeckManager.targetCard = GetTargetCard();
            editDeckManager.GetEvent();
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(!isLongClicked){
            if(parentScroll != null){
                isScrollDrag = true;
            }
        }
        if(isScrollDrag){
            parentScroll.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData){
        if(isScrollDrag){
            if(parentScroll != null){
                parentScroll.OnDrag(eventData);
            }
        }
        else{
            if(tempObject == null) return;

            tempObject.transform.position = eventData.position;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData){
        if(isScrollDrag){
            if(parentScroll != null){
                parentScroll.OnEndDrag(eventData);
            }
        }
        else{
            if(tempObject == null) return;

            editDeckManager.targetCard = GetTargetCard();
            editDeckManager.GetEvent();
        }
        
    }

    #endregion
}
