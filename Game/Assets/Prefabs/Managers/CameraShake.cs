using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _cameraPos;
    private Quaternion _cameraRot;
    private float _cameraSize;

    private bool _isShaking;
    private float _intensity;
    private float _speed;
    private int _shakes;

    private Vector3 _nextShakePosition;
    private Quaternion _nextShakeRot;
    private float _nextShakeSize;

    [SerializeField] private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _cameraPos = _camera.transform.localPosition;
        _cameraRot = _camera.transform.localRotation;
        _cameraSize = _camera.orthographicSize;
    }

    public void Shake(float intensity, float speed, int count)
    {
        _intensity = intensity;
        _speed = speed;
        _shakes = count;
        NextShakePos();
        _isShaking = true;
    }

    void NextShakePos()
    {
        _nextShakePosition = new Vector3(
            _cameraPos.x + Random.Range(-_intensity, _intensity),
            _cameraPos.y + Random.Range(-_intensity, _intensity),
            _cameraPos.z
            );

        _nextShakeSize = _cameraSize - Random.Range(0, _intensity);
        var euler = _cameraRot.eulerAngles;
        _nextShakeRot = Quaternion.Euler(euler.x, euler.y, euler.z + (Random.Range(-_intensity, _intensity) * 5));
    }

    // Update is called once per frame
    void Update()
    {
        if (_isShaking)
        {
            _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, _nextShakePosition, _speed * Time.deltaTime);
            _camera.transform.localRotation = Quaternion.RotateTowards(_camera.transform.localRotation, _nextShakeRot, _speed * 5 * Time.deltaTime);
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _nextShakeSize, _speed * Time.deltaTime);

            if (Vector3.Distance(_camera.transform.localPosition, _nextShakePosition) < _intensity / 5f
                && Quaternion.Angle(_camera.transform.localRotation, _nextShakeRot) < _intensity / 1f
                && _camera.orthographicSize - _nextShakeSize < _intensity / 5f)
            {
                _shakes--;

                if (_shakes <= 0)
                {
                    _isShaking = false;
                    _camera.transform.localPosition = _cameraPos;
                    _camera.transform.localRotation = _cameraRot;
                    _camera.orthographicSize = _cameraSize;

                }
                else if (_shakes <= 1)
                {
                    _nextShakePosition = _cameraPos;
                    _nextShakeRot = _cameraRot;
                    _nextShakeSize = _cameraSize;
                }
                else
                {
                    NextShakePos();
                }
            }
        }
    }
}
