using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOTurtleTransform", menuName = "ActionBehavior/TurtleTransform")]
public class SOTurtleTransform : SOActionBase
{
    public override bool Condition(Action_MJW action){
        action.targetObjects = FindTarget(action);
        if(action.targetObjects.Count == 0){
            Unit unit = action.mainUnit.GetComponent<Unit>();
            if(unit.IsActionPlaying()){
                unit.SetAnimation("Idle");
                unit.anim.Play("turtle_idle");
            }
            return false;
        }
        return action.targetObjects.Count > 0;
    }

    public override IEnumerator ExecuteAction(Action_MJW action){
        action.targetObjects = FindTarget(action);
        if(action.targetObjects.Count == 0) yield break;
        Unit unit = action.mainUnit.GetComponent<Unit>();
        if(!unit.IsActionPlaying()){
            unit.anim.Play("turtle_defense");
            if (action.audio.clip != null)
            {
                action.audio.Play();
            }
        }
        yield break;
    }
}
