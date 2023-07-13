using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card_MJW: MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Properties

    public GameManager gameManager;
    private EditDeckManager_MJW editDeckManager;
    private Transform canvas;
    private CanvasGroup canvasGroup;

    public GameObject copyObject;
    private GameObject tempObject;

    public int id;
    public TextMeshProUGUI nameText;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    #endregion 


    #region Methods

    public GameObject GetTargetCard(){
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        Debug.Log("Get Target");
        raycaster.Raycast(pointerEventData, results);
        for(int i = 0; i < results.Count; ++i){
            GameObject hit = results[i].gameObject;
            Debug.Log("" + hit.name);
            if(hit.tag == "Card"){
                return hit;
            }
        }

        return null;
    }

    public void GetNameText(){
        nameText.text = gameManager.unitPrefabManager.unitPrefabs[id].GetComponent<UnitObject_MJW>().unit.unitInfo.k_name;
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        editDeckManager = GameObject.Find("EditDeckManager").GetComponent<EditDeckManager_MJW>();
        canvas = FindObjectOfType<Canvas>().transform;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
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
