using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    private void OnEnable()
    {
        transform.LeanMoveLocal(new Vector2(0, 35), 3f).setEaseInOutCubic().setLoopPingPong();
    }
}
