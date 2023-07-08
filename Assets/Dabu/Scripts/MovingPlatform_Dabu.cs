using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatform_Dabu : MonoBehaviour
{
    public Transform targetPos;
    
    public bool isMoving;

    public UnityEvent standEvent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //using DoTween move to move the platform to the target position and then back to the original position and loop
            //transform.DOMove(targetPos.position, 2f).SetLoops(lo)

            standEvent?.Invoke();
            

        }
    }
}
