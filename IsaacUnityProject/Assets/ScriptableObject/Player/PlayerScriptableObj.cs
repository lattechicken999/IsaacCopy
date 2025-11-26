using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="PlayerSO",menuName ="ScripableObject/PlayerSO")]
public class PlayerScriptableObj : ScriptableObject
{
    [Header("Status")]
    public float Damage = 3.7f;
    public float Tears =2.73f;
    public float Range = 20f;
    public float ShotSpeed = 0.8f;
    public float Speed = 1.4f;
    public int Heart = 3;
    public int SoulHeart = 0;

}
