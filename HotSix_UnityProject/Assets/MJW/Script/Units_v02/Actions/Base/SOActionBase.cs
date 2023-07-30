using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ActionValue
{
    public float level;
    public float range;
    public float cooldown;
    public float value;
    public float upgradeValue;
}

[System.Serializable]
public class Action
{
    [HideInInspector]
    public GameObject mainUnit;
    [HideInInspector]
    public List<GameObject> targetObjects;
    [HideInInspector]
    public ActionValue actionValue;
    public SOActionBase action;

    public bool Condition(){
        return action.Condition(mainUnit, out targetObjects, actionValue);
    }

    public void ExecuteAction(float deltaTime){
        action.ExecuteAction(deltaTime, mainUnit, targetObjects, actionValue);
    }
}

public abstract class SOActionBase : ScriptableObject
{
    #region Properties
    
    public GameObject projectile;

    protected RaycastHit[] hits;

    public float splashRange = 0.0f;
    public bool applyToAllies = false;
    public bool applyToTower = false;

    #endregion


    #region Methods

    public abstract bool Condition(GameObject mainUnit, out List<GameObject> targetObjects, ActionValue values);

    public abstract void ExecuteAction(float deltaTime, GameObject mainUnit, List<GameObject> targetObjects, ActionValue values);

    public List<GameObject> FindTarget(float range, GameObject mainUnit){
        List<GameObject> targetObjects = new List<GameObject>();

        GameObject tempTarget = null;
        Unit mainComp = mainUnit.GetComponent<Unit>();

        Vector3 center = mainUnit.transform.position;
        hits = Physics.BoxCastAll(center, mainUnit.transform.lossyScale / 2.0f, -mainUnit.transform.right, Quaternion.identity, range)
                                .OrderBy(h => h.distance).ToArray();
        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider.CompareTag("Unit") && (hit.collider.transform.parent == mainUnit.transform.parent)){ // 상대 유닛
                Unit enemy = hit.collider.gameObject.GetComponent<Unit>();
                if((applyToAllies && (mainComp.isEnemy == enemy.isEnemy)) || (!applyToAllies && (mainComp.isEnemy != enemy.isEnemy))){
                    tempTarget = hit.collider.gameObject;
                    break;
                }
            }
            else if(applyToTower && hit.collider.CompareTag("Tower")){                                           // 상대 타워
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
                center.x += range;
            }

            Collider[] hitSplashs = Physics.OverlapBox(center, new Vector3(splashRange / 2.0f, splashRange / 2.0f, 1.0f), Quaternion.identity);
            foreach(Collider h in hitSplashs){
                if(System.Object.ReferenceEquals(mainUnit, h)) continue;
                else if(h.CompareTag("Unit") && (h.transform.parent == mainUnit.transform.parent)){              // 상대 유닛
                    Unit enemy = h.gameObject.GetComponent<Unit>();
                    if((applyToAllies && (mainComp.isEnemy == enemy.isEnemy)) || (!applyToAllies && (mainComp.isEnemy != enemy.isEnemy))){
                        targetObjects.Add(h.gameObject);
                        break;
                    }
                }
                else if(applyToTower && h.CompareTag("Tower")){                                                  // 상대 타워
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