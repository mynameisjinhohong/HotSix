using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterJump : MonoBehaviour
{
    [SerializeField] GameObject glow; 
    public void StartJumping(){
        transform.LeanMoveLocal(new Vector2(0,0),1).setEaseOutQuart().setLoopPingPong();
    }
}
