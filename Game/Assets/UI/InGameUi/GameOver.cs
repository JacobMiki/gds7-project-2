using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Scores _scores;
    [SerializeField] private InputField _input;
    [SerializeField] private GameObject _newRecord;

    private string _name = "";
    private int _score;

    void OnEnable()
    {
        if (_scores.IsHiScore(GameManager.gameManager.Score))
        {
            _score = GameManager.gameManager.Score;
            if (PlayerPrefs.HasKey("player_name"))
            {
                _input.text = PlayerPrefs.GetString("player_name");
            }

            ShowDialog();
        }
        else
        {
            _newRecord?.SetActive(false);
            _input?.gameObject.SetActive(false);
        }
    }

    public void OnTextChange(string text)
    {
        _name = text;
    }

    public void OnTextResult(string text)
    {
        _scores.Add(text, _score);
        PlayerPrefs.SetString("player_name", text);
        PlayerPrefs.Save();
    }

    public void ShowDialog()
    {
        _newRecord?.SetActive(true);
        _input?.gameObject.SetActive(true);
        _input.ActivateInputField();
    }

    public void OnDestroy()
    {
        if (!string.IsNullOrEmpty(_name))
        {
            OnTextResult(_name);
        }
    }
}
