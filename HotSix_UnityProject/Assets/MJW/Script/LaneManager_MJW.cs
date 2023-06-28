using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager_MJW : MonoBehaviour
{
    public SpawnButton_MJW spawnButton;
    public GameObject[] lanes;

    private RaycastHit[] hits;

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
        if(spawnButton.selectedUnit != null){
            GameObject unitInstance = Instantiate(spawnButton.selectedUnit);
            Unit_MJW unit = unitInstance.GetComponent<Unit_MJW>();
            unitInstance.transform.position = new Vector3(lane.transform.position.x - (lane.transform.lossyScale.x / 2.0f), RandomY(lane, unitInstance), -0.2f);
            unitInstance.transform.SetParent(lane.transform);
            spawnButton.moneyManager.money -= unit.currentStat.cost;
            spawnButton.selectedUnit = null;
            spawnButton.selectedButton = null;
        }
    }

    public void SpawnEnemyUnit(int laneIndex, GameObject enemy){
        GameObject lane = lanes[laneIndex];
        GameObject unitInstance = Instantiate(enemy);
        Unit_MJW unit = unitInstance.GetComponent<Unit_MJW>();
        unit.isEnemy = true;
        unitInstance.transform.SetParent(lane.transform);
        unitInstance.transform.Rotate(new Vector3(0, 180.0f, 0));
        unitInstance.transform.position = new Vector3(lane.transform.position.x + (lane.transform.lossyScale.x / 2.0f), RandomY(lane, unitInstance), -0.2f);
    }

    void Awake(){
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
        if(spawnButton.selectedButton != null && Input.GetMouseButtonDown(0)){
            GameObject lane = ClickLane();
            if(lane != null){
                SpawnPlayerUnit(lane);
            }
            else{
                spawnButton.selectedButton = null;
                spawnButton.selectedUnit = null;
            } 
        }
    }
}
