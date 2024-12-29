using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public Transform launchPosition;
    public Transform[] positions;
    public List<GameObject> birds;
    private int pigCount = 0;
    public Slingshot slingshot;
    public EventSystem eventSystem;
    public UIManager uiManager;
    public AudioClip clearSound;
    public AudioClip failSound;
    private bool gameOver = false;
    public Camera mainCamera;
    
    private Vector3 origin = new Vector3(-6.5f, -4.5f, 0);  // slerp 원점 재설정
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    private void Awake()
    {
        SetBird();
    }

    void Start()
    {
        mainCamera = Camera.main;
        Time.timeScale = 1;
        MoveBird(birds.Count);
    }
    
    void Update()
    {
        if (birds.Count <= 0 && !gameOver)
        {
            StartCoroutine(CheckLeftPig());
            gameOver = true;
        }
    }

    void SetBird()
    {
        for (var i = 0; i < birds.Count; i++)
        {
           birds[i].transform.position = positions[i].position;
           var rb = birds[i].GetComponent<Rigidbody2D>();
           rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void MoveBird(int leftBirdCount)
    {
        if (leftBirdCount <= 0) return;
        Transform[] nextPos = new Transform[leftBirdCount];
        nextPos[0] = launchPosition;
        for (var i = 1; i < leftBirdCount; i++)
        {
            nextPos[i] = positions[i - 1];
        }

        for (var i = 0; i < leftBirdCount; i++)
        {
            StartCoroutine(MoveBirdCoroutine(i, nextPos[i].position));
        }
    }

    private IEnumerator MoveBirdCoroutine(int index, Vector3 nextPos)
    {
        eventSystem.enabled = false;
        float duration = 1f;
        float timer = 0f;
        Vector3 startPos = birds[index].transform.position;
        
        while (timer/duration <= 1.0f)
        {
            Vector3 p = Vector3.Slerp(startPos-origin, nextPos-origin, curve.Evaluate(timer/duration));
            birds[index].transform.position = p+origin;
            timer += Time.deltaTime;
            yield return null;
        }
        eventSystem.enabled = true;
    }

    public void CountPig(int x)
    {
        pigCount += x;
        Debug.Log(pigCount);
        if (pigCount <= 0)
        {
            StageClear();
        }
    }

    IEnumerator CheckLeftPig()
    {
        yield return new WaitForSeconds(6f);
        if (pigCount >= 1)
        {
            StageFailed();
        }
    }

    private void StageClear()
    {
        uiManager.ActiveClearUI();
        AudioSource.PlayClipAtPoint(clearSound, mainCamera.transform.position);
    }

    private void StageFailed()
    {
        uiManager.ActiveFailUI();
        AudioSource.PlayClipAtPoint(failSound, mainCamera.transform.position);
    }
}
