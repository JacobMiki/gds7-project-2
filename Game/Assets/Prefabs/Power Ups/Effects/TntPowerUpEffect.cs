using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TntPowerUpEffect : MonoBehaviour
{
    [SerializeField] private float _effectTime;
    [SerializeField] private float _effectRadius;
    [SerializeField] private int _scoreOnBirdKill;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(WaitForEnd());
        transform.localScale = Vector2.one * (_effectRadius / 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        var birds = Physics2D.OverlapCircleAll(transform.position, _effectRadius, LayerMask.GetMask("Birds"));

        foreach (var bird in birds)
        {
            var bs = bird.GetComponent<BirdShooting>();
            if (bs.Enabled)
            {
                bs.Die(_scoreOnBirdKill);
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
        Destroy(gameObject);
    }
}
