using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private float _curveAmplitude;

    [HideInInspector]
    public GameObject Target { get; set; }

    [HideInInspector]
    public bool Dead { get; set; }

    private float _curveTimeShift;
    private Vector3 _scale;
    private Animator _animator;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }


    void Start()
    {
        _curveTimeShift = Random.value;
        _scale = transform.localScale;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Dead)
        {
            _animator.speed = 0.2f;
            FallDown();
            return;
        }

        _animator.speed = _speed / 4.0f;

        if (Target != null)
        {
            MoveToTarget();
        }

        MoveOnCurve();
    }

    private void FallDown()
    {
        var movementVector = Vector2.down;

        transform.position += (Vector3)movementVector * 20 * Time.deltaTime;

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToTarget()
    {
        var movementVector = Target.transform.position - transform.position;
        movementVector.Normalize();

        transform.localScale = new Vector3(Mathf.Sign(movementVector.x) * _scale.x, _scale.y, _scale.z);

        transform.position += movementVector * _speed * Time.deltaTime;
    }

    private void MoveOnCurve()
    {
        if (Time.timeScale > 0)
        {
            var yShift = _curve.Evaluate(_curveTimeShift + Time.time);

            transform.position += new Vector3(0, yShift * _curveAmplitude);
        }
    }
}
