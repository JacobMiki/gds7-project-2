using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBirdFly : MonoBehaviour
{
    public float Speed;
    public float GrabCherryRange;
    public Transform CarryCherryPoint;
    public float BobSpeedFactor = 0.5f;
    public float BobAmplitudeFactor = 0.02f;
    private Vector3 _moveDirection;
    private GameObject _targetCherry;
    private Collider2D _collider;
    private bool _dead = false;
    private float _randomBobShift;
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _gameManager = GameManager.gameManager;

        var cherries = GameObject.FindGameObjectsWithTag("Cherry");

        _targetCherry = cherries.Length > 0 ? cherries[Random.Range(0, cherries.Length)] : null;

        if (_targetCherry != null)
        {
            _moveDirection = Vector3.Normalize(_targetCherry.transform.position - this.transform.position);
            transform.localScale = _moveDirection.x < 0 ? new Vector3(-this.transform.localScale.x, this.transform.localScale.y, 1f) : this.transform.localScale;

            _randomBobShift = Random.value * 2 * Mathf.PI;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var moveVector = _moveDirection * Speed * Time.deltaTime;

        if (_dead)
        {
            this.transform.position += moveVector;
            return;
        }

        if (CheckForTouch())
        {
            _dead = true;
            _moveDirection = new Vector3(0f, -1f, 0f);
            _targetCherry = null;
            Speed = 12f;
            this.transform.localScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, 1f);

            _gameManager.Score += 100;
        }

        if (_targetCherry != null)
        {
            var cherryDistance = Vector3.Distance(this.transform.position, _targetCherry.transform.position);
            moveVector *= Mathf.Lerp(0.25f, 1f, cherryDistance / (3 * GrabCherryRange));
            if (cherryDistance < GrabCherryRange)
            {
                _targetCherry.transform.SetParent(CarryCherryPoint);
                _targetCherry.transform.position = CarryCherryPoint.position;
                _targetCherry.tag = "Untagged";
                _targetCherry = null;
            }
        }


        var bobVector = Mathf.Sin(Time.time * Mathf.PI * Speed * BobSpeedFactor + _randomBobShift) * new Vector3(0, BobAmplitudeFactor, 0);

        this.transform.position += moveVector + bobVector;

        if (this.transform.position.y <= -3.5f)
        {
            _moveDirection = new Vector3(_moveDirection.x, -_moveDirection.y, _moveDirection.z);
        }


    }

    bool CheckForTouch()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    var touchPosition = new Vector2(wp.x, wp.y);

                    if (_collider == Physics2D.OverlapCircle(touchPosition, 0.25f))
                    {
                        return true;
                    }
                }
            }


        }
        return false;
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject, 2f);
    }
}
