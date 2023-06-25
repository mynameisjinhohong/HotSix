using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="UnitData",menuName="Game Data/Unit Data")]
public class UnitData_MJW : ScriptableObject
{
    public int maxHP = 100;
    public int attackDamage = 10;
    public float attackSpeed = 1.0f;
    public float attackRange = 1.0f;
    public int defensive = 5;
    public float moveSpeed = 5.0f;
    public int cost = 100;
}
