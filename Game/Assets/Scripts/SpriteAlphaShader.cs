using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAlphaShader : MonoBehaviour
{
    GameManager _gameManager;
    SpriteRenderer _spriteRenderer;
    Color _originalColor;

    public float Influence = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.gameManager;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.color = _originalColor * (1 - _gameManager.GlobalSpriteShade.b) * 2;
    }
}
