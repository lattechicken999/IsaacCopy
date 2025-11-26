using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class MonsterController : MonoBehaviour
{

    public void TakenDamage(float damage)
    {
        _monsterModel.TakeDamage(damage);
        if (_monsterModel.HP <= 0)
        {
            _state = MonsterState.Die;
            _monsterView.SetStatus(_state);
        }
           
    }

    private void StopAction()
    {
        if (_searchCoroutine != null)
            StopCoroutine(_searchCoroutine);
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);
        _moveFlag = false;
    }

    private void StartAction()
    {
        if (_searchCoroutine == null)
            _searchCoroutine = StartCoroutine(DoAttack());
        if (_attackCoroutine == null)
            _attackCoroutine = StartCoroutine(AttackCoolTime());

        if (_monsterModel._isBoos)
        {
            if (_resultPath != null)
            {
                if (_moveFlag)
                    OnMove();
            }
        }
        else
        {
            OnMoveRandom();
        }
        _monsterView.SetSpeed(_monsterModel.Speed);
        
    }
}

