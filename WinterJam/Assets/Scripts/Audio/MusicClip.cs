using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public struct MusicClip
{
    public AudioClip Clip;

    public EMusicType MusicType;

    [Tooltip("Tick to not loop")]
    public bool DoNotLoop;

    [Tooltip("Only used if type is Level")]
    public int LevelNumber;
}