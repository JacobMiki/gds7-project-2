using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissCatcher : TouchableBehaviour
{
    GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.gameManager;
    }

    public override void OnTouched()
    {
        _gameManager.Miss();
    }
}
