using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl_Line : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public Transform touchPos;
    public Transform originPos;

    public GameObject touchObj;
    private Vector3 mousePos;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            touchObj.SetActive(true);
            touchObj.transform.position = new Vector3(mousePos.x,mousePos.y,0);
            
            
            
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.SetPosition(0,originPos.position);
            lineRenderer.SetPosition(1,touchObj.transform.position);
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
            touchObj.SetActive(false);
        }
        
        
        
    }
    
    
}
