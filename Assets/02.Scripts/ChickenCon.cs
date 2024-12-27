using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCon : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    public Bird bird;
    private bool skillUsed = false;

    private float power = 70f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (skillUsed == false && Input.GetMouseButtonDown(0))
        {
            Dash();
        }

        if (transform.position.y < -30 && gameObject!=null)
        {
            Destroy(this.gameObject);
        }
    }

    void Dash()
    {
        if (bird.currentState == Bird.States.Flying)
        {
            Vector3 dashDir = transform.right;
            rb.AddForce(dashDir * power, ForceMode2D.Impulse);
            animator.Play("ChickenDash");
            
            skillUsed = true;
        }
    }
}
