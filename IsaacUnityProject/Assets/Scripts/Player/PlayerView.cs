using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerView : MonoBehaviour
{
    private Rigidbody2D _rig;
    private Animator _anim;
    private CinemachineConfiner2D _camConfiner;
    private Vector2 _moveDir;
    private Vector2 _attDir;
    private Vector2 _oldMoveDir;
    private float _speed;

    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _camConfiner = transform.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
    }

    private void FixedUpdate()
    {
        OnMove();
        OnAttack();
    }
    private void OnMove()
    {
        SetAnimationMotion(_rig.velocity.magnitude);
        if (_rig.velocity == Vector2.zero && _moveDir == Vector2.zero) return;
        if (_rig.velocity != Vector2.zero && _moveDir == Vector2.zero)
        {
            //부드러운 캐릭터 정지
            _rig.velocity = Vector2.Lerp(_rig.velocity, Vector2.zero, 0.15f);
            SetAnimationDirection(_oldMoveDir);
        }
        else
        {
            _rig.velocity = _moveDir*_speed;
            SetAnimationDirection(_moveDir);
            _oldMoveDir = _moveDir;
        }
        
    }
    private void OnAttack()
    {
        if(_attDir == Vector2.zero) return;
        SetAnimationDirection(_attDir);
    }
    
    private Vector2 GetLookAxis(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) return new Vector2(dir.x, 0);
        else return new Vector2(0, dir.y);
    }
    private void SetAnimationDirection(Vector2 dir)
    {
        Vector2 moveAxis = GetLookAxis(dir);
        _anim.SetFloat("UpDown", moveAxis.y);
        _anim.SetFloat("LeftRight", moveAxis.x);
    }
    private void SetAnimationMotion(float speed)
    {
        _anim.SetFloat("Speed",speed);
    }
    public void MoveCommand(Vector2 moveDir,float speed)
    {
        _moveDir = moveDir;
        _speed = speed * 3;
    }
    public void AttackCommand(Vector2 attDir)
    {
        _attDir = attDir;
    }
    public void SetBounder(PolygonCollider2D bounder)
    {
        _camConfiner.m_BoundingShape2D = bounder;
    }
}
