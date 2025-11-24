using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveControll : MonoBehaviour
{
    private InputAction _moveAction;
    private Vector2 _moveDir;
    private Rigidbody2D _rig;
    private float _speed = 5f;
    private void Awake()
    {
        _rig = GetComponent<Rigidbody2D>();
        _moveAction = InputSystem.actions["Move"];
    }

    private void OnEnable()
    {
        _moveAction.performed += (ctx) => _moveDir = ctx.ReadValue<Vector2>();
        _moveAction.canceled += ctx => _moveDir = Vector2.zero;
    }
    private void OnDisable()
    {
        _moveAction.performed -= (ctx) => _moveDir = ctx.ReadValue<Vector2>();
        _moveAction.canceled -= ctx => _moveDir = Vector2.zero;
    }

    private void FixedUpdate()
    {
        OnMove();
    }
    private void OnMove()
    {
        if (_moveDir == Vector2.zero) return;
        _rig.MovePosition(_rig.position + (_moveDir.normalized*Time.fixedDeltaTime * _speed));
    }

    public void SetSpeed(float sp)
    {
        _speed = sp;
    }
}
