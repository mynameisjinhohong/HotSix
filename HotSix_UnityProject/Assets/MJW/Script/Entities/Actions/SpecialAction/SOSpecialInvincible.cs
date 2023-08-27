using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSpecialInvincible", menuName = "SpecialBehavior/SpecialInvincible")]
public class SOSpecialInvincible : SOActionBase
{
    public GameObject invincibleAura;

    public override bool Condition(Action_MJW action){
        return true;
    }

    public override IEnumerator ExecuteAction(Action_MJW action){
        GameObject map = GameObject.Find("BG+MapManager");
        Entity mainSpecial = action.mainUnit.GetComponent<Entity>();
        Collider mainCollider = action.mainUnit.GetComponent<Collider>();
        bool isEnemy = mainSpecial.isEnemy;

        // 중앙으로 이동
        while(true){
            Vector3 center = mainCollider.bounds.center;
            if(isEnemy && (center.x <= map.transform.position.x)
            || !isEnemy && (center.x >= map.transform.position.x)){
                break;
            }
            action.mainUnit.transform.Translate(new Vector3(-15.0f, 0, 0) * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        GameObject effectObject = Instantiate(actionObject, action.mainUnit.transform);

        Transform[] allChildren = action.mainUnit.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren){
            SpriteRenderer sprite = child.GetComponent<SpriteRenderer>();
            if(sprite != null){
                Color tmp = sprite.color;
                tmp.a = 0.0f;
                sprite.color = tmp;
            }
        }

        // 효과 발동
        if(action.audio.clip != null)
        {
            action.audio.Play();
        }

        Transform lane = GameObject.Find("Lane").transform;
        // Transform[] allChildren = lane.GetComponentsInChildren<Transform>();
        allChildren = lane.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren){
            if(child == null) continue;
            if(child.CompareTag("Unit")){
                Unit unit = child.GetComponent<Unit>();
                if(isEnemy == unit.isEnemy){
                    unit.isInvincible = true;
                    GameObject aura = Instantiate(invincibleAura, child.transform.position, quaternion.identity, child);
                    Collider childCollider = child.GetComponent<Collider>();
                    aura.transform.Translate(new Vector3(0.0f, -childCollider.bounds.size.y / 2.0f, 0.0f));
                    aura.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
                    aura.name = "Invincible Aura";
                }
            }
        }

        yield return new WaitForSeconds(action.value);

        allChildren = lane.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren){
            if(child == null) continue;
            if(child.CompareTag("Unit")){
                Unit unit = child.GetComponent<Unit>();
                if(isEnemy == unit.isEnemy){
                    unit.isInvincible = false;
                }
            }
            else if(child.name == "Invincible Aura"){
                Destroy(child.gameObject);
            }
        }

        Destroy(effectObject);
        mainSpecial.state = Entity.UnitState.Die;

        yield break;
    }
}
