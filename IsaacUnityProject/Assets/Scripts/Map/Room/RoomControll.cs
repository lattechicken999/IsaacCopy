using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControll : MonoBehaviour
{
    [SerializeField] GameObject _monsterPrefeb;
    private Transform[] _monsterSpawnPoint;
    private GameObject[] _monsterInstances;
    private bool _isChallenging;

    private void Start()
    {
        var monsterSpawner = transform.Find("MonsterSpawner");
        _monsterSpawnPoint = new Transform[monsterSpawner.childCount];
        _monsterInstances = new GameObject[monsterSpawner.childCount];

        for (int i=0;i<_monsterSpawnPoint.Length;i++)
        {
            _monsterSpawnPoint[i] = monsterSpawner.GetChild(i).transform;
        }
        _isChallenging = false;
    }

    private void Update()
    {
        if(_isChallenging)
        {
            foreach(var mon in _monsterInstances)
            {
                if (mon.GetComponent<BoxCollider2D>().enabled) return;
            }
            ClearChallenge();
        }
    }

    public void StartChallenge()
    {
        if (_monsterSpawnPoint == null) return;
        for(int i =0;i< _monsterSpawnPoint.Length;i++)
        {
            if(_monsterInstances[i] == null)
                _monsterInstances[i] = Instantiate(_monsterPrefeb, _monsterSpawnPoint[i]);
            else
            {
                _monsterInstances[i]?.SetActive(true);
            }
        }
        _isChallenging = true;
        RoomManager.Instance.StartChallenge();
    }
    public void StartChallenge(bool isBoss)
    {
        if (_monsterSpawnPoint == null) return;
        for (int i = 0; i < _monsterSpawnPoint.Length; i++)
        {
            if (_monsterInstances[i] == null)
            {
                _monsterInstances[i] = Instantiate(_monsterPrefeb, _monsterSpawnPoint[i]);
                _monsterInstances[i].GetComponent<MonsterModel>().SetBoss();
                _monsterInstances[i].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
            else
            {
                _monsterInstances[i]?.SetActive(true);
                _monsterInstances[i]?.GetComponent<MonsterModel>().SetBoss();
                _monsterInstances[i].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            }
        }
        _isChallenging = true;
        RoomManager.Instance.StartChallenge();
    }
    public void StopChallenge()
    {
        if (_monsterSpawnPoint == null) return;
        for (int i = 0; i < _monsterSpawnPoint.Length; i++)
        {
            _monsterInstances[i]?.SetActive(false);
        }
        _isChallenging = false;
    }
    public void ClearChallenge()
    {
        if (_monsterSpawnPoint == null) return;
        //부자연스러워서 그냥 삭제
        //for (int i = 0; i < _monsterSpawnPoint.Length; i++)
        //{
        //    if(_monsterInstances != null)
        //        Destroy(_monsterInstances[i]);
        //}
        _isChallenging = false;
        RoomManager.Instance.ClearRoom();
    }

}
