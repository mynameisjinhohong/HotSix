using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOArrowAttack", menuName = "ActionBehavior/ArrowAttack")]
public class SOArrowAttack : SOActionBase
{
    public override bool Condition(Action action){
        action.targetObjects = FindTarget(action);
        return action.targetObjects.Count > 0;
    }

    public override void ExecuteAction(float deltaTime, Action action){
        GameObject pInstance = Instantiate(projectile);
        Projectile pScript = pInstance.GetComponent<Projectile>();

        pInstance.transform.SetParent(action.mainUnit.transform.parent);
        pInstance.tag = "Projectile";

        Vector3 startPos = action.mainUnit.transform.position;
        Vector3 endPos = action.targetObjects[0].transform.position;
        Vector3 midPos = (startPos + endPos) / 2.0f;
        midPos.y += System.Math.Abs(endPos.x - startPos.x) * 0.5f;

        //Debug.Log(startPos + " " + midPos + " " + endPos);

        pScript.SetPos(startPos, midPos, endPos);
        pScript.action.lane = pInstance.transform.parent.gameObject;
        pScript.action.duration = (System.Math.Abs(endPos.x - startPos.x) + 0.1f) / 5.0f;
        pScript.action.value = action.value;
        pScript.isEnemy = action.mainUnit.GetComponent<Unit>().isEnemy;
        pScript.action.isEnemy = pScript.isEnemy;
        
        pScript.isActive = true;
    }
}
