using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Vector3 _dragOffset;
    private Camera _cam;
    public Vector3 originalPos;
    public bool canDrag = true;
    public bool isDragging = false;
    public float _speed = 100;
    public Transform targetPos;
    public float distance = 1f;

    public virtual void Awake()
    {
        _cam = Camera.main;
        originalPos = transform.position;
    }

    public virtual void OnMouseDown()
    {
        if (canDrag)
        {
            isDragging = true;
            _dragOffset = transform.position - GetMousePos();
        }
    }

    public virtual void OnMouseDrag()
    {
        if (canDrag)
        {
            transform.position = Vector3.MoveTowards(transform.position, GetMousePos() + _dragOffset, _speed * Time.deltaTime);
        }
    }

    public virtual void OnMouseUp()
    {
        isDragging = false;
    }

    public virtual Vector3 GetMousePos()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
