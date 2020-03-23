using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntPowerUpEffect : MonoBehaviour
{
    [SerializeField] private float _effectTime;
    [SerializeField] private int _scoreOnBirdKill;
    [SerializeField] private string _achievement;
    [SerializeField] private Vector2[] _moveToClosest;

    private Animator _animator;
    private Collider2D _collider;

    private int _birds = 0;



    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        StartCoroutine(WaitForEnd());

        if (_moveToClosest != null && _moveToClosest.Length > 0)
        {
            var closest = (Vector2)transform.position;
            var closestDist = Mathf.Infinity;
            foreach (var v in _moveToClosest)
            {
                var dist = Vector2.Distance(transform.position, v);
                if (Vector2.Distance(transform.position, v) < closestDist)
                {
                    closestDist = dist;
                    closest = v;
                }
            }

            transform.position = closest;
            transform.rotation = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var birds = new List<Collider2D>();
        Physics2D.OverlapCollider(_collider, new ContactFilter2D() { layerMask = LayerMask.GetMask("Birds") }, birds);
        foreach (var bird in birds)
        {
            var bs = bird.GetComponent<BirdShooting>();
            if (bs != null && bs.Enabled)
            {
                bs.Die(_scoreOnBirdKill);
                _birds++;
            }
        }
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(_effectTime);
        _animator.SetTrigger("End");
    }

    public void End()
    {
        Achievements.achievements.Set(_achievement, _birds);
        Destroy(gameObject);
    }
}
