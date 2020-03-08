using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DifficultyStep
{
    public float BirdSpeedIncrement;
    public int MaxBirdsOnScreenIncrement;
    public float TimeBetweenSpawns;
    public int MinBirdSpawnCount;
    public int MaxBirdSpawnCount;
    public float TimeToNextStep;
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager => FindObjectOfType<GameManager>();

    public int Score { get => _score; set { _score = value; _scoreText.text = _score.ToString(); _gameOverScoreText.text = _score.ToString(); } }

    public int CherryCount { get; set; }

    public bool IsNight { get; set; }
    public float PhaseTimer { get; set; } = 1f;
    public float DayTime => _dayTime;
    public float NightTime => _nightTime;

    [SerializeField] private GameObject _inGameUi;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _multiplierText;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Text _gameOverScoreText;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private float _dayTime;
    [SerializeField] private float _nightTime;
    [SerializeField] private float _hitsPerMultiplierIncrement;

    [SerializeField] private DifficultyStep[] _difficultySteps;
    private int _difficultyStepIndex = 0;
    private float _difficultyStepTimer;

    [SerializeField] private BirdSpawner _birdSpawner;
    [SerializeField] private float _speedBoost;
    [SerializeField] private int _maxBirdsOnScreen;

    private float _timeSinceLastSpawn = 0;

    private DifficultyStep _currentDifficultyStep { get { return _difficultySteps[_difficultyStepIndex]; } }

    private int _score;
    private int _scoreMultiplier = 1;
    private int _birdHitCounter = 0;

    void Start()
    {
        _multiplierText.text = "";
        _difficultyStepTimer = _currentDifficultyStep.TimeToNextStep;
        Score = 0;
    }

    void Update()
    {
        if (CherryCount == 0)
        {
            _gameOverScreen.SetActive(true);
            _inGameUi.SetActive(false);

            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (_pauseScreen.activeInHierarchy)
        {
            return;
        }

        _difficultyStepTimer -= Time.deltaTime;
        if (_difficultyStepTimer <= 0)
        {
            DifficultyUp();
        }

        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn > _currentDifficultyStep.TimeBetweenSpawns)
        {
            var birdsOnScreen = FindObjectsOfType<BirdMovement>().Length;
            while (_timeSinceLastSpawn > _currentDifficultyStep.TimeBetweenSpawns)
            {
                var count = Random.Range(_currentDifficultyStep.MinBirdSpawnCount, _currentDifficultyStep.MaxBirdSpawnCount + 1);
                count = Mathf.Min(count, _maxBirdsOnScreen - birdsOnScreen);
                for (var i = 0; i < count; i++)
                {
                    birdsOnScreen++;
                    var bird = _birdSpawner.SpawnBird();
                    bird.Speed += _speedBoost;
                }

                _timeSinceLastSpawn -= _currentDifficultyStep.TimeBetweenSpawns;
            }
        }

        PhaseTimer += Time.deltaTime;
        PhaseTimer %= _dayTime + _nightTime;

        IsNight = PhaseTimer > _dayTime;

        var mm = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60).ToString().PadLeft(2, '0');
        var ss = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60).ToString().PadLeft(2, '0');

        _timeText.text = mm + ":" + ss;
    }

    private void DifficultyUp()
    {
        _difficultyStepIndex = Mathf.Min(_difficultySteps.Length - 1, _difficultyStepIndex + 1);
        _difficultyStepTimer = _currentDifficultyStep.TimeToNextStep;
        _maxBirdsOnScreen += _currentDifficultyStep.MaxBirdsOnScreenIncrement;
        _speedBoost += _currentDifficultyStep.BirdSpeedIncrement;
    }

    public void RestartGame()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Pause()
    {
        _pauseScreen.SetActive(true);
        _inGameUi.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        _pauseScreen.SetActive(false);
        _inGameUi.SetActive(true);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
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
