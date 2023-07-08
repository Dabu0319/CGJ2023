using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeToDie : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] grounds;
    public GameObject Player;
    void Start()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void InWall()
    {
        LevelLoader.instance.RestartLevel();
    }
    // Update is called once per frame
    void Update()
    {
        foreach(var ground in grounds)
        {
            BoxCollider2D bx2D = ground.GetComponent<BoxCollider2D>();
            Vector3 bxsize = bx2D.bounds.extents;
            float lW = ground.transform.position.x - bxsize.x;
            float rW = ground.transform.position.x + bxsize.x;
            float uW = ground.transform.position.y + bxsize.y;
            float dW = ground.transform.position.y - bxsize.y;
            var plyr2D = Player.transform.position;
            if (plyr2D.x > lW&&plyr2D.x <rW&&plyr2D.y>dW&&plyr2D.y<uW)
            {
                InWall();
            }
        }
    }
}
