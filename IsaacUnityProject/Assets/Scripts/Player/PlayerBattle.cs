using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private PlayerModel _pm;

    private float _invincibility =0;
    private bool _attacked;

    
    private void Update()
    {
        if (_attacked)
            _invincibility += Time.deltaTime;
        if (_invincibility > 1f)
        {
            _attacked = false;
            _invincibility = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_attacked && collision.gameObject.CompareTag("Monster"))
        {
            _attacked = true;
            _pm.TakeDamage(collision.gameObject.GetComponent<MonsterModel>().AttackPoint);
        }
    }

}
