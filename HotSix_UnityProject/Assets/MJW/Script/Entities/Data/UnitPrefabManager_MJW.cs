using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPrefabManager_MJW
{
    public UnitPrefabData unitPrefabs;
    public SOActionBase defaultMove;

    public void LinkPrefabs(UnitTable playerUnits, UnitTable enemyUnits, SpecialUnitTable specialUnits){
        for(int i = 1; i < playerUnits.unitData.Count; ++i){
            if(i >= unitPrefabs.playerUnitPrefabs.Count) break;
            Unit unit = unitPrefabs.playerUnitPrefabs[i].GetComponent<Unit>();
            unit.unitData = playerUnits.unitData[i];
            unit.moveBehavior.action = defaultMove;
        }
        for(int i = 1; i < enemyUnits.unitData.Count; ++i){
            if(i >= unitPrefabs.enemyUnitPrefabs.Count) break;
            Unit unit = unitPrefabs.enemyUnitPrefabs[i].GetComponent<Unit>();
            unit.unitData = enemyUnits.unitData[i];
            unit.moveBehavior.action = defaultMove;
        }
        /*
        for(int i = 1; i < specialUnits.specialUnitData.Count; ++i){
            if(i >= unitPrefabs.specialUnitPrefabs.Count) break;
            unitPrefabs.specialUnitPrefabs[i].GetComponent<SpecialUnit>().unitData = specialUnits.specialUnitData[i];
        }
        */
    }

    public void SetLevel(int id, int level, bool isEnemy){
        if(isEnemy){
            unitPrefabs.enemyUnitPrefabs[id].GetComponent<Unit>().level = level;
        }
        else{
            unitPrefabs.playerUnitPrefabs[id].GetComponent<Unit>().level = level;
        }
    }

    public GameObject Instantiate(int id, bool isEnemy){
        if(id == 0
            || (isEnemy && id >= unitPrefabs.enemyUnitPrefabs.Count)
            || (!isEnemy && id >= unitPrefabs.playerUnitPrefabs.Count))
            return null;
        if(isEnemy){
            return Object.Instantiate(unitPrefabs.enemyUnitPrefabs[id], new Vector3(-200.0f, 0.0f, -200.0f), Quaternion.identity);
        }
        else{
            return Object.Instantiate(unitPrefabs.playerUnitPrefabs[id], new Vector3(-200.0f, 0.0f, -200.0f), Quaternion.identity);
        }
    }

    public void Destroy(GameObject obj){
        if(obj == null) return;
        Object.Destroy(obj);
    }
}
