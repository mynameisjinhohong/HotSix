using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInfo{
    public int id;
    public string e_name;
    public string k_name;
    public string uclass;
    public string e_information;
    public string k_information;

    public UnitInfo(int id, string e_name, string k_name, string uclass, string e_information, string k_information){
        this.id = id;
        this.e_name = e_name;
        this.k_name = k_name;
        this.uclass = uclass;
        this.e_information = e_information;
        this.k_information = k_information;
    }
}

[System.Serializable]
public class UnitStat{
    public float maxHP;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float defensive;
    public float moveSpeed;
    public int cost;
    public float cooldown;

    public UnitStat(float maxHP, float attackDamage, float attackSpeed, float attackRange, float defensive, float moveSpeed, int cost, float cooldown){
        this.maxHP = maxHP;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.defensive = defensive;
        this.moveSpeed = moveSpeed;
        this.cost = cost;
        this.cooldown = cooldown;
    }

    public UnitStat DeepCopy(){
        return new UnitStat(this.maxHP, this.attackDamage, this.attackSpeed, this.attackRange, this.defensive, this.moveSpeed, this.cost, this.cooldown);
    }
}

[System.Serializable]
public class UpgradeStat{
    public float maxHP;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float defensive;
    public float moveSpeed;
    public int uCost;

    public UpgradeStat(float maxHP, float attackDamage, float attackSpeed, float attackRange, float defensive, float moveSpeed, int uCost){
        this.maxHP = maxHP;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.defensive = defensive;
        this.moveSpeed = moveSpeed;
        this.uCost = uCost;
    }
}

[System.Serializable]
public class Unit_MJW
{
    public UnitInfo unitInfo;
    public UnitStat unitStat;
    public UpgradeStat upgradeStat;

    public Unit_MJW(int id, string e_name, string k_name, string uclass, string e_information, string k_information,
                    float maxHP, float attackDamage, float attackSpeed, float attackRange, float defensive, float moveSpeed, int cost, float cooldown,
                    float uMaxHP, float uAttackDamage, float uAttackSpeed, float uAttackRange, float uDefensive, float uMoveSpeed, int uCost){
        unitInfo = new UnitInfo(id, e_name, k_name, uclass, e_information, k_information);
        unitStat = new UnitStat(maxHP, attackDamage, attackSpeed, attackRange, defensive, moveSpeed, cost, cooldown);
        upgradeStat = new UpgradeStat(uMaxHP, uAttackDamage, uAttackSpeed, uAttackRange, uDefensive, uMoveSpeed, uCost);
    }
}
