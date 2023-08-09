using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SODefaultMove", menuName = "ActionBehavior/DefaultMove")]
public class SODefaultMove : SOActionBase
{
    public override bool Condition(Action action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count == 0;
    }

    public override IEnumerator ExecuteAction(Action action){
        action.mainUnit.transform.Translate(new Vector3(-action.value, 0, 0) * Time.deltaTime);
        yield break;
    }
}
