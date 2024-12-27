using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour
{
    [Header("Bird Properties")] 
    private float launchPower = 16f;
    private float maxLaunchSpeed = 38f;
    public Transform launchPoint;
    
    [Header("Bird AudioClips")]
    public AudioClip[] onCollisionSounds;
    public AudioClip selectSound;
    public AudioClip flySound;
    public AudioClip destroySound;

    [Header("Components")] 
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    
    public LineRenderer trajectoryRenderer;
    public enum States
    {
        Idle,
        Launched,
        Flying,
        Crushed
    };
    public States currentState = States.Idle;
    [Header("State")]
    private bool isDragging = false;
    private Vector3 initialPosition;
    public Transform centerPosition;
    private int maxStep = 30;
    private float rotationSpeed = 5f;

    private void Start()
    {
        initialPosition = centerPosition.position;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        trajectoryRenderer.positionCount = maxStep - 10;
    }
    void Update()
    {
        Vector2 velocity = rb.velocity;

        if (velocity.sqrMagnitude > 0.01f && currentState == States.Flying)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public Vector3 LaunchBird(Vector3 dragPosition)
    {
        // 방향 조준
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;
        currentState = States.Launched;
        
        transform.rotation = Quaternion.Euler(Vector3.zero);
        launchPoint.position = dragPosition;

        // 발사 방향과 힘 계산
        Vector3 force = (initialPosition - launchPoint.position) * launchPower;
        // 최대 속도 제한
        if (force.magnitude > maxLaunchSpeed)
        {
            force = force.normalized * maxLaunchSpeed;
        }

        var t = PredictTrajectory(force, rb.mass);
        DrawTrajectory(t);
        return force;
    }

    public void ShootBird(Vector3 force)
    {
        currentState = States.Flying;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = false;
        rb.AddForce(force, ForceMode2D.Impulse);
        audioSource.PlayOneShot(flySound);
        ClearTrajectory();
        // Trail 활성화
        //if (tr != null) tr.enabled = true;
    }

    List<Vector3> PredictTrajectory(Vector3 force, float mass)
    {
        List<Vector3> trajectory = new List<Vector3>();
        Vector3 position = transform.position;
        Vector3 velocity = force / mass;
        for (int i = 0; i < maxStep; i++)
        {
            float t = i * Time.fixedDeltaTime;

            // 공기 저항에 의한 감속 계산
            Vector3 dragForce = -rb.drag * velocity;
            Vector3 gravityEffect = Physics2D.gravity * mass; // 중력 계산
            Vector3 netForce = dragForce + gravityEffect; // 순수 힘

            // 속도와 위치 업데이트
            velocity += (netForce / mass) * Time.fixedDeltaTime;
            position += velocity * Time.fixedDeltaTime;

            // 현재 위치 추가
            trajectory.Add(position);
        }

        return trajectory;
    }

    void DrawTrajectory(List<Vector3> trajectory)
    {
        trajectoryRenderer.enabled = true;
        for (int i = 0; i < maxStep - 10; i++)
        {
            trajectoryRenderer.SetPosition(i, trajectory[i + 10]);
        }
    }

    void ClearTrajectory()
    {
        trajectoryRenderer.enabled = false;
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        if (gameObject != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(currentState == States.Flying)
        {
            currentState = States.Crushed;
            animator.SetBool("Hit", true);
        }
        PlayHitSound();
        if (currentState == States.Crushed)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }
    
    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }
    void PlayHitSound()
    {
        audioSource.PlayOneShot(onCollisionSounds[Random.Range(0, onCollisionSounds.Length)]);
    }
}
