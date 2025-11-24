using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator _ani;
    private void Awake()
    {
        _ani = GetComponent<Animator>();
    }
    public void ChageStatue(DoorStatus stat)
    {
        _ani.SetInteger("Status", (int)stat);
    }
}
