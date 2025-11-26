using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="MonsterSO",menuName ="ScripableObject/MonsterSO")]
public class MonsterScriptableObj : ScriptableObject
{
    [Header("Status")]
    public float speed = 3;
    public int hp = 5;
    public float att = 1;

}
