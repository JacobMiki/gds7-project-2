using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Sprite[] _slides;
    [SerializeField] private Image _display;

    private int _currentSlide = 0;
    // Start is called before the first frame update
    void Start()
    {
        _display.sprite = _slides[_currentSlide];
    }

    public void Next()
    {
        _currentSlide = (_currentSlide + 1) % _slides.Length;
        _display.sprite = _slides[_currentSlide];

        PlayerPrefs.SetInt("seen_tutorial", 1);
        PlayerPrefs.Save();
    }

    public void Prev()
    {
        _currentSlide = (_currentSlide - 1) % _slides.Length;
        if (_currentSlide < 0)
        {
            _currentSlide += _slides.Length;
        }
        _display.sprite = _slides[_currentSlide];

        PlayerPrefs.SetInt("seen_tutorial", 1);
        PlayerPrefs.Save();
    }
}
