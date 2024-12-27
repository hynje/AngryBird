using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public GameManager gameManager;
    public List<Bird> birdsList;
    public Transform target;
    
    private Vector3 offset = new Vector3(0, 0, -10);
    
    void Start()
    {
        int count = gameManager.birds.Count;
        for (var i = 0; i < count; i++)
        {
            var go = gameManager.birds[i].GetComponent<Bird>();
            birdsList.Add(go);
        }
        SetTarget();
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    void SetTarget()
    {
        if (birdsList.Count > 0 && birdsList[0] != null)
        {
            target = birdsList[0].transform;
        }
    }

    void MoveCamera()
    {
        if (target != null)
        {
            if(target.position.x >= 0)
            {
                Vector3 targetPos = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
                targetPos = new Vector3(Mathf.Clamp(targetPos.x, -1, 23), targetPos.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, offset, Time.deltaTime * 10f);
            }
        }
        else
        {
            if (birdsList.Count > 0)
            {
                birdsList.RemoveAt(0);
                SetTarget();
            }
        }
    }
}
