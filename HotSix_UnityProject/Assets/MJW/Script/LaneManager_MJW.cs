using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager_MJW : MonoBehaviour
{
    #region Properties

    public GameManager gameManager;
    public SpawnButton_MJW spawnButton;
    [HideInInspector]
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

    public float RandomY(GameObject lane, GameObject unit){
        float scale = lane.transform.lossyScale.y / 2.0f;
        return lane.transform.position.y + Random.Range(-scale, scale) + (unit.transform.lossyScale.y / 2.0f);
    }

    public void SpawnPlayerUnit(GameObject lane){
        if(spawnButton.selectedIndex != null){
            GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(spawnButton.unitPrefabsID[(int)spawnButton.selectedIndex]);
            UnitObject_MJW unit = unitInstance.GetComponent<UnitObject_MJW>();

            // 유닛 초기 세팅
            unitInstance.transform.position = new Vector3(lane.transform.position.x - (lane.transform.lossyScale.x / 2.0f), RandomY(lane, unitInstance), -0.2f);
            unitInstance.transform.SetParent(lane.transform);

            spawnButton.moneyManager.money -= spawnButton.moneys[(int)spawnButton.selectedIndex];

            // 버튼 초기화
            spawnButton.currentCooldowns[(int)spawnButton.selectedIndex] = spawnButton.cooldowns[(int)spawnButton.selectedIndex];
            spawnButton.selectedIndex = null;
        }
    }

    public void SpawnEnemyUnit(int laneIndex, int enemyUnitID){
        GameObject lane = lanes[laneIndex];
        GameObject unitInstance = gameManager.unitPrefabManager.Instantiate(enemyUnitID);
        UnitObject_MJW unit = unitInstance.GetComponent<UnitObject_MJW>();

        // 유닛 초기 세팅
        unit.isEnemy = true;
        unitInstance.transform.SetParent(lane.transform);
        unitInstance.transform.Rotate(new Vector3(0, 180.0f, 0));
        unitInstance.transform.position = new Vector3(lane.transform.position.x + (lane.transform.lossyScale.x / 2.0f), RandomY(lane, unitInstance), -0.2f);
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
