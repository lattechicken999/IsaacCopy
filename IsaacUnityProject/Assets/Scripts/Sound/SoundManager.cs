using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEnum
{
    BGM, Button, Tears, BugDie,_End
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] SoundSO _so;

    public AudioClip GetSoundClip(SoundEnum se)
    {
        switch(se)
        {
            case SoundEnum.BGM:
                return _so.BGM;
            case SoundEnum.Button:
                return _so.Button;
            case SoundEnum.Tears:
                return _so.Tears;
            case SoundEnum.BugDie:
                return _so.BugDie;
        }
        return null;
    }
}
