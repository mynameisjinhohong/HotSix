using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEngine;

public class LaneSpawnManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public MoneyManager_HJH moneyManager;
    public MapManager_HJH mapManager;

    public GameObject laneSlot;
    public GameObject spawnButtonSlot;
    public GameObject spawnButtonPrefab;

    public GameObject[] lanes;
    private RaycastHit[] hits;

    #endregion


    #region Methods

    public GameObject CheckMouseToLane(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.RaycastNonAlloc(ray, hits);
        
        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider == null) continue;

            if(hit.collider.CompareTag("Lane")){
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public GameObject CheckUnitToLane(GameObject unit){
        Collider unitCollider = unit.GetComponent<Collider>();
        Vector3 unitCenter = unitCollider.bounds.center;
        Vector3 unitSize = unitCollider.bounds.size;

        hits = Physics.BoxCastAll(unitCenter, unitSize / 2.0f, -unit.transform.forward, Quaternion.identity, 20.0f)
                                .OrderBy(h => h.distance).ToArray();

        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider == null) continue;

            if(hit.collider.CompareTag("Lane")){
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    public Vector3 GetLaneSize(GameObject lane){
        Vector3 laneColSize = lane.GetComponent<BoxCollider>().size;
        Vector3 worldLaneSize = laneColSize;
        //Vector2 laneSpriteSize = lane.GetComponent<SpriteRenderer>().sprite.rect.size;
        //Vector2 localLaneSize = laneSpriteSize / lane.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        //Vector3 worldLaneSize = localLaneSize;
        worldLaneSize.x *= lane.transform.lossyScale.x;
        worldLaneSize.y *= lane.transform.lossyScale.y;
        return worldLaneSize;
    }

    public float RandomY(GameObject lane, float height, GameObject unit){
        Collider unitCollider = unit.GetComponent<Collider>();
        Vector3 unitCenter = unitCollider.bounds.center;
        Vector3 unitSize = unitCollider.bounds.size;

        return lane.transform.position.y + Random.Range(-((height / 2.0f) - (height / 2.0f * 0.5f)), (height / 2.0f) - (height / 2.0f * 0.5f)) + (unitSize.y / 2.0f);
    }

    public void SpawnPlayerUnit(GameObject lane, UnitID unitID){
        if(moneyManager.money < gameManager.playerUnitTable.unitData[unitID.id].entityInfos.cost) return;

        gameManager.unitPrefabManager.SetLevel(unitID, gameManager.userInfo.userUnitInfo[unitID.id].level, false);
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(unitID, false);
        Unit unit = unitInstance.GetComponent<Unit>();

        // 유닛 초기 세팅
        unitInstance.tag = "Unit";
        unit.isEnemy = false;
        unitInstance.transform.Rotate(new Vector3(0, 180.0f, 0));

        Transform playerTower = lane.transform.Find("PlayerTowerCollider").transform;
        Vector3 laneSize = GetLaneSize(lane);
        float randomY = RandomY(lane, laneSize.y, unitInstance);
        unitInstance.transform.position = new Vector3(playerTower.position.x - 1.0f, randomY, lane.transform.position.z - 0.05f + randomY * 0.1f);
        unitInstance.transform.SetParent(lane.transform);

        moneyManager.money -= unit.unitData.entityInfos.cost;
    }

    public void SpawnSpecialUnit(UnitID unitID){
        if(moneyManager.money < gameManager.playerUnitTable.unitData[unitID.id].entityInfos.cost) return;

        gameManager.unitPrefabManager.SetLevel(unitID, gameManager.userInfo.userUnitInfo[unitID.id].level, false);
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(unitID, false);
        SpecialUnit unit = unitInstance.GetComponent<SpecialUnit>();

        // 유닛 초기 세팅
        unitInstance.tag = "SpecialUnit";
        unit.isEnemy = false;
        unitInstance.transform.Rotate(new Vector3(0, 180.0f, 0));

        Transform playerTower = GameObject.Find("TowerHPManager").transform.Find("PlayerTower").transform;
        unitInstance.transform.position = new Vector3(playerTower.position.x, 2.5f, -0.2f);

        unit.state = Entity.UnitState.Action;

        moneyManager.money -= unit.unitData.entityInfos.cost;
    }

    public void SpawnEnemyUnit(int laneIndex, int enemyID, int enemyUnitLevel = 1){
        UnitID unitID = new(){
            unitTag = UnitTag.Unit,
            id = enemyID
        };
        GameObject lane = lanes[laneIndex];
        gameManager.unitPrefabManager.SetLevel(unitID, enemyUnitLevel, true);
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(unitID, true);
        Unit unit = unitInstance.GetComponent<Unit>();

        // 유닛 초기 세팅
        unitInstance.tag = "Unit";
        unit.isEnemy = true;
        
        Transform enemyTower = lane.transform.Find("EnemyTowerCollider").transform;
        Vector3 laneSize = GetLaneSize(lane);
        float randomY = RandomY(lane, laneSize.y, unitInstance);
        // unitInstance.transform.position = new Vector3(lane.transform.position.x + (laneSize.x / 2.0f), randomY, lane.transform.position.z - 0.05f + randomY * 0.1f);
        unitInstance.transform.position = new Vector3(enemyTower.position.x + 1.0f, randomY, lane.transform.position.z - 0.05f + randomY * 0.1f);
        unitInstance.transform.SetParent(lane.transform);
    }

    public void SetButtons(){
        for(int i = 0; i < 5; ++i){
            GameObject slot = Instantiate(spawnButtonPrefab);
            slot.transform.SetParent(spawnButtonSlot.transform);
            SpawnCard_MJW spawnButton = slot.GetComponent<SpawnCard_MJW>();
            spawnButton.SetUnit(gameManager.currentDeck.unitIDs[i]);
        }
    }

    #endregion


    #region Monobehavior Callbacks

    void Start(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager_HJH>();
        int count = GameManager.instance.mapElements[GameManager.instance.stage].lineCount;
        lanes = mapManager.lines;
        for(int i = 0; i < count; ++i){
            lanes[i].tag = "Lane";
        }
        SetButtons();
    }

    #endregion
}
