using System;
using UnityEngine;

public class Dragger_NoGravity : MonoBehaviour {

    private Vector3 _dragOffset;
    private Camera _cam;
    private Rigidbody2D _rb;
    
    

    [SerializeField] private float _speed = 100;

    void Awake() {
        _cam = Camera.main;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //set rigidbody constraints z axis none 
        


    }


    void OnMouseDown()
    {
        GameManager_Dabu.instance.isDrag = true;
        _dragOffset = transform.position - GetMousePos();
        //_rb.gravityScale = 0f;
        //_rb.bodyType = RigidbodyType2D.Kinematic;

        
    }

    void OnMouseDrag() {
        transform.position = Vector3.MoveTowards(transform.position, GetMousePos() + _dragOffset, _speed * Time.deltaTime) ;
        //_rb.AddForce(_dragOffset*_speed,ForceMode2D.Force);
        transform.position = GetMousePos() + _dragOffset;
        //GameManager_Dabu.instance.isDrag = true;
        //_rb.gravityScale = 0f;
    }

    private void OnMouseUp()
    {
        //_rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager_Dabu.instance.isDrag = false;
        //_rb.gravityScale = 1;
    }

    Vector3 GetMousePos() {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
        
        
  
    }
}