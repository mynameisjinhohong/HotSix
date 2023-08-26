using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SODoubleAttack", menuName = "ActionBehavior/DoubleAttack")]
public class SODoubleAttack : SOActionBase
{
    public float firstAction = 0.3f;
    public float secondAction = 0.6f;

    public override bool Condition(Action_MJW action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count > 0;
    }

    public override IEnumerator ExecuteAction(Action_MJW action){
        yield return new WaitForSeconds(action.cooldown * firstAction);
        if (action.mainUnit != null) 
        {
            Attack(action);
            if(action.mainUnit.GetComponent<Unit>().state != Unit.UnitState.Die && action.audio.clip != null)
            {
                action.audio.Play();
            }
        }
        yield return new WaitForSeconds(action.cooldown * (secondAction - firstAction));
        if (action.mainUnit != null) 
        {
            if(action.mainUnit.GetComponent<Unit>().state != Unit.UnitState.Die && action.audio.clip != null){
                action.audio.Play();
            }
        }
        yield break;
    }

    public void Attack(Action_MJW action){
        Unit unit = action.mainUnit.GetComponent<Unit>();
        if(action.targetObjects.Count == 0) return;
        foreach(GameObject t in action.targetObjects){
            if(t == null) continue;
            if(t.CompareTag("Unit")){
                t.GetComponent<Unit>().GetDamage(action.value);
            }
            else if(t.CompareTag("Tower")){
                if(unit.isEnemy){
                    action.towerManager.playerTowerHP -= action.value;
                }
                else{
                    action.towerManager.enemyTowerHP -= action.value;
                }
            }
        }
    }
}
