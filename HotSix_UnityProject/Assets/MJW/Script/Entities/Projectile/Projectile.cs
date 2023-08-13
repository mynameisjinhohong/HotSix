using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Properties

    public ProjectileAction action;

    public Vector3 startPos;
    public Vector3 midPos;
    public Vector3 endPos;

    public float curTime = 0.0f;
    public bool isEnemy;
    public bool isActive = false;
    

    #endregion


    #region Methods

    public void SetPos(Vector3 start, Vector3 mid, Vector3 end){
        startPos = start;
        midPos = mid;
        endPos = end;
        transform.position = startPos;
    }

    public void Fly(float deltaTime){
        float t = deltaTime / action.duration;

        Vector3 p1 = startPos * (1.0f - curTime) + midPos * curTime;
        Vector3 p2 = midPos * (1.0f - curTime) + endPos * curTime;
        Vector3 q = p1 * (1.0f - curTime) + p2 * curTime;

        Vector3 r = p2 - p1;
        float theta = System.MathF.Atan(r.y / r.x) * 180 / System.MathF.PI;

        transform.position = q;
        transform.localEulerAngles = new Vector3(0, isEnemy ? 0.0f : 180.0f, isEnemy ? theta : -theta);

        curTime += t;
    }

    #endregion


    #region Monobehavior Callbacks
    
    void Awake()
    {
        action.mainProjectile = transform.gameObject;
        transform.tag = "Projectile";
    }

    void FixedUpdate()
    {
        if(isActive){
            Fly(Time.deltaTime);

            if(curTime >= 1.0f){
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collider){
        action.targetObjects.Add(collider.gameObject);
        if(action.Condition()){
            action.ExecuteAction();
            Destroy(gameObject);
        }
        else{
            action.targetObjects.Clear();
        }
    }

    #endregion
}
