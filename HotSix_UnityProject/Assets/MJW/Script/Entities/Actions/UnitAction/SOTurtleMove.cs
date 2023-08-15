using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOTurtleMove", menuName = "ActionBehavior/TurtleMove")]
public class SOTurtleMove : SOActionBase
{
    public override bool Condition(Action action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count == 0;
    }

    public override IEnumerator ExecuteAction(Action action){
        Unit unit = action.mainUnit.GetComponent<Unit>();
        if(unit.IsActionPlaying()){
            unit.SetAnimation("Move");
            unit.anim.Play("turtle_walk");
        }
        action.mainUnit.transform.Translate(new Vector3(-action.value, 0, 0) * Time.deltaTime);
        yield break;
    }
}

