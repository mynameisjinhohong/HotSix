using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOProjectileInstantAttack", menuName = "ProjectileBehavior/InstantAttack")]
public class SOProjectileInstantAttack : SOProjectileActionBase
{
    public override bool Condition(ProjectileAction action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count > 0;
    }

    public override void ExecuteAction(ProjectileAction action){
        TowerHPManager_HJH towerManager = GameObject.Find("TowerHPManager").GetComponent<TowerHPManager_HJH>();
        if(action.targetObjects.Count == 0) return;
        foreach(GameObject t in action.targetObjects){
            if(t.CompareTag("Unit")){
                t.GetComponent<Unit>().GetDamage(action.value);
                if(!applySplash){
                    Destroy(action.mainProjectile);
                    return;
                } 
            }
            else if(t.CompareTag("Tower")){
                if(action.mainProjectile.GetComponent<Projectile>().isEnemy){
                    towerManager.playerTowerHP -= action.value;
                    if(!applySplash){
                    Destroy(action.mainProjectile);
                    return;
                } 
                }
                else{
                    towerManager.enemyTowerHP -= action.value;
                    if(!applySplash){
                    Destroy(action.mainProjectile);
                    return;
                } 
                }
            }
        }
        Destroy(action.mainProjectile);
        return;
    }
}
