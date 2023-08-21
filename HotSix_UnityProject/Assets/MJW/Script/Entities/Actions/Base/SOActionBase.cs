using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Action : ICloneable
{
    public SOActionBase action;
    public TowerHPManager_HJH towerManager;
    public AudioSource audio;
    [HideInInspector]
    public GameObject mainUnit;
    [HideInInspector]
    public List<GameObject> targetObjects;
    [HideInInspector]
    public Vector3 targetPosition;
    [HideInInspector]
    public RaycastHit[] hits;
    [HideInInspector]
    public Collider[] hitSplashs;

    public float range;
    public float cooldown;
    public float value;
    public float upgradeValue;

    public bool movable = false;

    public object Clone(){
        Action action = new(){
            action = this.action,
            mainUnit = mainUnit,
            targetObjects = targetObjects,
            targetPosition = targetPosition,
            hits = hits,
            hitSplashs = hitSplashs,
            range = range,
            cooldown = cooldown,
            value = value,
            upgradeValue = upgradeValue,
            movable = movable
        };
        return action;
    }

    public bool Condition(){
        return action.Condition(this);
    }

}

public abstract class SOActionBase : ScriptableObject
{
    #region Properties
    
    public GameObject actionObject;
    public float splashRange = 0.0f;
    public bool applyToAllies = false;
    public bool applyToTower = true;

    #endregion


    #region Methods

    public abstract bool Condition(Action action);

    public abstract IEnumerator ExecuteAction(Action action);

    public List<GameObject> FindTarget(Action action){
        List<GameObject> targetObjects = new();

        GameObject tempTarget = null;
        Unit mainComp = action.mainUnit.GetComponent<Unit>();
        Collider mainCollider = action.mainUnit.GetComponent<Collider>();

        Vector3 center = mainCollider.bounds.center;
        // action.hits = Physics.BoxCastAll(center, action.mainUnit.transform.lossyScale / 2.0f, -action.mainUnit.transform.right, Quaternion.identity, 0.1f + action.range - action.mainUnit.transform.lossyScale.x / 2.0f)
        //                                 .OrderBy(h => h.distance).ToArray();

        action.hits = Physics.BoxCastAll(center, mainCollider.bounds.size / 2.0f, -action.mainUnit.transform.right, Quaternion.identity, (action.range - 1) * mainCollider.bounds.size.x)
                                         .OrderBy(h => h.distance).ToArray();

        for(int i = 0; i < action.hits.Length; ++i){
            RaycastHit hit = action.hits[i];
            if(hit.collider == null) continue;
            
            if(hit.collider.CompareTag("Unit") && (hit.collider.transform.parent == action.mainUnit.transform.parent)){                             // 상대 유닛
                Unit enemy = hit.collider.gameObject.GetComponent<Unit>();
                if((applyToAllies && (mainComp.isEnemy == enemy.isEnemy)) || (!applyToAllies && (mainComp.isEnemy != enemy.isEnemy))){
                    tempTarget = hit.collider.gameObject;
                    break;
                }
            }
            else if(applyToTower && hit.collider.CompareTag("Tower") && (hit.collider.transform.parent == action.mainUnit.transform.parent)){       // 상대 타워
                if((mainComp.isEnemy && hit.collider.name == "PlayerTowerCollider") || (!mainComp.isEnemy && hit.collider.name == "EnemyTowerCollider")){
                    tempTarget = hit.collider.gameObject;
                    break;
                }
            }
        }

        if(actionObject == null && splashRange > 0.001f){
            if(tempTarget != null){
                center.x = tempTarget.transform.position.x;
            }
            else{
                center.x += action.range;
            }

            action.hitSplashs = Physics.OverlapBox(center, new Vector3(splashRange / 2.0f, splashRange / 2.0f, 1.0f), Quaternion.identity);

            foreach(Collider h in action.hitSplashs){
                if(System.Object.ReferenceEquals(action.mainUnit, h)) continue;
                else if(h.CompareTag("Unit") && (h.transform.parent == action.mainUnit.transform.parent)){              // 상대 유닛
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
        }
        else{
            if(tempTarget != null) targetObjects.Add(tempTarget);
        }

        return targetObjects;
    }

    #endregion
}