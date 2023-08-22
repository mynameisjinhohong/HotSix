using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOMagicAttack", menuName = "ActionBehavior/MagicAttack")]
public class SOMagicAttack : SOActionBase
{
    public GameObject particle;

    public override bool Condition(Action action){
        action.targetObjects = FindTarget(action);
        if(action.targetObjects.Count > 0){
            action.targetPosition = action.targetObjects[0].transform.position;
            return true;
        }
        return false;
    }

    public override IEnumerator ExecuteAction(Action action){
        GameObject particleObject = Instantiate(particle, action.mainUnit.transform);
        yield return new WaitForSeconds(action.cooldown * 0.5f);
        if(particleObject != null){
            particleObject.transform.localPosition = new Vector3(-0.115f, 0.315f, 0.1f);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            particleSystem.Play();
            if (action.audio.clip != null)
            {
                action.audio.Play();
            }
        }
        Shoot(action);
        yield break;
    }

    public void Shoot(Action action){
        if(action.targetObjects.Count > 0){
            if(action.targetObjects[0] != null){
                action.targetPosition = action.targetObjects[0].transform.position;
            }
        }

        GameObject pInstance = Instantiate(actionObject);
        Projectile pScript = pInstance.GetComponent<Projectile>();

        pInstance.transform.SetParent(action.mainUnit.transform.parent);

        Vector3 startPos = action.mainUnit.transform.position + new Vector3(action.mainUnit.GetComponent<Unit>().isEnemy ? -0.5f : 0.5f, 0.5f, 0);
        Vector3 endPos = action.targetPosition;

        pScript.SetPos(startPos, endPos, endPos);
        pScript.action.lane = pInstance.transform.parent.gameObject;
        pScript.action.duration = (System.Math.Abs(endPos.x - startPos.x) + 0.1f) / 8.0f;
        pScript.action.value = action.value;
        pScript.action.isEnemy = action.mainUnit.GetComponent<Unit>().isEnemy;
        pScript.isTurning = true;
        pScript.isEnemy = pScript.action.isEnemy;
        pScript.action.isEnemy = pScript.isEnemy;
        
        pScript.isActive = true;
    }
}
