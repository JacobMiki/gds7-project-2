using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BirdMovement))]
public class BirdShooting : TouchableBehaviour
{
    [SerializeField]
    private int _score;

    private BirdMovement _birdMovement;
    private GameManager _gameManager;
    [SerializeField] protected AudioSource _shotAudio;

    private void Start()
    {
        _birdMovement = GetComponent<BirdMovement>();
        _gameManager = GameManager.gameManager;
    }

    public override void OnTouched()
    {
        if(Time.timeScale == 0f)
        {
            return;
        }

        _gameManager.BirdHit(_score, true);
        Die(0);
        _shotAudio?.PlayOneShot(_shotAudio.clip);

    }

    public void Die(int score)
    {
        if (_birdMovement != null && !_birdMovement.Dead)
        {
            Enabled = false;
            var scale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
            _birdMovement.Dead = true;
            if (score > 0)
            {
                _shotAudio?.PlayOneShot(_shotAudio.clip);

                _gameManager.BirdHit(score);
            };
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
