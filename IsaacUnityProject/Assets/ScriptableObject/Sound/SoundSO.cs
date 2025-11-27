
using UnityEngine;
[CreateAssetMenu(fileName ="SoundSO",menuName = "ScripableObject/SoundSO")]
public class SoundSO : ScriptableObject
{
    [Header("SoundClip")]
    public AudioClip BGM;
    public AudioClip Button;
    public AudioClip Tears;
    public AudioClip BugDie;
}
