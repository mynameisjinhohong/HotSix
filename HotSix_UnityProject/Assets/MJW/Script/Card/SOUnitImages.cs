using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitImageData", menuName = "UnitData/UnitImageData")]

public class SOUnitImages : ScriptableObject
{
    [System.Serializable]
    public struct UnitImage{
        public Sprite moneySpace_Icon;
        public Sprite nomal_Icon;
        public Sprite inGame_Icon;
        public Sprite proFile_Icon;
    }

    public List<UnitImage> playerUnitImages;
    public List<UnitImage> enemyUnitImages;
    public List<UnitImage> specialUnitImages;
}
