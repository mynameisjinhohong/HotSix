using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInstantAttack", menuName = "ActionBehavior/InstantAttack")]
public class SOInstantAttack : SOActionBase
{
    public override bool Condition(GameObject mainUnit, out List<GameObject> targetObjects, ActionValue values){
        targetObjects = FindTarget(values.range, mainUnit);
        return targetObjects.Count > 0;
    }

    public override void ExecuteAction(float deltaTime, GameObject mainUnit, List<GameObject> targetObjects, ActionValue values){
        TowerHPManager_HJH towerManager = GameObject.Find("TowerHPManager").GetComponent<TowerHPManager_HJH>();
        Unit main = mainUnit.GetComponent<Unit>();
        if(targetObjects.Count == 0) return;
        foreach(GameObject t in targetObjects){
            if(t.CompareTag("Unit")){
                t.GetComponent<Unit>().GetDamage(main.curStat.attackDamage);
            }
            else if(t.CompareTag("Tower")){
                if(main.isEnemy){
                    towerManager.playerTowerHP -= main.curStat.attackDamage;
                }
                else{
                    towerManager.enemyTowerHP -= main.curStat.attackDamage;
                }
            }
        }
    }
}
