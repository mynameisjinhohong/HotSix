using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : Entity
{
    #region Properties

    [System.Serializable]
    public struct Buff{
        public string stat;
        public float value;

        public override bool Equals(object obj){
            Buff target = (Buff)obj;
            return stat.Equals(target.stat) && value == target.value;
        }
        public override int GetHashCode(){
            return (stat, value).GetHashCode();
        }
    }

    public Dictionary<Buff, float> buffs;

    // public GameManager gameManager;
    public UnitData unitData;
    // public Animator anim;

    public Action moveBehavior;
    public List<Action> actionBehaviors;
    public int attackAction;

    // public int id;
    // public int level;
    // public UnitState state;
    public UnitStats mainStat;
    public UnitStats curStat;
    // public bool isEnemy = false;
    
    public List<float> actionCurCooldowns;
    public Queue<int> actionQueue;

    public GameObject stunIcon;
    public float stunCooldown = 0.0f;
    // public bool isActive = true;
    public int curAction;
    public bool isInvincible = false;

    #endregion


    #region Methods

    public void Init(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stunIcon = transform.Find("Dizzy_effect").gameObject;
        stunIcon.SetActive(false);
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
        mainStat.maxHP += unitData.unitStats.uMaxHP * (level - 1);
        mainStat.defensive += unitData.unitStats.uDefensive * (level - 1);

        attackAction = unitData.attackAction;

        for(int i = 0; i < unitData.actionBehaviors.Count; ++i){
            actionBehaviors.Add((Action)unitData.actionBehaviors[i].Clone());

            actionBehaviors[i].mainUnit = transform.gameObject;
            actionBehaviors[i].value = unitData.actionBehaviors[i].value + (unitData.actionBehaviors[i].upgradeValue * (level - 1));
            actionBehaviors[i].range = unitData.actionBehaviors[i].range;
            actionBehaviors[i].cooldown = unitData.actionBehaviors[i].cooldown;

            actionCurCooldowns.Add(0.0f);
        }

        curStat = mainStat;

        moveBehavior = (Action)unitData.moveBehavior.Clone();
        moveBehavior.mainUnit = transform.gameObject;
        moveBehavior.range = actionBehaviors[attackAction].range;
        moveBehavior.value = curStat.moveSpeed;

        actionQueue = new Queue<int>();
        buffs = new Dictionary<Buff, float>();
    }

    public void GetDamage(float attackDamage){
        if(!isInvincible) curStat.maxHP -= attackDamage * 1.0f / (1.0f + curStat.defensive * 0.01f);
    }

    public void SetAnimation(string name){
        if(name == "Idle")  anim.SetBool("Idle", true);
        else                anim.SetBool("Idle", false);
        if(name == "Move")  anim.SetBool("Move", true);
        else                anim.SetBool("Move", false);
        if(name == "Stun")  anim.SetBool("Stun", true);
        else                anim.SetBool("Stun", false);
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
        SetAnimation("Stun");
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
        curAction = actionQueue.Dequeue();
        actionCurCooldowns[curAction] = 0.0f;
        SetAnimation("");
        anim.SetTrigger("Action" + curAction.ToString());
        anim.SetFloat("Action" + curAction.ToString() + "Speed" , 1.0f / actionBehaviors[curAction].cooldown);
        StartCoroutine(actionBehaviors[curAction].action.ExecuteAction(actionBehaviors[curAction]));
    }

    public bool IsActionPlaying(){
        return anim.GetCurrentAnimatorStateInfo(0).IsTag("Action");
    }

    public void AddBuff(string name, float value, float time){
        Buff buff = new(){
            stat = name,
            value = value
        };
        if(buffs.ContainsKey(buff)){
            buffs[buff] = time;
        }
        else{
            buffs.Add(buff, time);
        }
    }

    public void TakeBuff(){
        List<Buff> buffStat = buffs.Keys.ToList();
        for(int i = 0; i < buffs.Count; ++i){
            buffs[buffStat[i]] -= Time.deltaTime;
            if(buffs[buffStat[i]] < 0.0f){
                ChangeStat(buffStat[i].stat, 0.0f);
                buffs.Remove(buffStat[i]);
            }
        }
        buffStat = buffs.Keys.ToList();
        for(int i = 0; i < buffs.Count; ++i){
            ChangeStat(buffStat[i].stat, buffStat[i].value);
            for(int j = 0; j < i; ++j){
                if(buffStat[j].stat == buffStat[i].stat && buffStat[j].value > buffStat[i].value){
                    ChangeStat(buffStat[j].stat, buffStat[j].value);
                }
            }
        }
    }

    public void ChangeStat(string name, float value){
        if(name == "AttackSpeed"){
            float attackSpeed = 1.0f / unitData.actionBehaviors[attackAction].cooldown;
            actionBehaviors[attackAction].cooldown = 1.0f / (attackSpeed + value);
        }
        else if(name == "Defensive"){
            curStat.defensive = mainStat.defensive + value;
        }
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
            TakeBuff();
            stunIcon.SetActive(stunCooldown > 0.0f);

            if(IsActionPlaying()){
                if(actionBehaviors[curAction].movable && CheckMove()) state = UnitState.Move;
                else state = UnitState.Idle;
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
            else if(stunCooldown > 0.0f){
                state = UnitState.Stun;
            }
        }

        // 상태별 행동
        if(isActive){
            if(state == UnitState.Die){
                StartCoroutine(Die(1.0f));
            }
            if(state == UnitState.Stun){
                Stun();
            }
            if(state == UnitState.Action){
                Action();
            }
            if(state == UnitState.Move){
                Move();
            }
            if(state == UnitState.Idle){
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
     
    /*
    public void OnDrawGizmos(){
        Collider mainCollider = GetComponent<Collider>();
        Vector3 center = mainCollider.bounds.center;
        Gizmos.color = Color.red;
        Gizmos.DrawRay (center, -transform.right * ((actionBehaviors[attackAction].range - 1) * mainCollider.bounds.size.x));
        Gizmos.DrawWireCube (center - transform.right * ((actionBehaviors[attackAction].range - 1) * mainCollider.bounds.size.x), mainCollider.bounds.size);
    }
    */

    #endregion
}
