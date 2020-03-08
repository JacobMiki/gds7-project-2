using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    private RectTransform _rectTransform;

    [SerializeField]
    private float _transitionSpeed = 7f;

    Vector3 _targetPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
       _rectTransform = GetComponent<RectTransform>();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.position = Vector3.Lerp(_rectTransform.position, _targetPosition, _transitionSpeed * Time.deltaTime);   
    }

    public void GoToScoreboard()
    {
        _targetPosition = new Vector3(16, 0, 0);
    }

    public void GoToMainMenu()
    {
        _targetPosition = new Vector3(0, 0, 0);
    }

    public void GoToChallenges()
    {
        _targetPosition = new Vector3(-16, 0, 0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
