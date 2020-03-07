using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BirdTargetingEscape
{
    ClosestSide,
    OppositeSide,
    Entrance,
}

enum BirdTargetType
{
    Cherry,
    IntermediateTarget,
    Escape
}

[RequireComponent(typeof(BirdMovement))]
public class BirdTargeting : MonoBehaviour
{
    [SerializeField]
    private BirdTargetingEscape _escapeStrategy;

    [SerializeField]
    private Transform _beakPosition;

    [SerializeField]
    private BirdTargetType[] _targetList;

    [SerializeField]
    private GameObject _intermediateTarget;

    private BirdMovement _birdMovement;
    private Vector3 _initialPosition;
    private int _currentTargetIndex = 0;

    private GameObject _currentIntermediateTarget;

    void Start()
    {
        _initialPosition = transform.position;

        _birdMovement = GetComponent<BirdMovement>();
        _birdMovement.Target = FindNextTarget();

    }

    void Update()
    {
        if (_birdMovement.Dead)
        {
            return;
        }

        if (_birdMovement.Target == null)
        {
            _birdMovement.Target = FindEscape();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _birdMovement.Target)
        {

            if (collision.gameObject.CompareTag("Cherry"))
            {
                collision.gameObject.transform.SetParent(_beakPosition);
                collision.gameObject.transform.localPosition = Vector3.zero;
                collision.gameObject.tag = "Untagged";
            }

            if (_currentIntermediateTarget != null)
            {
                Destroy(_currentIntermediateTarget);
                _currentIntermediateTarget = null;
            }

            _birdMovement.Target = FindNextTarget();
        }
    }

    void OnBecameInvisible()
    {
        if (_currentIntermediateTarget != null)
        {
            Destroy(_currentIntermediateTarget);
            _currentIntermediateTarget = null;
        }
        Destroy(gameObject);
    }

    GameObject FindNextTarget()
    {
        GameObject target = null;

        switch (_targetList[_currentTargetIndex])
        {
            case BirdTargetType.Cherry:
                target = FindCherry();
                break;
            case BirdTargetType.IntermediateTarget:
                target = FindIntermediateTarget();
                break;
            case BirdTargetType.Escape:
                target = FindEscape();
                break;
        }

        _currentTargetIndex++;
        if (_currentTargetIndex >= _targetList.Length)
        {
            _currentTargetIndex = _targetList.Length - 1;
        }

        return target;
    }

    GameObject FindIntermediateTarget()
    {
        GameObject target;

        var halfScreenWidth = Screen.width / 2;
        float x;
        var y = Random.Range(Screen.height * 0.25f, Screen.height);
        if (Camera.main.WorldToScreenPoint(transform.position).x > halfScreenWidth)
        {
            x = Random.Range(0, halfScreenWidth / 2);
        }
        else
        {
            x = Random.Range(halfScreenWidth * 1.5f, Screen.width);
        }

        var screenPos = new Vector3(x, y);
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        target = Instantiate(_intermediateTarget);
        target.transform.position = worldPos;
        _currentIntermediateTarget = target;

        return target;
    }

    GameObject FindCherry()
    {
        var cherries = GameObject.FindGameObjectsWithTag("Cherry");
        if (cherries.Length == 0)
        {
            return null;
        }

        return cherries[Random.Range(0, cherries.Length)];
    }

    GameObject FindEscape()
    {
        var escapes = GameObject.FindGameObjectsWithTag("Spawner");

        GameObject escape = null;

        switch (_escapeStrategy)
        {
            case BirdTargetingEscape.ClosestSide:
                {
                    var minDist = Mathf.Infinity;
                    foreach (var e in escapes)
                    {
                        var dist = Vector3.Distance(e.transform.position, transform.position);
                        if (dist < minDist)
                        {
                            escape = e;
                            minDist = dist;
                        }
                    }
                }
                break;
            case BirdTargetingEscape.Entrance:
                {
                    var minDist = Mathf.Infinity;
                    foreach (var e in escapes)
                    {
                        var dist = Vector3.Distance(e.transform.position, _initialPosition);
                        if (dist < minDist)
                        {
                            escape = e;
                            minDist = dist;
                        }
                    }
                }
                break;
            case BirdTargetingEscape.OppositeSide:
                {
                    var maxDist = 0f;
                    foreach (var e in escapes)
                    {
                        var dist = Vector3.Distance(e.transform.position, _initialPosition);
                        if (dist > maxDist)
                        {
                            escape = e;
                            maxDist = dist;
                        }
                    }
                }
                break;
        }

        var randomSpawnArea = escape.GetComponent<BoxCollider2D>();

        var offset = Random.insideUnitCircle * (randomSpawnArea.size / 2);

        var worldPos = randomSpawnArea.transform.position + new Vector3(offset.x, offset.y, 0f);


        var target = Instantiate(_intermediateTarget);
        target.transform.position = worldPos;
        _currentIntermediateTarget = target;

        return target;
    }
}
