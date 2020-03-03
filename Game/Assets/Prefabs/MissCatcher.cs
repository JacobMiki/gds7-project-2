using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissCatcher : TouchableBehaviour
{
    GameManager _gameManager;

    protected override void Start()
    {
        base.Start();
        _gameManager = GameManager.gameManager;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTouched()
    {
        _gameManager.Miss();
    }
}
