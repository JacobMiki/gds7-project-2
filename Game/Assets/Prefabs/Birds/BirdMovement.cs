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

    void Start()
    {
        _curveTimeShift = Random.value;
        _scale = transform.localScale;
    }

    void Update()
    {
        if (Dead)
        {
            FallDown();
            return;
        }

        if (Target != null)
        {
            MoveToTarget();
        }

        MoveOnCurve();
    }

    private void FallDown()
    {
        var movementVector = Vector2.down;

        transform.position += (Vector3)movementVector * 2 * _speed * Time.deltaTime;
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
        var yShift = _curve.Evaluate(_curveTimeShift + Time.realtimeSinceStartup);

        transform.position += new Vector3(0, yShift * _curveAmplitude);
    }
}
