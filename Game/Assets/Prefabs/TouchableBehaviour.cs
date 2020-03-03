using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TouchableBehaviour : MonoBehaviour
{
    public bool Enabled { get; set; } = true;
    public TouchPhase ReactToTouchPhase { get; set; } = TouchPhase.Began;
    protected abstract void OnTouched();

    private Collider2D _collider;

    protected virtual void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (!Enabled)
        {
            return;
        }

        HandleTouch();
        HandleMouse();
    }

    void HandleTouch()
    {
        for (var i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);

            if(touch.phase == ReactToTouchPhase)
            {
                var position = Camera.main.ScreenToWorldPoint(touch.position);
                if(Physics2D.OverlapPoint(position) == _collider)
                {
                    OnTouched();
                }
            }
        }
    }

    void HandleMouse()
    {
    }
}
