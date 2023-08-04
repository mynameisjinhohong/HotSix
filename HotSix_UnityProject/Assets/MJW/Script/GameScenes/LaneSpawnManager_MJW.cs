using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneSpawnManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public MoneyManager_HJH moneyManager;

    public GameObject laneSlot;
    public GameObject spawnButtonSlot;
    public GameObject spawnButtonPrefab;

    public GameObject[] lanes;
    private RaycastHit[] hits;

    #endregion


    #region Methods

    public GameObject CheckMouseToLane(){
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

    public GameObject CheckUnitToLane(GameObject unit){
        Collider unitCollider = unit.GetComponent<Collider>();
        Vector3 unitCenter = unitCollider.bounds.center;
        Vector3 unitSize = unitCollider.bounds.size;

        hits = Physics.BoxCastAll(unitCenter, unitSize / 2.0f, -unit.transform.forward, Quaternion.identity, 20.0f)
                                .OrderBy(h => h.distance).ToArray();
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
        Collider unitCollider = unit.GetComponent<Collider>();
        Vector3 unitCenter = unitCollider.bounds.center;
        Vector3 unitSize = unitCollider.bounds.size;

        return lane.transform.position.y + Random.Range(0.1f, height / 2.0f - 0.2f) + (unitSize.y / 2.0f - (unitCenter.y - transform.position.y));
    }

    public void SpawnPlayerUnit(GameObject lane, int unitID){
        if(moneyManager.money < gameManager.playerUnitTable.unitData[unitID].unitStats.cost) return;

        gameManager.unitPrefabManager.SetLevel(unitID, gameManager.userInfo.userUnitInfo[unitID].level, false);
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(unitID, false);
        Unit unit = unitInstance.GetComponent<Unit>();

        // 유닛 초기 세팅
        unitInstance.tag = "Unit";
        unit.isEnemy = false;
        unitInstance.transform.Rotate(new Vector3(0, 180.0f, 0));

        Vector3 laneSize = GetLaneSize(lane);
        float randomY = RandomY(lane, laneSize.y, unitInstance);
        unitInstance.transform.position = new Vector3(lane.transform.position.x - (laneSize.x / 2.0f), randomY, lane.transform.position.z - 0.05f + randomY * 0.1f);
        unitInstance.transform.SetParent(lane.transform);

        moneyManager.money -= unit.curStat.cost;
    }

    public void SpawnEnemyUnit(int laneIndex, int enemyUnitID, int enemyUnitLevel = 1){
        GameObject lane = lanes[laneIndex];
        gameManager.unitPrefabManager.SetLevel(enemyUnitID, enemyUnitLevel, true);
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(enemyUnitID, true);
        Unit unit = unitInstance.GetComponent<Unit>();

        // 유닛 초기 세팅
        unitInstance.tag = "Unit";
        unit.isEnemy = true;
        

        Vector3 laneSize = GetLaneSize(lane);
        float randomY = RandomY(lane, laneSize.y, unitInstance);
        unitInstance.transform.position = new Vector3(lane.transform.position.x + (laneSize.x / 2.0f), randomY, lane.transform.position.z - 0.05f + randomY * 0.1f);
        unitInstance.transform.SetParent(lane.transform);
    }

    public void SetButtons(){
        for(int i = 0; i < 5; ++i){
            GameObject slot = Instantiate(spawnButtonPrefab);
            slot.transform.SetParent(spawnButtonSlot.transform);
            SpawnButton_MJW spawnButton = slot.GetComponent<SpawnButton_MJW>();
            spawnButton.SetUnit(gameManager.currentDeck.unitIDs[i]);
        }
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager_HJH>();

        int count = laneSlot.transform.childCount;
        lanes = new GameObject[count];
        for(int i = 0; i < count; ++i){
            lanes[i] = laneSlot.transform.GetChild(i).gameObject;
            lanes[i].tag = "Lane";
        }

        SetButtons();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion
}
