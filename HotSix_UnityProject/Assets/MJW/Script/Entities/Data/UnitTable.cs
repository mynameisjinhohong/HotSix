using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitInfos{
    public int id;
    public int uClass;
    public string e_name;
    public string k_name;
    public string e_information;
    public string k_information;
    public int cost;
    public float cooldown;
}

[System.Serializable]
public struct UnitStats{
    public float maxHP;
    public float uMaxHP;
    public float defensive;
    public float uDefensive;
    public float moveSpeed;
}

[System.Serializable]
public struct ActionValue{
    public int index;
    public string k_name;
    public string e_name;
}

[System.Serializable]
public struct UnitData{
    public UnitInfos entityInfos;
    public UnitStats unitStats;
    public int attackAction;
    public AudioClip audio;
    public ActionValue secondAction;
    public Action_MJW moveBehavior;
    public List<Action_MJW> actionBehaviors;
}

[CreateAssetMenu(fileName = "UnitTable", menuName = "UnitData/UnitTable")]
public class UnitTable : ScriptableObject
{
    public List<UnitData> unitData;
}
