using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBuffAura", menuName = "ActionBehavior/BuffAura")]
public class SOBuffAura : SOActionBase
{
    public string buffStat;

    public override bool Condition(Action action){
        action.targetObjects.Clear();

        GameObject aura = null;
        foreach(Transform child in action.mainUnit.transform){
            if(child.CompareTag("Aura")) aura = child.gameObject;
        }
        if(aura == null){
            aura = Instantiate(actionObject, action.mainUnit.transform.position, quaternion.identity, action.mainUnit.transform);
            aura.transform.Rotate(new Vector3(80.0f, 0, 0));
            float spriteSize = aura.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            float lossyScale = action.range * 2 / spriteSize;
            lossyScale /= aura.transform.parent.transform.localScale.x;

            aura.transform.localScale = 2 * lossyScale * Vector3.one;
        }
        Unit mainUnit = action.mainUnit.GetComponent<Unit>();
        Collider collider = aura.GetComponent<Collider>();
        Collider[] hitColliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.size / 2.0f, quaternion.identity);
        foreach(Collider h in hitColliders){
            if(h.CompareTag("Unit") && (h.transform.parent == action.mainUnit.transform.parent)){
                Unit targetUnit = h.gameObject.GetComponent<Unit>();
                if((applyToAllies && (mainUnit.isEnemy == targetUnit.isEnemy)) || (!applyToAllies && (mainUnit.isEnemy != targetUnit.isEnemy))){
                    action.targetObjects.Add(h.gameObject);
                }
            }
        }

        return action.targetObjects.Count > 0;
    }

    public override IEnumerator ExecuteAction(Action action){
        foreach(GameObject t in action.targetObjects){
            if(t == null) continue;
            Unit unit = t.GetComponent<Unit>();
            unit.AddBuff(buffStat, action.value, 0.2f);
        }

        yield break;
    }
}
