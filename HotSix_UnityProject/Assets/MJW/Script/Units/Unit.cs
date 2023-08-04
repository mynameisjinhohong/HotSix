using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    #region Properties

    public enum UnitState{
        Idle,
        Move,
        Action,
        Stun,
        KnockBack,
        Die
    };

    public GameManager gameManager;
    public UnitData unitData;
    public Animator anim;

    public Action moveBehavior;
    public List<Action> actionBehaviors;

    public int id;
    public int level;
    public UnitState state;
    public UnitStats mainStat;
    public UnitStats curStat;
    public bool isEnemy = false;
    
    public List<float> actionCurCooldowns;
    public Queue<int> actionQueue;

    public float knockbackCooldown = 0.0f;
    public float stunCooldown = 0.0f;
    public bool isActive = true;

    #endregion


    #region Methods

    public void Init(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(transform.GetComponent<Animator>() == null){
            anim = transform.GetChild(0).transform.GetComponent<Animator>();
        }
        else{
            anim = transform.GetComponent<Animator>();
        }

        state = UnitState.Idle;

        UpgradeStats upgradeStat = unitData.upgradeStats;

        mainStat = unitData.unitStats;
        mainStat.maxHP += upgradeStat.uMaxHP * level;
        mainStat.attackDamage += upgradeStat.uAttackDamage * level;
        mainStat.defensive += upgradeStat.uDefensive * level;

        curStat = mainStat;

        moveBehavior = curStat.moveBehavior;
        moveBehavior.mainUnit = transform.gameObject;
        moveBehavior.range = curStat.attackRange;
        moveBehavior.value = curStat.moveSpeed;

        for(int i = 0; i < curStat.actionBehaviors.Count; ++i){
            actionBehaviors.Add(curStat.actionBehaviors[i]);

            actionBehaviors[i].mainUnit = transform.gameObject;
            actionBehaviors[i].range = curStat.attackRange;
            actionBehaviors[i].cooldown = 1.0f / curStat.attackSpeed;
            actionBehaviors[i].value = curStat.attackDamage;

            actionCurCooldowns.Add(0.0f);
        }

        actionQueue = new Queue<int>();
    }

    public void GetDamage(float attackDamage){
        curStat.maxHP -= attackDamage * 1.0f / (1.0f + curStat.defensive * 0.01f);
    }

    public void SetAnimation(string name){
        if(name == "Idle")  anim.SetBool("Idle", true);
        else                anim.SetBool("Idle", false);
        if(name == "Move")  anim.SetBool("Move", true);
        else                anim.SetBool("Move", false);
    }

    public void Die(){
        Destroy(gameObject);
    }

    public void Idle(){
        SetAnimation("Idle");
    }

    public bool CheckMove(){
        return moveBehavior.Condition();
    }

    public void Move(){
        anim.SetFloat("MoveSpeed", 1.0f + (curStat.moveSpeed - 3.0f) * 0.33f);
        SetAnimation("Move");
        StartCoroutine(moveBehavior.action.ExecuteAction(moveBehavior));
    }

    public void Stun(){
        // Get Stun

        stunCooldown -= Time.deltaTime;
    }

    public void KnockBack(){
        // Get Knock Back

        knockbackCooldown -= Time.deltaTime;
    }

    public void CheckAction(){
        for(int i = 0; i < actionBehaviors.Count; ++i){
            actionCurCooldowns[i] += Time.deltaTime;
            if(actionCurCooldowns[i] >= actionBehaviors[i].cooldown){
                if(actionBehaviors[i].Condition()){
                    actionQueue.Enqueue(i);
                    actionCurCooldowns[i] = 0.0f;
                }
            }
        }
    }

    public void Action(){
        if(actionQueue.Count == 0) return;
        int curActionIndex = actionQueue.Dequeue();
        actionCurCooldowns[curActionIndex] = 0.0f;
        SetAnimation("");
        anim.SetTrigger("Action" + curActionIndex.ToString());
        anim.SetFloat("Action" + curActionIndex.ToString() + "Speed" , 1.0f / actionBehaviors[curActionIndex].cooldown);
        StartCoroutine(actionBehaviors[curActionIndex].action.ExecuteAction(actionBehaviors[curActionIndex]));
    }

    public bool IsActionPlaying(){
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Action");
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        Init();
    }

    void FixedUpdate(){
        // 분기별 상태 전환
        if(isActive){
            CheckAction();
            if(IsActionPlaying()){
                state = UnitState.Idle;
            }
            else{
                if(actionQueue.Count > 0){
                    state = UnitState.Action;
                }
                else if(CheckMove()){
                    state = UnitState.Move;
                }
                else{
                    state = UnitState.Idle;
                }
            }

            if(curStat.maxHP <= 0.0f){
                state = UnitState.Die;
            }
            else if(knockbackCooldown > 0.0001f){
                state = UnitState.KnockBack;
            }
            else if(stunCooldown > 0.0001f){
                state = UnitState.Stun;
            }
        }

        // 상태별 행동
        if(isActive){
            if(state == UnitState.Die){
                Die();
            }
            else if(state == UnitState.KnockBack){
                KnockBack();
            }
            else if(state == UnitState.Stun){
                Stun();
            }
            else if(state == UnitState.Action){
                Action();
            }
            else if(state == UnitState.Move){
                Move();
            }
            else if(state == UnitState.Idle){
                Idle();
            }
        }
    }

    void Update(){
        
    }

    #endregion
}
