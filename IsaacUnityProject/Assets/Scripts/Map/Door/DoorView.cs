using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorView : MonoBehaviour,IDoorStatus
{
    private Animator _ani;
    private void Awake()
    {
        _ani = GetComponent<Animator>();
    }

    public void SetDoorStatus(DoorStatus doorStatus)
    {
        _ani.SetInteger("Status", (int)doorStatus);
    }
}
