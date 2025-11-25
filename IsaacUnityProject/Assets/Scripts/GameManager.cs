using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player { private set; get; }
    private CinemachineConfiner2D _playerCineConfinder;
    private Rigidbody2D _rig;

    public void SetPlayer(GameObject player)
    {
        Player = player;
        _playerCineConfinder = Player.transform.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
        _rig = Player.GetComponent<Rigidbody2D>();
    }
    public void TeleportPlayer(Vector2 position, PolygonCollider2D bounder)
    {
        _rig.position = position;
        _rig.velocity = Vector2.zero;
        _playerCineConfinder.m_BoundingShape2D = bounder;
    }
}
