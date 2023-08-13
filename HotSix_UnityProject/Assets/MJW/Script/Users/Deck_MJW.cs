using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum UnitTag{
    Unit,
    Special
}

[System.Serializable]
public struct UnitID{
    public UnitTag unitTag;
    public int id;
}

[System.Serializable]
public class Deck_MJW
{
    public List<UnitID> unitIDs;

    public Deck_MJW(){
        unitIDs = new List<UnitID>();
        for(int i = 1; i <= 5; ++i){
            UnitID ids = new()
            {
                unitTag = UnitTag.Unit,
                id = i
            };
            unitIDs.Add(ids);
        }
    }
}
