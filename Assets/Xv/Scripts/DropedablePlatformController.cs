using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedablePlatformController : MonoBehaviour
{

    Rigidbody2D rb;
    public GameObject upCheck;
    
    ConfigurableJoint joint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = rb.gameObject.AddComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y == -1)
        {
            rb.gravityScale = 1;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
