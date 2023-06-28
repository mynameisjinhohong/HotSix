using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeButton_HJH : MonoBehaviour
{
    public GameObject playerTower;
    public Vector3 positionMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(playerTower.transform.position + positionMove);

    }
}
