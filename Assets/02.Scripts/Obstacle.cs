using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Properties")]
    public float health = 100f;
    public float damageMultiplier = 1f;
    public float minimumDamageThreshold = 5f;
    
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
        // 데미지 시각 효과
        //StartCoroutine(DamageEffect());

        if (health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            DestroyObstacle();
        }
    }
    void DestroyObstacle()
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

        gameObject.SetActive(false);
    }
}
