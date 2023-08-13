using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInfo_MJW
{
    [System.Serializable]
    public class UserUnitInfo{
        public int id;
        public int level;
        public int number;

        public UserUnitInfo(int id, int level, int number){
            this.id = id;
            this.level = level;
            this.number = number;
        }
    }

    public List<UserUnitInfo> userUnitInfo;
    public List<UserUnitInfo> userSpecialUnitInfo;
    public Deck_MJW[] userDecks;
    public int selectedDeck;
    public int money;

    public UserInfo_MJW(){
        userUnitInfo = new List<UserUnitInfo>();
        userSpecialUnitInfo = new List<UserUnitInfo>();
        userDecks = new Deck_MJW[3];
        for(int i = 0; i < 9; ++i){
            userUnitInfo.Add(new UserUnitInfo(i, 1, 0));
        }
        for(int i = 0; i < 3; ++i){
            userSpecialUnitInfo.Add(new UserUnitInfo(i, 1, 0));
        }
        for(int i = 0; i < 3; ++i){
            userDecks[i] = new Deck_MJW();
        }
        selectedDeck = 0;
        money = 0;
    }

    public Deck_MJW GetSelectedDeck(){
        return userDecks[selectedDeck];
    }
}
