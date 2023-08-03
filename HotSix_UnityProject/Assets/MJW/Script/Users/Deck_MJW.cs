using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck_MJW
{
    public List<int> unitIDs;

    public Deck_MJW(){
        unitIDs = new List<int>(){ 1, 2, 3, 4, 5 };
    }
}
