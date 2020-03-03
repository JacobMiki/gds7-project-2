using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager => FindObjectOfType<GameManager>();

    public int Score { get => _score; set { _score = value; _scoreText.text = _score.ToString(); } }

    public int CherryCount { get; set; }

    public bool IsNight { get; set; }
    public float PhaseTimer { get; set; }
    public float DayTime => _dayTime;
    public float NightTime => _nightTime;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private Text _multiplierText;

    [SerializeField]
    private GameObject _gameOverText;

    [SerializeField]
    private float _dayTime;

    [SerializeField]
    private float _nightTime;

    [SerializeField]
    private float _hitsPerMultiplierIncrement;

    private int _score;
    private float _gameOverTimer;
    private int _scoreMultiplier = 1;
    private int _birdHitCounter = 0;

    void Start()
    {
        Input.backButtonLeavesApp = true;
        _multiplierText.text = "";
    }

    void Update()
    {
        if (CherryCount == 0)
        {
            _gameOverText.SetActive(true);
            _gameOverTimer += Time.deltaTime;

            if (_gameOverTimer >= 5f)
            {
                RestartGame();
            }

            return;
        }

        PhaseTimer += Time.deltaTime;
        PhaseTimer %= _dayTime + _nightTime;

        IsNight = PhaseTimer > _dayTime;

        var mm = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60).ToString().PadLeft(2, '0');
        var ss = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60).ToString().PadLeft(2, '0');

        _timeText.text = mm + ":" + ss;
    }

    void RestartGame()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void BirdHit(int score)
    {
        Score += score * _scoreMultiplier;
        _birdHitCounter++;

        if (_birdHitCounter % _hitsPerMultiplierIncrement == 0)
        {
            _scoreMultiplier++;
            _multiplierText.text = $"x{_scoreMultiplier}";
        }
    }

    public void Miss()
    {
        _scoreMultiplier = 1;
        _birdHitCounter = 0;
        _multiplierText.text = "";
    }
}
