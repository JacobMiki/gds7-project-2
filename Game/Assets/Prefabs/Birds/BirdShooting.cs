using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdMovement))]
public class BirdShooting : TouchableBehaviour
{
    [SerializeField]
    private int _score;

    private BirdMovement _birdMovement;
    private GameManager _gameManager;

    protected override void Start()
    {
        base.Start();
        _birdMovement = GetComponent<BirdMovement>();
        _gameManager = GameManager.gameManager;
    }

    protected override void OnTouched()
    {
        Enabled = false;
        var scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
        _birdMovement.Dead = true;
        _gameManager.Score += _score;
    }
}
