using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    private AudioSource theme;
    private AudioSource sfx;

    [Header("Voice Clips")]
    public AudioClip bullshit;
    public AudioClip destroy;
    public AudioClip destroyThem;
    public AudioClip donLetThemHitYou;
    public AudioClip finishThem;
    public AudioClip gameOver;
    public AudioClip killThemAll;
    public AudioClip killThemBeforeTheyHitYou;
    public AudioClip leaveNobodyAlive;
    public AudioClip letTheBodiesHitTheFloor;
    public AudioClip massacre;
    public AudioClip noMercy;
    public AudioClip rampage;
    public AudioClip tearThisTownApart;

    private AudioClip[] voiceClips;

    private void Awake()
    {
        instance = this;

        AudioSource[] sources = GetComponents<AudioSource>();

        theme = sources[0];
        sfx = sources[1];

        voiceClips = new AudioClip[]
        {
            bullshit,
            destroy,
            destroyThem,
            donLetThemHitYou,
            finishThem,
            gameOver,
            killThemAll,
            killThemBeforeTheyHitYou,
            leaveNobodyAlive,
            letTheBodiesHitTheFloor,
            massacre,
            noMercy,
            rampage,
            tearThisTownApart
        };
    }

    public static void PlayVoiceClip(int clip)
    {
        instance.sfx.PlayOneShot(instance.voiceClips[clip]);
    }
}
