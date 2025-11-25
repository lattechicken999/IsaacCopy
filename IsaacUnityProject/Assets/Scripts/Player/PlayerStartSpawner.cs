using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartSpawner : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefeb;
    private PolygonCollider2D _bounder;
    private void Awake()
    {
        _bounder = transform.parent.GetComponent<PolygonCollider2D>();
    }
    private void Start()
    {
        GameManager.Instance.SetPlayer(Instantiate(_playerPrefeb));
        GameManager.Instance.Player.transform.position = transform.position;
        GameManager.Instance.Player.GetComponent<PlayerView>().SetBounder(_bounder);
    }
}
