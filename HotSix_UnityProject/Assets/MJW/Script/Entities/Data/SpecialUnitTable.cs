using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpecialUnitData{
    public UnitInfos entityInfos;
    public AudioClip audio;
    public ActionValue action;
    public Action_MJW actionBehavior;
}


[CreateAssetMenu(fileName = "SpecialUnitTable", menuName = "UnitData/SpecialUnitTable")]
public class SpecialUnitTable : ScriptableObject
{
    public List<SpecialUnitData> specialUnitData;
}
