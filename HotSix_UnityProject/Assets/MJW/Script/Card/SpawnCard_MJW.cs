using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Localization.Settings;

public class SpawnCard_MJW : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Properties

    public GameManager gameManager;
    public LaneSpawnManager_MJW laneManager;
    public CameraMove_HJH cameraMove;
    
    public GameObject unitPrefab;
    private GameObject tempObject;

    public int id;
    public int cost;
    public Image backgroundImage;
    public Image unitImage;
    public Image tempImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;

    public float curCooldown;
    public float maxCooldown;

    private Vector3 mousePosition;

    #endregion


    #region Methods

    public void SetUnit(int id){
        this.id = id;
        int level = gameManager.userInfo.userUnitInfo[id].level;
        unitPrefab = gameManager.unitPrefabManager.unitPrefabs.playerUnitPrefabs[id];
        cost = gameManager.playerUnitTable.unitData[id].entityInfos.cost;
        maxCooldown = gameManager.playerUnitTable.unitData[id].entityInfos.cooldown;
        levelText.text = "Lv." + level.ToString();
        costText.text = cost.ToString();
        unitImage.sprite = gameManager.unitImages.playerUnitImages[id].iconImage;
        backgroundImage.sprite = gameManager.unitImages.playerUnitImages[id].iconImage;
        backgroundImage.color -= new Color(0.5f, 0.5f, 0.5f, 0.0f);
    }

    public void CountCooldowns(float time){
        curCooldown -= time;
        if(curCooldown < 0.0f) curCooldown = 0.0f;
        unitImage.fillAmount = 1.0f - (curCooldown / maxCooldown);
        tempImage.fillAmount = 1.0f - (curCooldown / maxCooldown);
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        laneManager = GameObject.Find("LaneManager").GetComponent<LaneSpawnManager_MJW>();
        cameraMove = GameObject.Find("Camera").GetComponent<CameraMove_HJH>();
        backgroundImage = transform.Find("BackGround").GetComponent<Image>();
        unitImage = transform.Find("Image").GetComponent<Image>();
        tempImage = transform.Find("TempImage").GetComponent<Image>();
        levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CountCooldowns(Time.deltaTime);
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost) return;

        tempObject = Instantiate(unitPrefab);
        Unit tempUnit = tempObject.GetComponent<Unit>();

        tempUnit.isActive = false;
        tempObject.transform.Rotate(new Vector3(0, 180.0f, 0));

        tempObject.tag = "Untagged";

        mousePosition = Input.mousePosition;
        mousePosition.z = -5.0f;

        tempObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0.0f, 0.0f, 10.0f));

        /*
        // 유닛 색상 변경
        Transform[] allChildren = tempObject.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren){
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            if(sprite != null){
                Color tmp = sprite.color;
                tmp -= new Color(0.5f, 0.5f, 0.5f, 0);
                sprite.color = tmp;
            }
        }
        */

        cameraMove.isActive = false;
    }

    public void OnDrag(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost || tempObject == null) return;

        mousePosition = Input.mousePosition;
        mousePosition.z = -5.0f;

        tempObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0.0f, 0.0f, 10.0f));
    }

    public void OnEndDrag(PointerEventData eventData){
        if(curCooldown > 0.0f || laneManager.moneyManager.money < cost || tempObject == null) return;

        cameraMove.isActive = true;

        GameObject lane = laneManager.CheckUnitToLane(tempObject);
        
        if(lane != null){
            laneManager.SpawnPlayerUnit(lane, id);
            curCooldown = maxCooldown;
        }

        Destroy(tempObject);
    }

    #endregion
}
