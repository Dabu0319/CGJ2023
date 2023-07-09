using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPotholing : MonoBehaviour
{
    public Transform dig;
    public Transform ground;
    
    public Transform left;
    public Transform right;
    public Transform top;
    public Transform bottom;
    [Header("防止碰撞体挤压的空隙")]
    public float offset = 0.001f;
    
    private Vector4 oriB;
    void Start()
    {
        oriB = Center2Boundary(ground);
        
        
    }
    
    void Update()
    {
        if (OverlapAreaTest(dig, ground))
        {
            Vector4 digB = Center2Boundary(dig);
            Vector4 leftB = new Vector4(digB.y+offset, oriB.y, oriB.z, oriB.w);
            Vector4 rightB = new Vector4(oriB.x, digB.x-offset, oriB.z, oriB.w);
            Vector4 topB = new Vector4(oriB.x, oriB.y, digB.w-offset, oriB.w);
            Vector4 bottomB = new Vector4(oriB.x, oriB.y, oriB.z, digB.z+offset);
            SetBoundary(leftB,left);
            SetBoundary(rightB,right);
            SetBoundary(topB,top);
            SetBoundary(bottomB,bottom);
        }
        else
        {
            SetBoundary(oriB,left);
            SetBoundary(oriB,right);
            SetBoundary(oriB,top);
            SetBoundary(oriB,bottom);
        }
    }

    private bool OverlapAreaTest(Transform t1,Transform t2)
    {
        if (Mathf.Abs(t1.position.x - t2.position.x) > (t1.localScale.x + t2.localScale.x) / 2 ||
            Mathf.Abs(t1.position.y - t2.position.y) > (t1.localScale.y + t2.localScale.y) / 2)
            return false;
        return true;
    }

    private void SetBoundary(Vector4 v,Transform t)
    {
        if (v.x >= v.y || v.w >= v.z)
        {//左边大于右边，下边大于上边，则关闭
            t.gameObject.SetActive(false);
            return;
        }
        if(!t.gameObject.activeSelf)
            t.gameObject.SetActive(true);
        
        float posx = (v.x + v.y) / 2f;
        float posy = (v.z + v.w) / 2f;
        float scalex = v.y - v.x;
        float scaley = v.z - v.w;
        t.position = new Vector3(posx,posy);
        t.localScale = new Vector3(scalex,scaley);
    }

    private Vector4 Boundary2Center(Vector4 v)
    {
        return Vector4.one;
    }

    private Vector4 Center2Boundary(Transform t)
    {
        Vector4 b = new Vector4(
            t.position.x - t.localScale.x/2,
            t.position.x + t.localScale.x/2,
            t.position.y + t.localScale.y/2,
            t.position.y - t.localScale.y/2);
        return b;
    }
}
