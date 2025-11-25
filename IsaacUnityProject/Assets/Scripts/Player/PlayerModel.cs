using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerView))]
public class PlayerModel : MonoBehaviour
{
    [SerializeField] PlayerScriptableObj _playerSO;

    private PlayerView _pv;

    //player status
    private float _damage;
    private float _tears;
    private float _range;
    private float _shotSpeed;
    private float _speed;
    private int _heart;
    private int _soulHeart;

    private void Awake()
    {
        _pv = GetComponent<PlayerView>();

        _damage = _playerSO.Damage;
        _tears = _playerSO.Tears;
        _range = _playerSO.Range;
        _shotSpeed = _playerSO.ShotSpeed;
        _speed = _playerSO.Speed;
        _heart = _playerSO.Heart;
        _soulHeart = _playerSO.SoulHeart;

    }

    public void SetPlayerMoveValue(Vector2 m)
    {
        _pv.MoveCommand(m, _speed);
    }
}
