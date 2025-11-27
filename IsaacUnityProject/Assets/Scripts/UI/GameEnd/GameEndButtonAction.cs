using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameEndButtonAction : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _audioSource.clip = SoundManager.Instance.GetSoundClip(SoundEnum.Button);
        _audioSource.maxDistance = 4000;
        _audioSource.minDistance = 0;
    }
    public void GameEnd()
    {
        _audioSource.Play();
        GameManager.Instance.UpdateGameState(GameState.Menu);
    }
}
