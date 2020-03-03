using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager => FindObjectOfType<GameManager>();

    public int Score { get => _score; set { _score = value; ScoreText.text = _score.ToString(); } }

    public int CherryCount { get; set; }

    public Color GlobalSpriteShade { get; set; }

    public Text ScoreText;
    public Text TimeText;
    public GameObject GameOverText;

    private int _score;
    private float _gameOverTimer;

    void Start()
    {
        Input.backButtonLeavesApp = true;
        GlobalSpriteShade = Color.white;

    }

    void Update()
    {
        if (CherryCount == 0)
        {
            GameOverText.SetActive(true);
            _gameOverTimer += Time.deltaTime;

            if (_gameOverTimer >= 5f)
            {
                var scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }

            return;
        }

        var mm = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60).ToString().PadLeft(2, '0');
        var ss = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60).ToString().PadLeft(2, '0');

        TimeText.text = mm + ":" + ss;


    }


}
