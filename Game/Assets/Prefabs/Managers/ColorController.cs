using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public static ColorController colorContoller => FindObjectOfType<ColorController>();

    [SerializeField]
    private AnimationCurve _redCurve;

    [SerializeField]
    private AnimationCurve _greenCurve;

    [SerializeField]
    private AnimationCurve _blueCurve;

    [SerializeField]
    private AnimationCurve _alphaCurve;

    public Color Color { get; set; }

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameManager.gameManager;
    }

    void Update()
    {
        float t;
        if (_gameManager.PhaseTimer > _gameManager.DayTime)
        {
            t = 0.5f + ((_gameManager.PhaseTimer - _gameManager.DayTime) / _gameManager.NightTime) * 0.5f;
        }
        else
        {
            t = (_gameManager.PhaseTimer / _gameManager.DayTime) * 0.5f;
        }

        var r = _redCurve.Evaluate(t);
        var g = _greenCurve.Evaluate(t);
        var b = _blueCurve.Evaluate(t);
        var a = _alphaCurve.Evaluate(t);

        Color = new Color(r, g, b, a);
    }
}
