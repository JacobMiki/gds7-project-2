using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _animator;

    [SerializeField]
    private float _secondsBetweenBlinks = 1f;
    [SerializeField]
    private float _blinkChance = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = Random.value > 0.5f;
        _gameManager = FindObjectOfType<GameManager>();
        _animator = GetComponent<Animator>();

        _gameManager.CherryCount++;

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            _animator.ResetTrigger("Blink");
            _animator.ResetTrigger("SideBlink");


            if (Random.value < _blinkChance)
            {
                if (Random.value > 0.5f)
                {
                    _animator.SetTrigger("Blink");

                }
                else
                {
                    _animator.SetTrigger("SideBlink");

                }
            }

            yield return new WaitForSeconds(_secondsBetweenBlinks);
        }
    }

    void OnDestroy()
    {
        _gameManager.CherryCount--;
    }

}
