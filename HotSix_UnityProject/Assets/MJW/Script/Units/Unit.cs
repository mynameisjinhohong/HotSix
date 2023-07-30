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
    
    public List<float> actionCooldowns;

    public float knockbackCooldown = 0.0f;
    public float stunCooldown = 0.0f;
    public int curActionIndex = -1;
    public bool actionBegin = false;
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
        moveBehavior.actionValue.range = curStat.attackRange;
        moveBehavior.actionValue.value = curStat.moveSpeed;
        moveBehavior.actionValue.upgradeValue = upgradeStat.uMoveSpeed;

        for(int i = 0; i < curStat.actionBehaviors.Count; ++i){
            actionBehaviors.Add(curStat.actionBehaviors[i]);

            actionBehaviors[i].mainUnit = transform.gameObject;
            actionBehaviors[i].actionValue.level = level;
            actionBehaviors[i].actionValue.range = curStat.attackRange;
            actionBehaviors[i].actionValue.cooldown = 1.0f / curStat.attackSpeed;
            actionBehaviors[i].actionValue.value = curStat.attackDamage;
            actionBehaviors[i].actionValue.upgradeValue = upgradeStat.uAttackDamage;

            actionCooldowns.Add(0.0f);
        }
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
        SetAnimation("Move");
        moveBehavior.ExecuteAction(Time.deltaTime);
    }

    public void Stun(){
        // Get Stun

        stunCooldown -= Time.deltaTime;
    }

    public void KnockBack(){
        // Get Knock Back

        knockbackCooldown -= Time.deltaTime;
    }

    public bool CheckAction(){
        for(int i = 0; i < actionBehaviors.Count; ++i){
            if(curActionIndex < 0 && actionCooldowns[i] >= actionBehaviors[i].actionValue.cooldown){
                if(actionBehaviors[i].Condition()){
                    curActionIndex = i;
                }
            }
            actionCooldowns[i] += Time.deltaTime;
        }

        return curActionIndex >= 0;
    }

    public void Action(){
        SetAnimation("");
        anim.SetTrigger("Action" + curActionIndex.ToString());
        actionBehaviors[curActionIndex].ExecuteAction(Time.deltaTime);
        actionBegin = true;
    }

    public bool IsCurrentActionOver(){
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Action") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    public void EndAction(){
        actionCooldowns[curActionIndex] = 0.0f;
        curActionIndex = -1;
        actionBegin = false;
    }

    #endregion


    #region Monobehavior Callbacks

    void Awake()
    {
        Init();
    }

    void FixedUpdate(){
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
                if(!actionBegin) Action();
                else if(IsCurrentActionOver()){
                    EndAction();
                }
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
        // 분기별 상태 전환
        if(isActive){
            CheckAction();
            if(curActionIndex >= 0){
                state = UnitState.Action;
            }
            else{
                if(CheckMove()){
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
    }

    #endregion
}
