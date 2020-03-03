using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = Random.value > 0.5f;
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _gameManager.CherryCount++;
    }

    void OnDestroy()
    {
        _gameManager.CherryCount--;
    }

}
