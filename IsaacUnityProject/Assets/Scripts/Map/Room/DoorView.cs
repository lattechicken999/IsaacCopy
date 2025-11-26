using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(DoorCollisionEvent))]

public class DoorView : MonoBehaviour,IDoorStatus
{
    private Animator _ani;
    private BoxCollider2D _collider;
    private void Awake()
    {
        _ani = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void SetDoorStatus(DoorStatus doorStatus)
    {
        _ani.SetInteger("Status", (int)doorStatus);
        _collider.isTrigger = doorStatus == DoorStatus.Open;
    }
}

