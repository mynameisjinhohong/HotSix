using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileAction
{
    public SOProjectileActionBase action;

    [HideInInspector]
    public GameObject mainProjectile;
    [HideInInspector]
    public List<GameObject> targetObjects;
    public GameObject lane;

    public float duration;
    public float value;
    public bool isEnemy;

    public bool Condition(){
        return action.Condition(this);
    }

    public void ExecuteAction(){
        action.ExecuteAction(this);
    }
}

public abstract class SOProjectileActionBase : ScriptableObject
{
    #region Properties

    public float splashRange = 0.0f;
    public bool applyToAllies = false;
    public bool applyToTower = true;

    #endregion

    
    #region Methods

    public abstract bool Condition(ProjectileAction action);

    public abstract void ExecuteAction(ProjectileAction action);

    #endregion

}
