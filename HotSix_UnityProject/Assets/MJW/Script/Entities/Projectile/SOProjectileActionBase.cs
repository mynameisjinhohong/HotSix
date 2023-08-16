using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileAction : ICloneable
{
    public SOProjectileActionBase action;

    [HideInInspector]
    public GameObject mainUnit;
    [HideInInspector]
    public GameObject mainProjectile;
    [HideInInspector]
    public List<GameObject> targetObjects;
    public GameObject lane;
    [HideInInspector]
    public Collider[] hitSplashs;

    public float duration;
    public float value;
    public bool isEnemy;

    public object Clone(){
        ProjectileAction action = new(){
            action = this.action,
            mainUnit = mainUnit,
            mainProjectile = mainProjectile,
            targetObjects = targetObjects,
            lane = lane,
            hitSplashs = hitSplashs,
            duration = duration,
            value = value,
            isEnemy = isEnemy
        };
        return action;
    }

    public bool Condition(){
        return action.Condition(this);
    }

    public void ExecuteAction(){
        action.ExecuteAction(this);
    }
}

public abstract class SOProjectileActionBase : ScriptableObject
{
    #region Properties

    public bool applySplash = false;
    public bool applyToAllies = false;
    public bool applyToTower = true;

    #endregion

    
    #region Methods

    public abstract bool Condition(ProjectileAction action);

    public abstract void ExecuteAction(ProjectileAction action);

    public List<GameObject> FindTarget(ProjectileAction action){
        List<GameObject> targetObjects = new();

        Projectile mainComp = action.mainProjectile.GetComponent<Projectile>();
        Collider mainCollider = action.mainProjectile.GetComponent<Collider>();

        Vector3 center = mainCollider.bounds.center;

        action.hitSplashs = Physics.OverlapBox(center, mainCollider.bounds.size / 2.0f, Quaternion.identity);

        foreach(Collider h in action.hitSplashs){
            if(System.Object.ReferenceEquals(action.mainUnit, h)) continue;
            else if(h.CompareTag("Unit") && (h.transform.parent == action.mainProjectile.transform.parent)){              // 상대 유닛
                Unit enemy = h.gameObject.GetComponent<Unit>();
                if((applyToAllies && (mainComp.isEnemy == enemy.isEnemy)) || (!applyToAllies && (mainComp.isEnemy != enemy.isEnemy))){
                    targetObjects.Add(h.gameObject);
                    break;
                }
            }
            else if(applyToTower && h.CompareTag("Tower")){                                                         // 상대 타워
                if((mainComp.isEnemy && h.name == "PlayerTowerCollider") || (!mainComp.isEnemy && h.name == "EnemyTowerCollider")){
                    targetObjects.Add(h.gameObject);
                    break;
                }
            }
        }

        return targetObjects;
    }

    #endregion

}
