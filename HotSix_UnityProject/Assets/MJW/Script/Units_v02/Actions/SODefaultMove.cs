using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SODefaultMove", menuName = "ActionBehavior/DefaultMove")]
public class SODefaultMove : SOActionBase
{
    public override bool Condition(GameObject mainUnit, out List<GameObject> targetObjects, ActionValue values){
        targetObjects = FindTarget(values.range, mainUnit);
        return targetObjects.Count == 0;
    }

    public override void ExecuteAction(float deltaTime, GameObject mainUnit, List<GameObject> targetObjects, ActionValue values){
        Unit unit = mainUnit.GetComponent<Unit>();
        mainUnit.transform.Translate(new Vector3(unit.curStat.moveSpeed, 0, 0) * deltaTime);
    }
}
