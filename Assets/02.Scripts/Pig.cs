using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pig : MonoBehaviour
{
    [Header("Pig Properties")]
    public float health = 8f;
    public float damageMultiplier = 1f;
    public float minimumDamageThreshold = 5f;
    private AudioSource audioSource;
    public AudioClip destroySound;
    public AudioClip[] onCollisionSounds;
    
    public GameManager gameManager;
    public ParticleSystem destroyEffect;

    private void Start()
    {
        gameManager.CountPig(1);
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        // 최소 충격량보다 작으면 데미지 없음
        if (impactForce < minimumDamageThreshold) return;

        // 데미지 계산
        float damage = impactForce * damageMultiplier;
        TakeDamage(damage);

        // 충돌 사운드
        PlayHitSound();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            DestroyPig();
        }
    }
    void PlayHitSound()
    {
        audioSource.PlayOneShot(onCollisionSounds[Random.Range(0, onCollisionSounds.Length)]);
    }
    void DestroyPig()
    {
        // 파괴 효과 생성
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position - new Vector3(0,0,1), Quaternion.identity);
        }

        //파괴 사운드 재생
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
        
        Destroy(gameObject);
        gameManager.CountPig(-1);
    }
}
