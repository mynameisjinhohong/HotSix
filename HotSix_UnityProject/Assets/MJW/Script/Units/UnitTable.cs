using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitInfos{
    public int id;
    public string e_name;
    public string k_name;
    public string e_information;
    public string k_information;
}

[System.Serializable]
public struct UnitStats{
    public float maxHP;
    public float attackDamage;
    public float attackRange;
    public float attackSpeed;
    public float defensive;
    public float moveSpeed;
    public int cost;
    public float cooldown;
    public Action moveBehavior;
    public List<Action> actionBehaviors;
}

[System.Serializable]
public struct UpgradeStats{
    public float uMaxHP;
    public float uAttackDamage;
    public float uAttackRange;
    public float uAttackSpeed;
    public float uDefensive;
    public float uMoveSpeed;
    public int upgradeCost;
}

[System.Serializable]
public struct UnitData{
    public UnitInfos unitInfos;
    public UnitStats unitStats;
    public UpgradeStats upgradeStats;
}

[CreateAssetMenu(fileName = "UnitTable", menuName = "UnitData/UnitTable")]
public class UnitTable : ScriptableObject
{
    public List<UnitData> unitData;
}
