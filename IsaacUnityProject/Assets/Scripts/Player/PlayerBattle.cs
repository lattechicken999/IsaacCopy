using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    [SerializeField] private PlayerModel _pm;

    private float _invincibility =0;
    private bool _attacked;
    private Rigidbody2D _parentRig;

    private void Start()
    {
        _parentRig = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }
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
            //피격시에는 움직임 정지
            _pm.SetPlayerMoveValue(Vector2.zero);
            _pm.TakeDamage(collision.gameObject.GetComponent<MonsterModel>().AttackPoint);
            _parentRig.AddForce((transform.position - collision.transform.position).normalized * 10 , ForceMode2D.Impulse);
        }
    }

}
