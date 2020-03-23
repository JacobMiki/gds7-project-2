using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDispatcher : MonoBehaviour
{
    [SerializeField] protected float _touchToleranceRadius;
    [SerializeField] protected AudioSource _shotAudio;

    public TouchPhase ReactToTouchPhase { get; set; } = TouchPhase.Began;

    private GameManager _gameManager;

    protected virtual void Start()
    {
        _gameManager = GameManager.gameManager;
    }

    protected virtual void Update()
    {

        if (_gameManager.Paused)
        {
            return;
        }

        HandleTouch();
        HandleMouse();
    }

    void Touch(Vector2 position)
    {
        var touched = Physics2D.OverlapCircleAll(position, _touchToleranceRadius, LayerMask.GetMask("Birds", "PowerUps", "OtherTouchables"));

        if (touched.Length > 0)
        {
            TouchableBehaviour bird = null;
            TouchableBehaviour powerup = null;
            TouchableBehaviour other = null;

            foreach (var item in touched)
            {
                var layerName = LayerMask.LayerToName(item.gameObject.layer);
                switch (layerName)
                {
                    case "Birds":
                        if (bird == null || item.transform.position.z < bird.transform.position.z)
                        {
                            var b = item.GetComponent<TouchableBehaviour>();
                            if (b.Enabled)
                            {
                                bird = b;
                            }
                        }
                        break;
                    case "PowerUps":
                        if (powerup == null || item.transform.position.z < powerup.transform.position.z)
                        {
                            var b = item.GetComponent<TouchableBehaviour>();
                            if (b.Enabled)
                            {
                                powerup = b;
                            }
                        }
                        break;
                    case "OtherTouchables":
                        if (other == null || item.transform.position.z < other.transform.position.z)
                        {
                            var b = item.GetComponent<TouchableBehaviour>();
                            if (b.Enabled)
                            {
                                other = b;
                            }
                        }
                        break;
                }
            }

            if (bird != null)
            {
                bird.OnTouched();
                return;
            }

            if (powerup != null)
            {
                if (other == null || other.gameObject.transform.position.z > powerup.transform.position.z)
                {
                    powerup.OnTouched();
                    return;
                }
            }

            if (other != null)
            {
                other.OnTouched();
            }
        }
    }

    void HandleTouch()
    {
        for (var i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);

            if (touch.phase == ReactToTouchPhase)
            {
                _shotAudio?.PlayOneShot(_shotAudio.clip);

                var position = Camera.main.ScreenToWorldPoint(touch.position);
                Touch(position);
            }
        }
    }

    void HandleMouse()
    {
    }
}
