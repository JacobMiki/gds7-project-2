using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonMove : MonoBehaviour
{
    public Vector2 radius;
    public Vector2 center;

    public float angle;
    public float baseSpeed;
    public float lowerHalfSpeedMultiplier;

    GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.gameManager;
    }

    void Update()
    {
        if (!_gameManager.IsNight)
        {
            angle = _gameManager.PhaseTimer / _gameManager.DayTime * 180;
        }
        else
        {
            angle = 180 + (_gameManager.PhaseTimer - _gameManager.DayTime) / _gameManager.NightTime * 180;
        }

        var sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        var cos = -Mathf.Cos(angle * Mathf.Deg2Rad);

        var x = center.x + cos * radius.x;
        var y = center.y + Mathf.Sign(sin) * Mathf.Pow(Mathf.Abs(sin), 0.5f) * radius.y;

        transform.position = new Vector2(x, y);
    }
}
