using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Properties

    [System.Serializable]
    public enum UnitState{
        Idle,
        Move,
        Action,
        Stun,
        Die
    };

    public GameManager gameManager;
    public Animator anim;

    public UnitState state;
    public int id;
    public int level;
    public bool isEnemy;

    public bool isActive = true;

    #endregion


    #region Methods

    public void Die(){
        Destroy(gameObject);
    }

    #endregion
}
