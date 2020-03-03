using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShader : MonoBehaviour
{
    ColorController _colorController;
    SpriteRenderer _spriteRenderer;
    Color _originalColor;

    public float Influence = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _colorController = ColorController.colorContoller;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        var c = Color.Lerp(_originalColor, _colorController.Color, Influence);
        _spriteRenderer.color = new Color(c.r, c.g, c.b, _originalColor.a);
    }
}
