using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    public Transform block1;
    public Transform block2;
    public Transform block3;
    public Transform overlapArea12;
    public Transform overlapArea13;
    public Transform overlapArea23;
    

    private void Update()
    {
        OverlapAreaTest(block1,block2,ref overlapArea12);
        OverlapAreaTest(block1,block3,ref overlapArea13);
        OverlapAreaTest(block2,block3,ref overlapArea23);
    }
    private void OverlapAreaTest(Transform t1,Transform t2, ref Transform t0)
    {
        if (Mathf.Abs(t1.position.x - t2.position.x) > (t1.localScale.x + t2.localScale.x)/2 ||
            Mathf.Abs(t1.position.y - t2.position.y) > (t1.localScale.y + t2.localScale.y)/2)
        {
            t0.gameObject.SetActive(false);
            return;
        }

        float[] xb = new float[4]
        {
            t1.position.x + t1.localScale.x / 2,
            t1.position.x - t1.localScale.x / 2,
            t2.position.x + t2.localScale.x / 2,
            t2.position.x - t2.localScale.x / 2
        };
        float[] yb = new float[4]
        {
            t1.position.y + t1.localScale.y / 2,
            t1.position.y - t1.localScale.y / 2,
            t2.position.y + t2.localScale.y / 2,
            t2.position.y - t2.localScale.y / 2
        };
        Array.Sort(xb);
        Array.Sort(yb);
        t0.transform.position = new Vector3((xb[2]+xb[1])/2,(yb[2]+yb[1])/2, 0f);
        t0.transform.localScale = new Vector3((xb[2]-xb[1]),(yb[2]-yb[1]), 0f);
        
        t0.gameObject.SetActive(true);
        return;
    }
}
