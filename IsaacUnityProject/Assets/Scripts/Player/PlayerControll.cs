using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(PlayerModel))]
public class PlayerControll : MonoBehaviour
{
    private PlayerModel _pm;
    private InputAction _moveAction;
    private InputAction _attackAction;

    private void Awake()
    {
        _moveAction = InputSystem.actions["Move"];
        _attackAction = InputSystem.actions["Attack"];
        _pm = GetComponent<PlayerModel>();
        
    }

    private void OnEnable()
    {
        _moveAction.performed += (ctx) => _pm.SetPlayerMoveValue(ctx.ReadValue<Vector2>());
        _moveAction.canceled += ctx => _pm.SetPlayerMoveValue(Vector2.zero);
        _attackAction.performed += ctx => _pm.SetPlayerAttackValue(ctx.ReadValue<Vector2>());
        _attackAction.canceled += ctx => _pm.SetPlayerAttackValue(Vector2.zero);
    }
    private void OnDisable()
    {
        _moveAction.performed -= (ctx) => _pm.SetPlayerMoveValue(ctx.ReadValue<Vector2>());
        _moveAction.canceled -= ctx => _pm.SetPlayerMoveValue( Vector2.zero);
        _attackAction.performed -= ctx => _pm.SetPlayerAttackValue(ctx.ReadValue<Vector2>());
        _attackAction.canceled -= ctx => _pm.SetPlayerAttackValue(Vector2.zero);
    }
}
