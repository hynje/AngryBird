using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Properties")]
    public float health = 100f;
    public float damageMultiplier = 1f;
    public float minimumDamageThreshold = 5f;
    private AudioSource audioSource;
    public AudioClip destroySound;
    public AudioClip[] onCollisionSounds;
    
    public ParticleSystem destroyEffect;

    private void Start()
    {
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
            GetComponent<BoxCollider2D>().enabled = false;
            DestroyObstacle();
        }
    }

    void PlayHitSound()
    {
        audioSource.PlayOneShot(onCollisionSounds[Random.Range(0, onCollisionSounds.Length)]);
    }
    void DestroyObstacle()
    {
        // 파괴 효과 생성
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // 파괴 사운드 재생
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }

        gameObject.SetActive(false);
    }
}
