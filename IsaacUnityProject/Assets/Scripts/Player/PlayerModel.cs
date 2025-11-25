using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct sStatus
{
    public float _damage;
    public float _tears;
    public float _range;
    public float _shotSpeed;
    public float _speed;
    public int _heart;
    public int _soulHeart;

}
[RequireComponent(typeof(PlayerView))]
public class PlayerModel : MonoBehaviour
{
    [SerializeField] PlayerScriptableObj _playerSO;
    [SerializeField] TearsSpawner _ts;
    private PlayerView _pv;
    private sStatus _status;

    List<IStatus> _subscribers;
    private void Awake()
    {
        _pv = GetComponent<PlayerView>();

        _status._damage = _playerSO.Damage;
        _status._tears = _playerSO.Tears;
        _status._range = _playerSO.Range;
        _status._shotSpeed = _playerSO.ShotSpeed;
        _status._speed = _playerSO.Speed;
        _status._heart = _playerSO.Heart;
        _status._soulHeart = _playerSO.SoulHeart;

        _subscribers = new List<IStatus>();
    }

    public void RegistSubscripber(IStatus sub)
    {
        if (_subscribers.Contains(sub)) return;
        _subscribers.Add(sub);

        sub.NotifyStatus(_status);
    }
    public void UnregistSubscriber(IStatus sub)
    {
        if (_subscribers.Contains(sub)) _subscribers.Remove(sub);
    }
    public void SetPlayerMoveValue(Vector2 m)
    {
        _pv.MoveCommand(m, _status._speed);
    }
    public void SetPlayerAttackValue(Vector2 att)
    {
        _pv.AttackCommand(att);
        _ts.SetAttackTrigger(att);
    }

}
