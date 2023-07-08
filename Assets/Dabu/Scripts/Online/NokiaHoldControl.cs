using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NokiaHoldControl : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public float speed;

    private float startX;
    public Transform leftTargetPos;

    public GameObject targetAnim;
    public GameObject originAnim;
    public bool animStart;
    
    void Start()
    {
        startX =leftHand.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animStart)
        {
            if (Input.GetKey(KeyCode.Mouse0)  && leftHand.position.x < leftTargetPos.position.x )
            {
                Debug.Log("1111");
                //leftHand move right
                leftHand.Translate(Vector3.right * (speed * Time.deltaTime));
                //rightHand move left
                rightHand.Translate(Vector3.left * (speed * Time.deltaTime));
            
            
            }

            if (leftHand.position.x < leftTargetPos.position.x && leftHand.position.x > startX && !Input.GetKey(KeyCode.Mouse0)  )
            {
                //leftHand move left
                leftHand.Translate(Vector3.left * (speed * Time.deltaTime));
                //rightHand move right
                rightHand.Translate(Vector3.right * (speed * Time.deltaTime));
            }

            if (leftHand.position.x >= leftTargetPos.position.x)
            {
                animStart = true;
                targetAnim.SetActive(true);
                originAnim.SetActive(false);
                AudioManager.instance.Play("Nokia");
            }
        }
        
        
        
        
 
 
        
        
    }
}
