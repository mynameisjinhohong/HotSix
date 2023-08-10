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
        Die
    };

    public GameManager gameManager;
    public UnitData unitData;
    public Animator anim;

    public Action moveBehavior;
    public List<Action> actionBehaviors;
    public int attackAction;

    public int id;
    public int level;
    public UnitState state;
    public UnitStats mainStat;
    public UnitStats curStat;
    public bool isEnemy = false;
    
    public List<float> actionCurCooldowns;
    public Queue<int> actionQueue;

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

        id = unitData.entityInfos.id;
        level = gameManager.userInfo.userUnitInfo[id].level;

        mainStat = unitData.unitStats;
        mainStat.maxHP += unitData.unitStats.uMaxHP * level;
        mainStat.defensive += unitData.unitStats.uDefensive * level;

        attackAction = unitData.attackAction;

        for(int i = 0; i < unitData.actionBehaviors.Count; ++i){
            actionBehaviors.Add(unitData.actionBehaviors[i]);

            actionBehaviors[i].mainUnit = transform.gameObject;
            actionBehaviors[i].value = unitData.actionBehaviors[i].value + (unitData.actionBehaviors[i].upgradeValue * (level - 1));
            actionBehaviors[i].range = unitData.actionBehaviors[i].range;
            actionBehaviors[i].cooldown = unitData.actionBehaviors[i].cooldown;

            actionCurCooldowns.Add(0.0f);
        }

        curStat = mainStat;

        moveBehavior.mainUnit = transform.gameObject;
        moveBehavior.range = actionBehaviors[attackAction].range;
        moveBehavior.value = curStat.moveSpeed;

        

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
            else if(stunCooldown > 0.0001f){
                state = UnitState.Stun;
            }
        }

        // 상태별 행동
        if(isActive){
            if(state == UnitState.Die){
                Die();
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

    void OnDestroy(){
        IEnumerator coroutine = moveBehavior.action.ExecuteAction(moveBehavior);
        if(coroutine != null) StopCoroutine(coroutine);

        for(int i = 0; i < actionBehaviors.Count; ++i){
            coroutine = actionBehaviors[i].action.ExecuteAction(actionBehaviors[i]);
            if(coroutine != null) StopCoroutine(coroutine);
        }
    }

    #endregion
}
