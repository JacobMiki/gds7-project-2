using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAlphaShader : MonoBehaviour
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
        _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _colorController.Color.a);
    }
}
