using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOProjectileInstantAttack", menuName = "ProjectileBehavior/InstantAttack")]
public class SOProjectileInstantAttack : SOProjectileActionBase
{
    public override bool Condition(ProjectileAction action){
        if(action.targetObjects[0].CompareTag("Unit") && (action.lane == action.targetObjects[0].transform.parent)){
            Unit target = action.targetObjects[0].GetComponent<Unit>();

            if(target.isEnemy != action.isEnemy){
                return true;
            }
        }
        else if(action.targetObjects[0].CompareTag("Tower")){
            if((action.isEnemy && action.targetObjects[0].name == "PlayerTower") || (!action.isEnemy && action.targetObjects[0].name == "EnemyTower")){
                return true;
            }
        }
        return false;
    }

    public override void ExecuteAction(ProjectileAction action){
        TowerHPManager_HJH towerManager = GameObject.Find("TowerHPManager").GetComponent<TowerHPManager_HJH>();
        foreach(GameObject t in action.targetObjects){
            if(t.CompareTag("Unit")){
                t.GetComponent<Unit>().GetDamage(action.value);
            }
            else if(t.CompareTag("Tower")){
                if(action.mainProjectile.GetComponent<Projectile>().isEnemy){
                    towerManager.playerTowerHP -= action.value;
                }
                else{
                    towerManager.enemyTowerHP -= action.value;
                }
            }
        }
    }
}
