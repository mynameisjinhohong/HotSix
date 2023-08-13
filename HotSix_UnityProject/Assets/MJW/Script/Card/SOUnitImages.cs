using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitImageData", menuName = "UnitData/UnitImageData")]

public class SOUnitImages : ScriptableObject
{
    [System.Serializable]
    public struct UnitImage{
        public Sprite iconImage;
        public Sprite fullImage;
    }

    public List<UnitImage> playerUnitImages;
    public List<UnitImage> enemyUnitImages;
    public List<UnitImage> specialUnitImages;
}
