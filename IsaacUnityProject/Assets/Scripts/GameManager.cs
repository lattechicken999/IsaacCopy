using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player { private set; get; }
    private CinemachineConfiner2D _playerCineConfinder;

    public void SetPlayer(GameObject player)
    {
        Player = player;
        _playerCineConfinder = Player.transform.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
    }
    public void TeleportPlayer(Vector2 position, PolygonCollider2D bounder)
    {
        Player.transform.position = position;
        _playerCineConfinder.m_BoundingShape2D = bounder;
    }
}
