using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slingshot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public LineRenderer[] lineRenderers; 
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;
    public List<Bird> birdsList;
    private Camera mainCamera;
    public GameManager gameManager;
    
    private Vector3 startPosition;
    public Vector3 currentPosition;
    private Vector3 force;
    
    public float maxLength;
    public float bottomBoundary;
    
    bool isMouseDown;

    private void Awake()
    {
        mainCamera = Camera.main;
        ResetStrips();
    }

    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers [1].positionCount = 2;
        lineRenderers [0]. SetPosition(0, stripPositions[0].position); 
        lineRenderers [1].SetPosition(0, stripPositions[1].position);

        int count = gameManager.birds.Count;
        for (var i = 0; i < count; i++)
        {
            var go = gameManager.birds[i].GetComponent<Bird>();
            birdsList.Add(go);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
        
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, 
                eventData.position.y , 
                mainCamera.WorldToScreenPoint(transform.position).z));
            
        startPosition = mouseWorldPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (birdsList.Count > 0)
        {
            birdsList[0].ShootBird(force);
            birdsList.RemoveAt(0);
            gameManager.birds.RemoveAt(0);
            gameManager.MoveBird(birdsList.Count);
        }
        ResetStrips();
        isMouseDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mainCamera != null && birdsList.Count > 0)
        {
            currentPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(eventData.position.x, 
                    eventData.position.y, 
                    mainCamera.WorldToScreenPoint(transform.position).z));
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLength);
            currentPosition = ClampBoundary(currentPosition);
            
            SetStrips(currentPosition);
            force = birdsList[0].LaunchBird(currentPosition);
        }

    }
    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition );
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1,position);
        lineRenderers[1].SetPosition(1,position);
    }

    Vector3 ClampBoundary(Vector3 v)
    {
        v.y = Mathf.Clamp(v.y, bottomBoundary, 1000);
        return v;
    }
    
}
