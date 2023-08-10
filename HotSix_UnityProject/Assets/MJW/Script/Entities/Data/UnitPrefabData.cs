using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitPrefabData", menuName = "UnitData/UnitPrefabData")]
public class UnitPrefabData : ScriptableObject
{
    public List<GameObject> playerUnitPrefabs;
    public List<GameObject> enemyUnitPrefabs;
    public List<GameObject> specialUnitPrefabs;
}
