using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MonsterView : MonoBehaviour
{

    private Animator _ani;
    private BoxCollider2D _coll;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        _ani.SetFloat("Posx", dir.x);
        _ani.SetFloat("Posy", dir.y);
    }
    public void SetSpeed(float speed)
    {
        _ani.SetFloat("Speed", speed);
    }
    public void SetStatus(MonsterState status)
    {
        _ani.SetInteger("Status", (int)status);
        if(status == MonsterState.Die)
        {
            _coll.enabled = false;
        }
    }

}
