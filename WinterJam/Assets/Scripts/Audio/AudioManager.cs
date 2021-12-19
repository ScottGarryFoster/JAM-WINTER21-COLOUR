using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer AudioMixer;

    public AudioSource MusicAudioSource;

    public List<MusicClip> MusicClips;

    public List<DiegeticClip> DiegeticClips;

    private AudioMixerSnapshot snapshotMusic;

    private AudioMixerSnapshot snapshotGame;

    // Start is called before the first frame update
    void Start()
    {
        snapshotMusic = AudioMixer.FindSnapshot("Music Focus");
        snapshotGame = AudioMixer.FindSnapshot("Unpaused");
    }

    public void SetSnapshotGame()
    {
        snapshotGame.TransitionTo(0.5f);
    }

    public void SetSnapshotMusic()
    {
        snapshotMusic.TransitionTo(0.5f);
    }

    public void PlayMusic(EMusicType musicType, int levelNumber = -1)
    {
        MusicClip clip;
        clip = MusicClips.Find(m => m.MusicType == musicType);
        MusicAudioSource.clip = clip.Clip;
        MusicAudioSource.loop = !clip.DoNotLoop;

        switch (musicType)
        {
            case EMusicType.OpenningScreen:
            SetSnapshotMusic();
            break;

            case EMusicType.Gameover:
            SetSnapshotGame();
            break;

            default:
            SetSnapshotGame();
            break;
        }
        MusicAudioSource.PlayDelayed(2f);
    }

    public void PlayDiegeticClip(EDiegeticClipType clipType, AudioSource audioSource)
    {
        List<DiegeticClip> diegeticClips = DiegeticClips.FindAll(c => c.ClipType == clipType);
        int clipIndex = Random.Range(0, diegeticClips.Count);

        DiegeticClip diegeticClip = diegeticClips[clipIndex];
        audioSource.clip = diegeticClip.Clip;
        audioSource.Play();
    }
}