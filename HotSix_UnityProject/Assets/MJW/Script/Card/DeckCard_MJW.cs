using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeckCard_MJW: MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Properties

    public GameManager gameManager;
    private EditDeckManager_MJW editDeckManager;
    private Transform canvas;
    private CanvasGroup canvasGroup;

    public GameObject copyObject;
    private GameObject tempObject;

    public int id;
    public Image iconImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    #endregion 


    #region Methods

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

    public void GetData(){
        iconImage.sprite = gameManager.unitImages.playerUnitImages[id].iconImage;
        levelText.text = "Lv." + gameManager.userInfo.userUnitInfo[id].level.ToString();
        costText.text = gameManager.playerUnitTable.unitData[id].unitStats.cost.ToString();
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
        levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData){
        editDeckManager.selectedCard = transform.gameObject;
        editDeckManager.GetEvent();
    }

    public void OnBeginDrag(PointerEventData eventData){
        editDeckManager.selectedCard = transform.gameObject;

        tempObject = Instantiate(copyObject);
        RectTransform rect = tempObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200.0f, 200.0f);

        tempObject.transform.position = eventData.position;
        tempObject.transform.SetParent(canvas);
        tempObject.transform.SetAsLastSibling();

        canvasGroup = tempObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        tempObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
        Destroy(tempObject);
        editDeckManager.targetCard = GetTargetCard();
        editDeckManager.GetEvent();
    }

    #endregion
}
