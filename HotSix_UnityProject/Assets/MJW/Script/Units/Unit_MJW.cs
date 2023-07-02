using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit_MJW : MonoBehaviour
{
    #region Properties

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
    private Animator animator;
    
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

    #endregion


    #region Methods

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < hits.Length; ++i)
    //    {
    //        RaycastHit hit = hits[i];
    //        if (hit.collider.tag == "Unit" && (hit.collider.transform.parent == transform.parent))
    //        {
    //            Debug.Log(hit.distance);
    //            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
    //            Gizmos.DrawWireCube(transform.position + (transform.forward * hit.distance), gameObject.transform.lossyScale);
    //        }
    //    }
    //}

    public int IsEnemyInFront(){
        Vector3 rayOrigin = gameObject.transform.position;
        hits = Physics.BoxCastAll(rayOrigin, gameObject.transform.lossyScale/2, transform.forward,Quaternion.identity,currentStat.attackRange)
                                .OrderBy(h => h.distance).ToArray();
        for(int i = 0; i < hits.Length; ++i){
            RaycastHit hit = hits[i];
            if(hit.collider.tag == "Unit" && (hit.collider.transform.parent == transform.parent)){  // 상대 유닛
                enemy = hit.collider.gameObject.GetComponent<Unit_MJW>();
                if(isEnemy != enemy.isEnemy) return 1;
            }
            else if(hit.collider.tag == "Tower"){                                                   // 상대 타워
                if((isEnemy && hit.collider.name == "PlayerTower") || (!isEnemy && hit.collider.name == "EnemyTower")){
                    return 2;
                }
            }
        }
        return 0;
    }

    public void SetAnimationSpeed(){
        animator.SetFloat("WalkSpeed", currentStat.moveSpeed * 0.2f);
        animator.SetFloat("AttackSpeed", currentStat.attackSpeed * 0.5f);
    }

    public void Move(){
        transform.Translate(new Vector3(currentStat.moveSpeed, 0, 0) * Time.deltaTime);
    }

    public void Attack(){
        if(attackCooldown <= 0.0f){
            if(checkEnemy == 1){    // 상대 유닛
                enemy.currentStat.maxHP -= GetDamage(currentStat.attackDamage, enemy.currentStat.defensive);
            }
            else{                   // 상대 타워
                if(isEnemy){
                    towerManager.playerTowerHP -= GetDamage(currentStat.attackDamage, 0.0f);
                }
                else{
                    towerManager.enemyTowerHP -= GetDamage(currentStat.attackDamage, 0.0f);
                }
            }
            attackCooldown = currentStat.attackSpeed;
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
        animator = GetComponent<Animator>();
        SetAnimationSpeed();
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
        checkEnemy = IsEnemyInFront();
        if(currentStat.maxHP <= 0){
            Destroy(gameObject);
        }
        else if(checkEnemy > 0){
            unitState = UnitState.Attack;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
        }
        else{
            unitState = UnitState.Move;
            animator.SetBool("Attack", false);
            animator.SetBool("Walk", true);
        }
    }

    #endregion
}
