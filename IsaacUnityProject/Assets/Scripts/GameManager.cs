using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player { private set; get; }

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
}
