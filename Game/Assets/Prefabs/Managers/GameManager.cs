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
    [SerializeField] private Text _multiplierText;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Text _gameOverScoreText;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private Animator _uiAnimator;
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
    private float _unscaledTimeSinceLevelLoad = 0;

    private DifficultyStep _currentDifficultyStep { get { return _difficultySteps[_difficultyStepIndex]; } }

    private int _score;
    private int _scoreMultiplier = 1;
    private int _birdHitCounter = 0;

    public float TimeScale { get; set; } = 1.0f;
    public bool Paused { get; set; } = true;

    [SerializeField] private CameraShake _cameraShake;

    void Start()
    {
        _multiplierText.text = "";
        _difficultyStepTimer = _currentDifficultyStep.TimeToNextStep;
        Score = 0;
        _unscaledTimeSinceLevelLoad = 0;
        Resume();
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

        if (Paused)
        {
            return;
        }

        _difficultyStepTimer -= Time.unscaledDeltaTime;
        if (_difficultyStepTimer <= 0)
        {
            DifficultyUp();
        }

        _unscaledTimeSinceLevelLoad += Time.unscaledDeltaTime;
        _timeSinceLastSpawn += Time.unscaledDeltaTime;
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

        PhaseTimer += Time.unscaledDeltaTime;
        PhaseTimer %= _dayTime + _nightTime;

        IsNight = PhaseTimer > _dayTime;
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
        Resume();
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Pause()
    {
        _pauseScreen.SetActive(true);
        _inGameUi.SetActive(false);
        Paused = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        _pauseScreen.SetActive(false);
        _inGameUi.SetActive(true);
        Paused = false;
        Time.timeScale = TimeScale;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BirdHit(int score, bool shake = false)
    {
        Score += score * _scoreMultiplier;
        _birdHitCounter++;

        if (_birdHitCounter % _hitsPerMultiplierIncrement == 0)
        {
            _scoreMultiplier++;
            _multiplierText.text = $"x{_scoreMultiplier}";
            _uiAnimator.SetTrigger("MultUp");

            if (shake) _cameraShake.Shake(0.15f, 3f, 3);
        }
        else
        {
            if(shake) _cameraShake.Shake(0.15f, 2f, 2);
        }

    }

    public void Miss()
    {
        _scoreMultiplier = 1;
        _birdHitCounter = 0;
        _multiplierText.text = "";
        _cameraShake.Shake(0.1f, 2f, 2);

    }

    public void CameraShake(float intensity, float speed, int count)
    {
        _cameraShake.Shake(intensity, speed, count);
    }
}
