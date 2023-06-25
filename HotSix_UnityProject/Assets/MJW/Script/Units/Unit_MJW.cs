using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_MJW : MonoBehaviour
{
    [System.Serializable]
    public class UnitStat{
        public int maxHP;
        public int currentHP;
        public int attackDamage;
        public float attackSpeed;
        public float attackRange;
        public int defensive;
        public float moveSpeed;
        public int cost;
    }

    public enum UnitState{
        Move,
        Attack
    };
    
    public Collider unitCollider;
    public UnitStat unitStat;
    public UnitState unitState;
    public bool isEnemy;

    #region Methods

    public bool isEnemyInFront(){
        return false;
    }

    public void Move(){
        transform.Translate(new Vector3(unitStat.moveSpeed, 0, 0) * Time.deltaTime);
    }

    public void Attack(){

    }

    #endregion

    #region MonoBehavior Callbacks

    void Awake(){
        unitCollider = GetComponent<Collider>();
    }

    void Start(){

    }

    void FixedUpdate(){
        if(unitState == UnitState.Attack){
            Attack();
        }
        else{
            Move();
        }
    }

    void Update(){
        if(isEnemyInFront()){
            unitState = UnitState.Attack;
        }
        else{
            unitState = UnitState.Move;
        }
    }

    #endregion
}
