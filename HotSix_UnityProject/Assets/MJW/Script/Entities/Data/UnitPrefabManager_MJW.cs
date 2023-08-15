using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPrefabManager_MJW
{
    public UnitPrefabData unitPrefabs;

    public void LinkPrefabs(UnitTable playerUnits, UnitTable enemyUnits, SpecialUnitTable specialUnits){
        for(int i = 1; i < playerUnits.unitData.Count; ++i){
            if(i >= unitPrefabs.playerUnitPrefabs.Count) break;
            Unit unit = unitPrefabs.playerUnitPrefabs[i].GetComponent<Unit>();
            unit.unitData = playerUnits.unitData[i];
        }
        for(int i = 1; i < enemyUnits.unitData.Count; ++i){
            if(i >= unitPrefabs.enemyUnitPrefabs.Count) break;
            Unit unit = unitPrefabs.enemyUnitPrefabs[i].GetComponent<Unit>();
            unit.unitData = enemyUnits.unitData[i];
        }
        for(int i = 1; i < specialUnits.specialUnitData.Count; ++i){
            if(i >= unitPrefabs.specialUnitPrefabs.Count) break;
            unitPrefabs.specialUnitPrefabs[i].GetComponent<SpecialUnit>().unitData = specialUnits.specialUnitData[i];
        }
    }

    public void SetLevel(UnitID unitID, int level, bool isEnemy){
        if(unitID.unitTag == UnitTag.Special){
            unitPrefabs.specialUnitPrefabs[unitID.id].GetComponent<Entity>().level = level;
        }
        else if(isEnemy){
            unitPrefabs.enemyUnitPrefabs[unitID.id].GetComponent<Entity>().level = level;
        }
        else{
            unitPrefabs.playerUnitPrefabs[unitID.id].GetComponent<Entity>().level = level;
        }
    }

    public GameObject Instantiate(UnitID unitID, bool isEnemy){
        if(unitID.id == 0
            || (isEnemy && unitID.id >= unitPrefabs.enemyUnitPrefabs.Count)
            || (!isEnemy && unitID.id >= unitPrefabs.playerUnitPrefabs.Count)
            || (unitID.unitTag == UnitTag.Special && unitID.id >= unitPrefabs.specialUnitPrefabs.Count))
            return null;
        if(unitID.unitTag == UnitTag.Special){
            return Object.Instantiate(unitPrefabs.specialUnitPrefabs[unitID.id], new Vector3(-200.0f, 0.0f, -200.0f), Quaternion.identity);
        }
        if(isEnemy){
            return Object.Instantiate(unitPrefabs.enemyUnitPrefabs[unitID.id], new Vector3(-200.0f, 0.0f, -200.0f), Quaternion.identity);
        }
        else{
            return Object.Instantiate(unitPrefabs.playerUnitPrefabs[unitID.id], new Vector3(-200.0f, 0.0f, -200.0f), Quaternion.identity);
        }
    }

    public void Destroy(GameObject obj){
        if(obj == null) return;
        Object.Destroy(obj);
    }
}
