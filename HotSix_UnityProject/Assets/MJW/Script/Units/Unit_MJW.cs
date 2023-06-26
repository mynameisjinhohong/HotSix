using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_MJW : MonoBehaviour
{
    [System.Serializable]
    public class UnitStat{
        public int maxHP = 100;
        public int attackDamage = 10;
        public float attackSpeed = 1;
        public float attackRange = 1;
        public int defensive = 0;
        public float moveSpeed = 5;
        public int cost = 100;
    }

    public enum UnitState{
        Move,
        Attack
    };

    private RaycastHit hit;
    private Unit_MJW enemy;
    private float attackCooldown = 0.0f;

    public Collider unitCollider;
    public UnitStat unitStat;
    [HideInInspector]
    public UnitStat currentStat;
    public UnitState unitState;
    public bool isEnemy;

    #region Methods

    public bool isEnemyInFront(){
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.right * (isEnemy ? -1 : 1), out hit, unitStat.attackRange)){
            if(hit.collider.tag == "Unit"){
                enemy = hit.collider.gameObject.GetComponent<Unit_MJW>();
                if(isEnemy != enemy.isEnemy) return true;
            }
        }
        return false;
    }

    public void Move(){
        transform.Translate(new Vector3(unitStat.moveSpeed, 0, 0) * Time.deltaTime);
    }

    public void Attack(){
        if(enemy != null){
            if(attackCooldown <= 0.0f){
                enemy.currentStat.maxHP -= unitStat.attackDamage - enemy.currentStat.defensive;

                attackCooldown = unitStat.attackSpeed;
            }
            attackCooldown -= Time.deltaTime;
        }
    }

    #endregion

    #region MonoBehavior Callbacks

    void Awake(){
        unitCollider = GetComponent<Collider>();
        currentStat.maxHP = unitStat.maxHP;
        currentStat.attackDamage = unitStat.attackDamage;
        currentStat.attackSpeed = unitStat.attackSpeed;
        currentStat.attackRange = unitStat.attackRange;
        currentStat.defensive = unitStat.defensive;
        currentStat.moveSpeed = unitStat.moveSpeed;
        currentStat.cost = unitStat.cost;
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
        if(currentStat.maxHP <= 0){
            Destroy(gameObject);
        }
        else if(isEnemyInFront()){
            unitState = UnitState.Attack;
        }
        else{
            unitState = UnitState.Move;
        }
    }

    #endregion
}
