using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    protected Collider unitCollider;
    protected int hp;
    protected int damage;
    protected float speed;

    public Unit(){
        hp = 100;
        damage = 10;
        speed = 5;
    }

    protected void Move(){
        transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
    }

    void Awake() {
        unitCollider = GetComponent<Collider>();
    }

    void Start(){

    }

    void FixedUpdate() {
        Move();
    }
}
