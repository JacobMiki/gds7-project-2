using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShooting : TouchableBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _powerUpEffect;
    [SerializeField] protected AudioSource _shotAudio;

    public override void OnTouched()
    {
        Enabled = false;
        _animator.SetTrigger("Explode");
        TriggerEffect();
        Achievements.achievements.Add("POWERUPS", 1);
        _shotAudio?.PlayOneShot(_shotAudio.clip);
    }

    public void OnExplodeAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void TriggerEffect()
    {
        Instantiate(_powerUpEffect, transform.position, transform.rotation);
        GetComponent<PowerUpMovement>().enabled = false;
    }

    public void CameraShake()
    {
        GameManager.gameManager.CameraShake(0.2f, 3f, 4);
    }
}
