using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimePowerUpEffect : MonoBehaviour
{
    [SerializeField] private float _timeScale;
    [SerializeField] private float _timeInEffect;
    [SerializeField] private float _disableSmoothTime;
    [SerializeField] private SpriteRenderer _tint;

    private GameManager _gameManager;
    private float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.gameManager;
        SetTimeScale(_timeScale);
        SetTintAlpha(0f);
    }

    private void Update()
    {
        if (_gameManager.Paused)
        {
            return;
        }

        timePassed += Time.unscaledDeltaTime;
        if (timePassed >= _timeInEffect)
        {
            SetTimeScale(Mathf.Lerp(_timeScale, 1.0f, (timePassed - _timeInEffect) / _disableSmoothTime));
            SetTintAlpha(Mathf.Lerp(_tint.color.a, 0f, (timePassed - _timeInEffect) / _disableSmoothTime));
            if (timePassed - _timeInEffect >= _disableSmoothTime)
            {
                Disable();
            }
        }
        else
        {
            if (_tint.color.a < 0.3f)
            {
                SetTintAlpha(_tint.color.a + timePassed / 2);
            }
        }
    }

    void SetTimeScale(float scale)
    {
        _gameManager.TimeScale = scale;
        Time.timeScale = scale;
    }

    void SetTintAlpha(float a)
    {
        _tint.color = new Color(_tint.color.r, _tint.color.g, _tint.color.b, a);
    }

    void Disable()
    {
        SetTimeScale(1.0f);
        Destroy(gameObject);
    }
}
