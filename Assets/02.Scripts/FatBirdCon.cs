using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBirdCon : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    public Bird bird;
    private bool skillUsed = false;

    private float power = 100f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (skillUsed == false && Input.GetMouseButtonDown(0))
        {
            Smash();
        }
    }
    void Smash()
    {
        if (bird.currentState == Bird.States.Flying)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.AddForce(-transform.up * power, ForceMode2D.Impulse);
            animator.Play("FatBirdSmash");
            
            skillUsed = true;
        }
    }
}
