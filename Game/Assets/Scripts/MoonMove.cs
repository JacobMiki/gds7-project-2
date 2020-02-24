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
        angle -= baseSpeed * Time.deltaTime;

        if (angle < 0)
        {
            angle += 360;
        }

        if (angle > 180)
        {
            angle -= lowerHalfSpeedMultiplier * baseSpeed * Time.deltaTime;

        }

        var sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        var cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        var x = center.x + cos * radius.x;
        var y = center.y + Mathf.Sign(sin) * Mathf.Pow(Mathf.Abs(sin), 0.5f) * radius.y;

        var r = 1.3f * Mathf.Sqrt((sin + 1f) / 2f);
        var g = 1f * Mathf.Sqrt((sin + 1f) / 2f);
        var b = 1f * Mathf.Sqrt((sin + 1f) / 2f);

        _gameManager.GlobalSpriteShade = new Color(Mathf.Clamp(r, 0.3f, 1f), Mathf.Clamp(g, 0.3f, 1f), Mathf.Clamp(b, 0.5f, 1f));

        this.transform.position = new Vector2(x, y);
    }
}
