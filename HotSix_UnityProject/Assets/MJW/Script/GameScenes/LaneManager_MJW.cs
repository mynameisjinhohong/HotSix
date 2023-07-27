using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public SpawnButton_MJW spawnButton;
    public GameObject[] lanes;
    private RaycastHit[] hits;

    #endregion


    #region Methods

    public GameObject ClickLane(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hits = Physics.RaycastAll(ray);
        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider.tag == "Lane"){
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public Vector3 GetLaneSize(GameObject lane){
        Vector2 laneSpriteSize = lane.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localLaneSize = laneSpriteSize / lane.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 worldLaneSize = localLaneSize;
        worldLaneSize.x *= lane.transform.lossyScale.x;
        worldLaneSize.y *= lane.transform.lossyScale.y;
        return worldLaneSize;
    }

    public float RandomY(GameObject lane, float height, GameObject unit){
        return lane.transform.position.y + Random.Range(0.1f, height / 2.0f - 0.2f) + (unit.transform.lossyScale.y / 2.0f);
    }

    public void SpawnPlayerUnit(GameObject lane){
        if(spawnButton.selectedIndex != null){
            int index = (int)spawnButton.selectedIndex;
            gameManager.unitPrefabManager.SetLevel(spawnButton.unitPrefabsID[index], gameManager.userInfo.userUnitInfo[spawnButton.unitPrefabsID[index]].level, false);
            GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(spawnButton.unitPrefabsID[index], false);
            Unit unit = unitInstance.GetComponent<Unit>();

            // 유닛 초기 세팅
            Vector3 laneSize = GetLaneSize(lane);
            unitInstance.transform.position = new Vector3(lane.transform.position.x - (laneSize.x / 2.0f), RandomY(lane, laneSize.y, unitInstance), lane.transform.position.z - 0.1f);
            unitInstance.transform.SetParent(lane.transform);

            spawnButton.moneyManager.money -= spawnButton.moneys[index];

            // 버튼 초기화
            spawnButton.currentCooldowns[index] = spawnButton.cooldowns[index];
            spawnButton.selectedIndex = null;
        }
    }

    public void SpawnEnemyUnit(int laneIndex, int enemyUnitID, int enemyUnitLevel = 1){
        GameObject lane = lanes[laneIndex];
        gameManager.unitPrefabManager.SetLevel(enemyUnitID, enemyUnitLevel, true);
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(enemyUnitID, true);
        Unit unit = unitInstance.GetComponent<Unit>();

        // 유닛 초기 세팅
        unit.isEnemy = true;
        unitInstance.transform.Rotate(new Vector3(0, 180.0f, 0));

        Vector3 laneSize = GetLaneSize(lane);
        unitInstance.transform.position = new Vector3(lane.transform.position.x + (laneSize.x / 2.0f), RandomY(lane, laneSize.y, unitInstance), lane.transform.position.z - 0.1f);
        unitInstance.transform.SetParent(lane.transform);
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        int count = transform.childCount;
        lanes = new GameObject[count];
        for(int i = 0; i < count; ++i){
            lanes[i] = transform.GetChild(i).gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnButton.selectedIndex != null && Input.GetMouseButtonDown(0)){
            GameObject lane = ClickLane();
            Debug.Log(lane);
            if(lane != null){
                SpawnPlayerUnit(lane);
            }
            else{
                spawnButton.selectedIndex = null;
            } 
        }
    }

    #endregion
}
