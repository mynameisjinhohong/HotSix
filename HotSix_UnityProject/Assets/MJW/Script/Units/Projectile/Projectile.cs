using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;

    public bool isEnemy = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collider collision){
        if(collision.tag == "Unit"){
            Unit target = collision.gameObject.GetComponent<Unit>();

            if(target.isEnemy != isEnemy){
                


            }
        }
    }
}
