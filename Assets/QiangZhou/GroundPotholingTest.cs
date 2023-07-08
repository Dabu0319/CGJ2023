using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPotholingTest : MonoBehaviour
{
    public Transform[] dig;
 
    public GameObject groundPrefab;
    public GameObject[] groundPool = new GameObject[30];
    public int curPoolIndex = 1;
    public List<GameObject> groundClipsCache;
    [Header("防止碰撞体挤压的空隙")]
    public float offset = 0.001f;

    void Start()
    {
        InitGround();
    }

    void Update()
    {
        //恢复初始状态
        ResetGround();

        //挖地,临时生成的ground存储在groundClipsCache中
        foreach (var value in dig)
        {
            DigGround(value);
        }

    }
   
    void InitGround()
    {
        groundClipsCache = new List<GameObject>();
        //预存储一些ground，以防使用,第一个永远是原始版本
        for (int i = 0; i < groundPool.Length; i++)
        {
            groundPool[i] = Instantiate(groundPrefab);
            groundPool[i].SetActive(false);
        }
        groundPool[0].SetActive(true);
        groundClipsCache.Add(groundPool[0]);
    }

    void ResetGround()
    {
        for(int i = 0; i< groundClipsCache.Count;i++)
        {
            groundClipsCache[i].transform.localScale = groundPool[0].transform.localScale;
            groundClipsCache[i].transform.position = groundPool[0].transform.position;
            groundClipsCache[i].SetActive(false);
        }
        groundClipsCache = new List<GameObject>();
        groundClipsCache.Add(groundPool[0]);
        groundPool[0].SetActive(true);
        curPoolIndex = 1;
    }

    private bool OverlapAreaTest(Transform t1, Transform t2)
    {
        if (Mathf.Abs(t1.position.x - t2.position.x) > (t1.localScale.x + t2.localScale.x) / 2 ||
            Mathf.Abs(t1.position.y - t2.position.y) > (t1.localScale.y + t2.localScale.y) / 2)
            return false;
        return true;
    }

    enum InsidePos
    {
        LEFTTOP = 1,
        LEFTDOWN = 2,
        RIGHTTOP = 4,
        RIGHTDOWN = 8
    }

    struct Pair
    {
        public Pair(Vector3 center,Vector3 scale)
        {
            this.center = center;
            this.scale = scale;
        }
        public Vector3 center;
        public Vector3 scale;
    }

    public void DigGround(Transform digRect)
    {
        Vector2 digPos = digRect.position;
        Vector2 digSize = digRect.localScale;
        Vector2 digPosLeftTop = new Vector2(digPos.x - digSize.x / 2 - offset, digPos.y + digSize.y / 2f + offset);
        Vector2 digPosLeftDown =  digPos - digSize / 2 - new Vector2( offset, offset);
        Vector2 digPosRightTop = digPos + digSize / 2 + new Vector2(offset, offset);
        Vector2 digPosRightDown = new Vector2(digPos.x + digSize.x / 2 + offset, digPos.y - digSize.y / 2f - offset);

        List<GameObject> tempCache = new List<GameObject>();
        for(int i = 0; i <groundClipsCache.Count;i++) 
        {
            var groundGO = groundClipsCache[i];
            Vector2 groundClipPos = groundGO.transform.position;
            Vector2 groundClipScale = groundGO.transform.localScale;
            if (OverlapAreaTest(digRect, groundGO.transform))
            {
                Vector2 groundPosLeftTop = new Vector2(groundClipPos.x - groundClipScale.x / 2, groundClipPos.y + groundClipScale.y / 2f);
                Vector2 groundPosLeftDown = groundClipPos - groundClipScale / 2;
                Vector2 groundPosRightTop = groundClipPos + groundClipScale / 2;
                Vector2 groundPosRightDown = new Vector2(groundClipPos.x + groundClipScale.x / 2, groundClipPos.y - groundClipScale.y / 2f);

                int insidePosNum = 0;
                List<InsidePos> insidePosEnum = new List<InsidePos>();
                if (digPosLeftTop.x >= groundPosLeftTop.x && digPosLeftTop.x <= groundPosRightTop.x
                    && digPosLeftTop.y <= groundPosLeftTop.y && digPosLeftTop.y >= groundPosLeftDown.y)
                {
                    insidePosNum++;
                    insidePosEnum.Add(InsidePos.LEFTTOP);
                }
                if (digPosLeftDown.x >= groundPosLeftTop.x && digPosLeftDown.x <= groundPosRightTop.x
    && digPosLeftDown.y <= groundPosLeftTop.y && digPosLeftDown.y >= groundPosLeftDown.y)
                {
                    insidePosNum++;
                    insidePosEnum.Add(InsidePos.LEFTDOWN);
                }
                if (digPosRightTop.x >= groundPosLeftTop.x && digPosRightTop.x <= groundPosRightTop.x
    && digPosRightTop.y <= groundPosLeftTop.y && digPosRightTop.y >= groundPosLeftDown.y)
                {
                    insidePosNum++;
                    insidePosEnum.Add(InsidePos.RIGHTTOP);
                }
                if (digPosRightDown.x >= groundPosLeftTop.x && digPosRightDown.x <= groundPosRightTop.x
&& digPosRightDown.y <= groundPosLeftTop.y && digPosRightDown.y >= groundPosLeftDown.y)
                {
                    insidePosNum++;
                    insidePosEnum.Add(InsidePos.RIGHTDOWN);
                }
                List<Pair> spawnRect = new List<Pair>();
                Vector3 spawnOneCenter = new Vector3();
                Vector3 spawnOneScale = new Vector3();
                Vector3 spawnTwoCenter = new Vector3();
                Vector3 spawnTwoScale = new Vector3();
                Vector3 spawnThreeCenter = new Vector3();
                Vector3 spawnThreeScale = new Vector3();
                Vector3 spawnFourCenter = new Vector3();
                Vector3 spawnFourScale = new Vector3();
                InsidePos insidePosOne;
                InsidePos insidePosTwo;
                switch (insidePosNum)
                {
                    case 0:
                        if (digPosLeftTop.x< groundPosLeftTop.x&&digPosLeftTop.y>groundPosLeftTop.y)
                        {
                            break;
                        }
                            else if (groundPosLeftTop.x < digPosLeftTop.x)
                        {
                            spawnOneCenter.x = (groundPosLeftTop.x + digPosLeftTop.x) / 2;
                            spawnOneCenter.y = groundClipPos.y;
                            spawnOneCenter.z = groundGO.transform.position.z;

                            spawnOneScale.x = digPosLeftTop.x - groundPosLeftTop.x;
                            spawnOneScale.y = groundClipScale.y;
                            spawnOneScale.z = groundGO.transform.localScale.z;

                            spawnTwoCenter.x = (groundPosRightTop.x + digPosRightTop.x) / 2;
                            spawnTwoCenter.y = groundClipPos.y;
                            spawnTwoCenter.z = groundGO.transform.position.z;

                            spawnTwoScale.x = groundPosRightTop.x - digPosRightTop.x;
                            spawnTwoScale.y = groundClipScale.y;
                            spawnTwoScale.z = groundGO.transform.localScale.z;
                        }
                        else
                        {
                            spawnOneCenter.x = groundClipPos.x;
                            spawnOneCenter.y = (groundPosLeftTop.y+ digPosLeftTop.y) / 2;
                            spawnOneCenter.z = groundGO.transform.position.z;

                            spawnOneScale.x = groundClipScale.x;
                            spawnOneScale.y = groundPosLeftTop.y - digPosLeftTop.y;
                            spawnOneScale.z = groundGO.transform.localScale.z;

                            spawnTwoCenter.x = groundClipPos.x;
                            spawnTwoCenter.y = (groundPosLeftDown.y + digPosLeftDown.y) / 2;
                            spawnTwoCenter.z = groundGO.transform.position.z;

                            spawnTwoScale.x = groundClipScale.x;
                            spawnTwoScale.y = digPosLeftDown.y - groundPosLeftDown.y;
                            spawnTwoScale.z = groundGO.transform.localScale.z;
                        }
                        break;

                    case 1:
                        insidePosOne = insidePosEnum[0];
                        switch (insidePosOne)
                        {
                            case InsidePos.LEFTTOP:

                                spawnOneCenter.x = (groundPosLeftTop.x + digPosLeftTop.x) / 2;
                                spawnOneCenter.y = groundClipPos.y;
                                spawnOneCenter.z = groundGO.transform.position.z;

                                spawnOneScale.x = digPosLeftTop.x - groundPosLeftTop.x;
                                spawnOneScale.y = groundClipScale.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;
                                
                                spawnTwoCenter.x = (groundPosRightTop.x + digPosLeftTop.x) / 2;
                                spawnTwoCenter.y = (groundPosRightTop.y + digPosLeftTop.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;
                               
                                spawnTwoScale.x = groundPosRightTop.x - digPosLeftTop.x;
                                spawnTwoScale.y = groundPosRightTop.y - digPosLeftTop.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;
                                //生成矩阵
                                break;
                            case InsidePos.RIGHTTOP:
                                spawnOneCenter.x = (groundPosRightTop.x + digPosRightTop.x) / 2;
                                spawnOneCenter.y = groundClipPos.y;
                                spawnOneCenter.z = groundGO.transform.position.z;
                             
                                spawnOneScale.x = groundPosRightTop.x - digPosRightTop.x;
                                spawnOneScale.y = groundClipScale.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;
                         
                                spawnTwoCenter.x = (groundPosLeftTop.x + digPosRightTop.x) / 2;
                                spawnTwoCenter.y = (groundPosLeftTop.y + digPosRightTop.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;
                          
                                spawnTwoScale.x = digPosRightTop.x - groundPosLeftTop.x;
                                spawnTwoScale.y = groundPosLeftTop.y - digPosRightTop.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;
                                break;
                            case InsidePos.LEFTDOWN:
                                spawnOneCenter.x = (groundPosLeftTop.x + digPosLeftDown.x) / 2;
                                spawnOneCenter.y = groundClipPos.y;
                                spawnOneCenter.z = groundGO.transform.position.z;
                        
                                spawnOneScale.x = digPosLeftDown.x - groundPosLeftTop.x;
                                spawnOneScale.y = groundClipScale.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;
    
                                spawnTwoCenter.x = (groundPosRightDown.x + digPosLeftDown.x) / 2;
                                spawnTwoCenter.y = (groundPosRightDown.y + digPosLeftDown.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;

                                spawnTwoScale.x = groundPosRightDown.x - digPosLeftDown.x;
                                spawnTwoScale.y = digPosLeftDown.y - groundPosRightDown.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;
                                break;
                            case InsidePos.RIGHTDOWN:
                                spawnOneCenter.x = (groundPosRightTop.x + digPosRightDown.x) / 2;
                                spawnOneCenter.y = groundClipPos.y;
                                spawnOneCenter.z = groundGO.transform.position.z;

                                spawnOneScale.x = groundPosRightTop.x - digPosRightDown.x;
                                spawnOneScale.y = groundClipScale.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;

                                spawnTwoCenter.x = (groundPosLeftDown.x + digPosRightDown.x) / 2;
                                spawnTwoCenter.y = (groundPosLeftDown.y + digPosRightDown.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;

                                spawnTwoScale.x = digPosRightDown.x - groundPosLeftDown.x;
                                spawnTwoScale.y = digPosRightDown.y - groundPosLeftDown.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;
                                break;
                        }
                        spawnRect.Add(new Pair(spawnOneCenter, spawnOneScale));
                        spawnRect.Add(new Pair(spawnTwoCenter, spawnTwoScale));
                        break;
                    case 2:
                        insidePosOne = insidePosEnum[0];
                        insidePosTwo = insidePosEnum[1];
                        int borderBit = (int)insidePosOne | (int)insidePosTwo;
                        switch(borderBit)
                        {
                            case 3://左平面进入

                                spawnOneCenter.x = groundClipPos.x;
                                spawnOneCenter.y = (groundPosRightTop.y + digPosLeftTop.y) / 2;
                                spawnOneCenter.z = groundGO.transform.position.z;
  
                                spawnOneScale.x = groundClipScale.x;
                                spawnOneScale.y = groundPosRightTop.y - digPosLeftTop.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;

                                spawnTwoCenter.x = (groundPosLeftTop.x + digPosLeftTop.x) / 2;
                                spawnTwoCenter.y = (digPosLeftTop.y + digPosLeftDown.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;

                                spawnTwoScale.x = digPosLeftTop.x - groundPosLeftTop.x;
                                spawnTwoScale.y = digPosLeftTop.y - digPosLeftDown.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;

                                spawnThreeCenter.x = groundClipPos.x;
                                spawnThreeCenter.y = (groundPosRightDown.y + digPosLeftDown.y) / 2;
                                spawnThreeCenter.z = groundGO.transform.position.z;

                                spawnThreeScale.x = groundClipScale.x;
                                spawnThreeScale.y =  digPosLeftDown.y - groundPosRightDown.y;
                                spawnThreeScale.z = groundGO.transform.localScale.z;
                                break;
                            case 5://上平面
                                spawnOneCenter.x = (groundPosLeftTop.x + digPosLeftTop.x) / 2;
                                spawnOneCenter.y = groundClipPos.y;
                                spawnOneCenter.z = groundGO.transform.position.z;

                                spawnOneScale.x = digPosLeftTop.x - groundPosLeftTop.x;
                                spawnOneScale.y = groundClipScale.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;

                                spawnTwoCenter.x = (digPosLeftTop.x + digPosRightTop.x) / 2;
                                spawnTwoCenter.y = (groundPosLeftTop.y + digPosRightTop.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;

                                spawnTwoScale.x = digPosRightTop.x - digPosLeftTop.x;
                                spawnTwoScale.y = groundPosLeftTop.y - digPosRightTop.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;

                                spawnThreeCenter.x = (groundPosRightDown.x + digPosRightTop.x) / 2;
                                spawnThreeCenter.y = groundClipPos.y;
                                spawnThreeCenter.z = groundGO.transform.position.z;

                                spawnThreeScale.x = groundPosRightDown.x - digPosRightTop.x;
                                spawnThreeScale.y = groundClipScale.y;
                                spawnThreeScale.z = groundGO.transform.localScale.z;
                                break;
                            case 10://下平面
                                spawnOneCenter.x = (groundPosLeftTop.x + digPosLeftDown.x) / 2;
                                spawnOneCenter.y = groundClipPos.y;
                                spawnOneCenter.z = groundGO.transform.position.z;

                                spawnOneScale.x = digPosLeftDown.x - groundPosLeftTop.x;
                                spawnOneScale.y = groundClipScale.y;
                                spawnOneScale.z = groundGO.transform.localScale.z;

                                spawnTwoCenter.x = (digPosLeftDown.x + digPosRightDown.x) / 2;
                                spawnTwoCenter.y = (groundPosLeftDown.y + digPosRightDown.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;

                                spawnTwoScale.x = digPosRightDown.x - digPosLeftDown.x;
                                spawnTwoScale.y = digPosRightDown.y - groundPosLeftDown.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;

                                spawnThreeCenter.x = (groundPosRightDown.x + digPosRightDown.x) / 2;
                                spawnThreeCenter.y = groundClipPos.y;
                                spawnThreeCenter.z = groundGO.transform.position.z;

                                spawnThreeScale.x = groundPosRightDown.x - digPosRightDown.x;
                                spawnThreeScale.y = groundClipScale.y;
                                spawnThreeScale.z = groundGO.transform.localScale.z;
                                break;
                            case 12://右平面
                                Debug.Log("CheckRight");
                                spawnOneCenter.x = groundClipPos.x;
                                spawnOneCenter.y = (groundPosRightTop.y + digPosRightTop.y) / 2;
                                spawnOneCenter.z = groundGO.transform.position.z;

                                spawnOneScale.x = groundClipScale.x;
                                spawnOneScale.y = groundPosRightTop.y - digPosRightTop.y;
                                Debug.Log(spawnOneScale.y);
                                spawnOneScale.z = groundGO.transform.localScale.z;

                                spawnTwoCenter.x = (groundPosRightTop.x + digPosRightTop.x) / 2;
                                spawnTwoCenter.y = (digPosRightTop.y + digPosRightDown.y) / 2;
                                spawnTwoCenter.z = groundGO.transform.position.z;

                                spawnTwoScale.x = groundPosRightTop.x - digPosRightTop.x;
                                spawnTwoScale.y = digPosRightTop.y - digPosRightDown.y;
                                spawnTwoScale.z = groundGO.transform.localScale.z;

                                spawnThreeCenter.x = groundClipPos.x;
                                spawnThreeCenter.y = (groundPosRightDown.y + digPosRightDown.y) / 2;
                                spawnThreeCenter.z = groundGO.transform.position.z;
                 
                                spawnThreeScale.x = groundClipScale.x;
                                spawnThreeScale.y = digPosRightDown.y - groundPosRightDown.y;
                                spawnThreeScale.z = groundGO.transform.localScale.z;
                                break;
                        }
                        spawnRect.Add(new Pair(spawnOneCenter, spawnOneScale));
                        spawnRect.Add(new Pair(spawnTwoCenter, spawnTwoScale));
                        spawnRect.Add(new Pair(spawnThreeCenter, spawnThreeScale));               
                        break;
                    case 4:
                        spawnOneCenter.x = (digPosLeftTop.x + groundPosLeftTop.x) / 2;
                        spawnOneCenter.y = groundClipPos.y;
                        spawnOneCenter.z = groundGO.transform.position.z;

                        spawnOneScale.x = digPosLeftTop.x - groundPosLeftTop.x;
                        spawnOneScale.y = groundClipScale.y;
                        spawnOneScale.z = groundGO.transform.localScale.z;

                        spawnTwoCenter.x = (digPosLeftTop.x + digPosRightTop.x) / 2;
                        spawnTwoCenter.y = (groundPosLeftTop.y + digPosRightTop.y) / 2;
                        spawnTwoCenter.z = groundGO.transform.position.z;

                        spawnTwoScale.x = digPosRightTop.x - digPosLeftTop.x;
                        spawnTwoScale.y = groundPosLeftTop.y - digPosRightTop.y;
                        spawnTwoScale.z = groundGO.transform.localScale.z;

                        spawnThreeCenter.x = (digPosRightTop.x + groundPosRightDown.x) / 2;
                        spawnThreeCenter.y = groundClipPos.y;
                        spawnThreeCenter.z = groundGO.transform.position.z;

                        spawnThreeScale.x = groundPosRightTop.x - digPosRightTop.x ;
                        spawnThreeScale.y = groundClipScale.y;
                        spawnThreeScale.z = groundGO.transform.localScale.z;

                        spawnFourCenter.x = (digPosLeftTop.x + digPosRightTop.x) / 2;
                        spawnFourCenter.y = (groundPosRightDown.y + digPosRightDown.y) / 2;
                        spawnFourCenter.z = groundGO.transform.position.z;

                        spawnFourScale.x = digPosRightTop.x - digPosLeftTop.x;
                        spawnFourScale.y = digPosRightDown.y - groundPosRightDown.y;
                        spawnFourScale.z = groundGO.transform.localScale.z;
                        
                        spawnRect.Add(new Pair(spawnOneCenter, spawnOneScale));
                        spawnRect.Add(new Pair(spawnTwoCenter, spawnTwoScale));
                        spawnRect.Add(new Pair(spawnThreeCenter, spawnThreeScale));
                        spawnRect.Add(new Pair(spawnFourCenter, spawnFourScale));
                        break;
                }
                //弹出被切除的ground
                GameObject tempGO = groundGO;
                groundGO.SetActive(false);
                groundClipsCache[i] = groundClipsCache[groundClipsCache.Count - 1];
                groundClipsCache[groundClipsCache.Count - 1] = tempGO;
                groundClipsCache.RemoveAt(groundClipsCache.Count - 1);
                i--;
                foreach (var value in spawnRect)
                {
                    groundPool[curPoolIndex].transform.position = value.center;
                    groundPool[curPoolIndex].transform.localScale = value.scale;
                    groundPool[curPoolIndex].SetActive(true);
                    tempCache.Add(groundPool[curPoolIndex]);
                    curPoolIndex++;
                }
            }
        }
        for(int i = 0; i < tempCache.Count; i++)
        {
            groundClipsCache.Add(tempCache[i]);
        }
    }

    private void SetBoundary(Vector4 v, Transform t)
    {
        if (v.x >= v.y || v.w >= v.z)
        {//左边大于右边，下边大于上边，则关闭
            t.gameObject.SetActive(false);
            return;
        }
        if (!t.gameObject.activeSelf)
            t.gameObject.SetActive(true);

        float posx = (v.x + v.y) / 2f;
        float posy = (v.z + v.w) / 2f;
        float scalex = v.y - v.x;
        float scaley = v.z - v.w;
        t.position = new Vector3(posx, posy);
        t.localScale = new Vector3(scalex, scaley);
    }

    private Vector4 Boundary2Center(Vector4 v)
    {
        return Vector4.one;
    }

    private Vector4 Center2Boundary(Transform t)
    {
        Vector4 b = new Vector4(
            t.position.x - t.localScale.x / 2,
            t.position.x + t.localScale.x / 2,
            t.position.y + t.localScale.y / 2,
            t.position.y - t.localScale.y / 2);
        return b;
    }
}
