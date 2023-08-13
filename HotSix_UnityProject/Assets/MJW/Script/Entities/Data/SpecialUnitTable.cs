using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpecialUnitData{
    public UnitInfos entityInfos;
    public ActionValue action;
    public Action actionBehavior;
}


[CreateAssetMenu(fileName = "SpecialUnitTable", menuName = "UnitData/SpecialUnitTable")]
public class SpecialUnitTable : ScriptableObject
{
    public List<SpecialUnitData> specialUnitData;
}
