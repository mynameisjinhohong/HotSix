using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_MJW : MonoBehaviour
{
    [System.Serializable]
    public class UnitStat{
        [Tooltip("유닛 전체 체력")]
        public float maxHP = 1000;
        [Tooltip("유닛 공격력")]
        public float attackDamage = 100;
        [Tooltip("유닛 초당 공격 속도")]
        public float attackSpeed = 1;
        [Tooltip("유닛 공격 사거리")]
        public float attackRange = 1;
        [Tooltip("유닛 방어력")]
        public float defensive = 50;
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
    private RaycastHit[] hits;
    
    [HideInInspector]
    public UnitStat currentStat;
    [HideInInspector]
    public UnitState unitState;
    private Unit_MJW enemy;
    private TowerHPManager_HJH towerManager;
    [HideInInspector]
    public bool isEnemy = false;
    private float attackCooldown = 0.0f;
    private int checkEnemy = 0;


    #region Methods

    public int isEnemyInFront(){
        hits = Physics.RaycastAll(gameObject.transform.position, gameObject.transform.right * (isEnemy ? -1 : 1), currentStat.attackRange);
        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider.tag == "Unit"){         // 상대 유닛
                enemy = hit.collider.gameObject.GetComponent<Unit_MJW>();
                if(isEnemy != enemy.isEnemy) return 1;
            }
            else if(hit.collider.tag == "Tower"){   // 상대 타워
                if((isEnemy && hit.collider.name == "PlayerTower") || (!isEnemy && hit.collider.name == "EnemyTower")){
                    return 2;
                }
            }
        }
        return 0;
    }

    public void Move(){
        transform.Translate(new Vector3(unitStat.moveSpeed, 0, 0) * Time.deltaTime);
    }

    public void Attack(){
        if(attackCooldown <= 0.0f){
            if(checkEnemy == 1){    // 상대 유닛
                enemy.currentStat.maxHP -= GetDamage(unitStat.attackDamage, enemy.currentStat.defensive);
            }
            else{                   // 상대 타워
                if(isEnemy){
                    towerManager.playerTowerHP -= GetDamage(unitStat.attackDamage, 0.0f);
                }
                else{
                    towerManager.enemyTowerHP -= GetDamage(unitStat.attackDamage, 0.0f);
                }
            }
            attackCooldown = unitStat.attackSpeed;
        }
        attackCooldown -= Time.deltaTime;
    }

    public float GetDamage(float attackDamage, float defensive){
        return attackDamage * 1.0f / (1.0f + defensive * 0.01f);
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

        towerManager = GameObject.Find("TowerHPManager").GetComponent<TowerHPManager_HJH>();
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
        checkEnemy = isEnemyInFront();
        if(currentStat.maxHP <= 0){
            Destroy(gameObject);
        }
        else if(checkEnemy > 0){
            unitState = UnitState.Attack;
        }
        else{
            unitState = UnitState.Move;
        }
    }

    #endregion
}
