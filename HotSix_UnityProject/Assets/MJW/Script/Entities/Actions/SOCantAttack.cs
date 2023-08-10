using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOCantAttack", menuName = "ActionBehavior/CantAttack")]
public class SOCantAttack : SOActionBase
{
    public override bool Condition(Action action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count > 0;
    }

    public override IEnumerator ExecuteAction(Action action){
        yield break;
    }
}
