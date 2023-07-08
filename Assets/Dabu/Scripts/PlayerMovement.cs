using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerID")] 

    
    private CharacterController2D _controller2D;

    private float _horizontalMove;
    private bool _jump;

    public float runSpeed;


    public GameObject characterHolder;
    void Start()
    {
        _controller2D = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager_Dabu.instance.isDrag)
        {
            _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
        
        if (GameManager_Dabu.instance.isDrag)
        {
            _horizontalMove = 0;
        }
        

        if (Input.GetButtonDown("Jump") && !GameManager_Dabu.instance.isDrag)
        {
            if (_controller2D.m_Grounded == enabled)
            {
                StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
            }
            // jumpTimer = Time.time + jumpDelay;
            _jump = true;
            
        }
    }

    private void FixedUpdate()
    {
        
            _controller2D.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
            _jump = false;
        
        
        if (GameManager_Dabu.instance.isDrag)
        {

            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }

        
    }
    
    
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds) {
        Vector3 originalSize = characterHolder.transform.localScale;
        Vector3 newSize = new Vector3(originalSize.x*xSqueeze,originalSize.y* ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
}
