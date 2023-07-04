using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitPrefabManager_MJW
{
    public List<GameObject> unitPrefabs;

    public void LinkPrefabs(List<Unit_MJW> unitDataList){
        for(int i = 1; i < unitDataList.Count; ++i){
            if(i >= unitPrefabs.Count) break;
            unitPrefabs[i].GetComponent<UnitObject_MJW>().unit = unitDataList[i];
        }
    }

    public void SetLevel(int id, int level){
        unitPrefabs[id].GetComponent<UnitObject_MJW>().level = level;
    }

    public GameObject Instantiate(int id){
        if(id == 0 || id >= unitPrefabs.Count) return null;
        return Object.Instantiate(unitPrefabs[id]);
    }

    public void Destroy(GameObject obj){
        if(obj == null) return;
        Object.Destroy(obj);
    }
}
