using UnityEngine;
public class MonsterModel : MonoBehaviour
{
    [SerializeField] MonsterScriptableObj _monsterSo;
    private float _speed;
    private float _hp;
    private float _att;
    public bool _isBoos;

    private void Start()
    {
        _speed = _monsterSo.speed;
        _hp = _monsterSo.hp;
        _att = _monsterSo.att;
        _isBoos = false;
    }
    public float Speed 
    { 
        get { return _speed; } 
    }
    public float AttackPoint
    {
        get { return _att; }
    }
    public float HP
    {
        get { return _hp; }
    }
    public void SetBoss()
    {
        _isBoos = true;
        _hp = _monsterSo.hp * 10;
        _att = _monsterSo.att * 1.8f;
        _speed = _monsterSo.speed * 0.8f;
    }
    public void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp < 0) _hp = 0;
    }
    public void HealHp(float heal)
    {
        _hp += heal;
        if (_hp > _monsterSo.hp) _hp = _monsterSo.hp;
    }
    public void ResetHp()
    {
        _hp = _monsterSo.hp;
    }
}
