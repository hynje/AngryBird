using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [Header("Pig Properties")]
    public float health = 8f;
    public float damageMultiplier = 1f;
    public float minimumDamageThreshold = 5f;
    
    public GameManager gameManager;

    private void Start()
    {
        gameManager.CountPig(1);
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
        //PlayHitSound();
    }
    void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);

        if (health <= 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            DestroyPig();
        }
    }
    void DestroyPig()
    {
        // 파괴 효과 생성
        // if (destroyEffect != null)
        // {
        //     Instantiate(destroyEffect, transform.position, Quaternion.identity);
        // }

        // 파괴 사운드 재생
        // if (destroySound != null)
        // {
        //     AudioSource.PlayClipAtPoint(destroySound, transform.position);
        // }

        // 게임 매니저에 알림
        //GameManager.Instance.OnObstacleDestroyed();

        gameManager.CountPig(-1);
        gameObject.SetActive(false);
    }
}
