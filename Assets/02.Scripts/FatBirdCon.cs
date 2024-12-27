using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatBirdCon : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    public Bird bird;
    public AudioClip explosionSound;
    public ParticleSystem explosionEffect;
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

        if (skillUsed == true && bird.currentState == Bird.States.Crushed)
        {
            Explode();
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
    void Explode()
    {
        // 폭발 효과 표시
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSound,transform.position + new Vector3(0, 0, -10));
        }

        // 폭발 반경 내 객체에 힘 적용
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (rb.transform.position - transform.position).normalized;
                rb.AddForce(direction * 500f);
            }
        }
        // 폭탄 제거
        Destroy(gameObject);
    }

}
