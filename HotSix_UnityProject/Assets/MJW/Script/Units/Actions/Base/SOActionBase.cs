using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Action
{
    public SOActionBase action;

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

    public bool Condition(){
        return action.Condition(this);
    }
}

public abstract class SOActionBase : ScriptableObject
{
    #region Properties
    
    public GameObject projectile;

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

        Vector3 center = action.mainUnit.transform.position;
        Physics.BoxCastNonAlloc(center, action.mainUnit.transform.lossyScale / 2.0f, -action.mainUnit.transform.right, action.hits, Quaternion.identity, action.range);
        action.hits = action.hits.OrderBy(h => h.distance).ToArray();

        for(int i = 0; i < action.hits.Length; ++i){
            RaycastHit hit = action.hits[i];
            if(hit.collider.CompareTag("Unit") && (hit.collider.transform.parent == action.mainUnit.transform.parent)){ // 상대 유닛
                Unit enemy = hit.collider.gameObject.GetComponent<Unit>();
                if((applyToAllies && (mainComp.isEnemy == enemy.isEnemy)) || (!applyToAllies && (mainComp.isEnemy != enemy.isEnemy))){
                    tempTarget = hit.collider.gameObject;
                    break;
                }
            }
            else if(applyToTower && hit.collider.CompareTag("Tower")){                                                  // 상대 타워
                if((mainComp.isEnemy && hit.collider.name == "PlayerTower") || (!mainComp.isEnemy && hit.collider.name == "EnemyTower")){
                    tempTarget = hit.collider.gameObject;
                    break;
                }
            }
        }

        if(projectile == null && splashRange > 0.001f){
            if(tempTarget != null){
                center.x = tempTarget.transform.position.x;
            }
            else{
                center.x += action.range;
            }

            Physics.OverlapBoxNonAlloc(center, new Vector3(splashRange / 2.0f, splashRange / 2.0f, 1.0f), action.hitSplashs, Quaternion.identity);

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
                    if((mainComp.isEnemy && h.name == "PlayerTower") || (!mainComp.isEnemy && h.name == "EnemyTower")){
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