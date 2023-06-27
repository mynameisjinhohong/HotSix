using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_MJW : MonoBehaviour
{
    [System.Serializable]
    public class UnitStat{
        [Tooltip("유닛 전체 체력")]
        public float maxHP = 100;
        [Tooltip("유닛 공격력")]
        public float attackDamage = 10;
        [Tooltip("유닛 초당 공격 속도")]
        public float attackSpeed = 1;
        [Tooltip("유닛 공격 사거리")]
        public float attackRange = 1;
        [Tooltip("유닛 방어력")]
        public float defensive = 0;
        [Tooltip("유닛 이동 속도")]
        public float moveSpeed = 5;
        [Tooltip("유닛 가격")]
        public int cost = 100;
    }

    public enum UnitState{
        Move,
        Attack
    };

    public UnitStat unitStat;
    [HideInInspector]
    public Collider unitCollider;
    private RaycastHit hit;
    
    
    [HideInInspector]
    public UnitStat currentStat;
    [HideInInspector]
    public UnitState unitState;
    private Unit_MJW enemy;
    [HideInInspector]
    public bool isEnemy;
    private float attackCooldown = 0.0f;


    #region Methods

    public bool isEnemyInFront(){
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.right * (isEnemy ? -1 : 1), out hit, unitStat.attackRange)){
            if(hit.collider.tag == "Unit"){ // 상대 유닛
                enemy = hit.collider.gameObject.GetComponent<Unit_MJW>();
                if(isEnemy != enemy.isEnemy) return true;
            }
            else if(hit.collider.tag == "Tower"){ // 상대 타워

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
                enemy.currentStat.maxHP -= unitStat.attackDamage * 1.0f / (1.0f + enemy.currentStat.defensive * 0.01f);

                attackCooldown = unitStat.attackSpeed;
            }
            attackCooldown -= Time.deltaTime;
        }
    }

    #endregion

    #region MonoBehavior Callbacks

    void Awake(){
        // 초기값 세팅
        unitCollider = GetComponent<Collider>();
        currentStat.maxHP = unitStat.maxHP;
        currentStat.attackDamage = unitStat.attackDamage;
        currentStat.attackSpeed = 1.0f / unitStat.attackSpeed;
        currentStat.attackRange = unitStat.attackRange;
        currentStat.defensive = unitStat.defensive;
        currentStat.moveSpeed = unitStat.moveSpeed;
        currentStat.cost = unitStat.cost;
    }

    void Start(){

    }

    void FixedUpdate(){
        // 상태별 행동
        if(unitState == UnitState.Attack){
            Attack();
        }
        else{
            Move();
        }
    }

    void Update(){
        // 분기별 상태 전환
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
