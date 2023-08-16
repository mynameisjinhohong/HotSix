using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInstantAttack", menuName = "ActionBehavior/InstantAttack")]
public class SOInstantAttack : SOActionBase
{
    public override bool Condition(Action action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count > 0;
    }

    public override IEnumerator ExecuteAction(Action action){
        yield return new WaitForSeconds(action.cooldown * 0.5f);
        if(action.mainUnit != null) Attack(action);
        yield break;
    }

    public void Attack(Action action){
        TowerHPManager_HJH towerManager = GameObject.Find("TowerHPManager").GetComponent<TowerHPManager_HJH>();
        Unit unit = action.mainUnit.GetComponent<Unit>();
        if(action.targetObjects.Count == 0) return;
        foreach(GameObject t in action.targetObjects){
            if(t == null) continue;
            if(t.CompareTag("Unit")){
                t.GetComponent<Unit>().GetDamage(action.value);
            }
            else if(t.CompareTag("Tower")){
                if(unit.isEnemy){
                    towerManager.playerTowerHP -= action.value;
                }
                else{
                    towerManager.enemyTowerHP -= action.value;
                }
            }
        }
    }
}
