using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomTeleport : MonoBehaviour
{
    private Transform[] _spawns;
    private PolygonCollider2D _bounder;
    private void Start()
    {
        var spawnPoint = transform.Find("SpawnPoint");
        if (spawnPoint == null) return;

        _spawns = new Transform[4];

        //자식 spawn point를 알맞은 위치 인덱스에 넣어줌
        for (int i = 0; i < spawnPoint.childCount; i++)
        {
            var child = spawnPoint.GetChild(i);
            _spawns[(int)Enum.Parse(typeof(DoorDirection), child.name)] = child;
        }
        _bounder = transform.gameObject.GetComponent<PolygonCollider2D>();
    }
    public void Teleport(DoorDirection dir)
    {
        GameManager.Instance.TeleportPlayer(_spawns[(int)dir].position, _bounder);
    }
}
