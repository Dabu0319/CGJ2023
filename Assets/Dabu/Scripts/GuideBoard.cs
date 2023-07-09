using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GuideBoard : MonoBehaviour
{
    public GameObject guideBoard;

   
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
            
            if (col.gameObject.CompareTag("Player"))
            {
                
                
                
                //Restart doTween animation
                GetComponent<DOTweenAnimation>().DORestartAllById("1");
                
            }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<DOTweenAnimation>().DORestartAllById("2");
        }
        
    }
}
