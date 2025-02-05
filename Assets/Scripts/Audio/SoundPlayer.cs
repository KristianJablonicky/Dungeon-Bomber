using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> playerHurtSounds;
    [SerializeField] private List<AudioClip> undeadDeathSounds;
    [SerializeField] private AudioClip chessPieceDeathSound;
    [SerializeField] private List<AudioClip> spiritAnimalSounds;

    private static SoundPlayer instance;
    public static SoundPlayer getInstance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;
        source.volume = PlayerPrefs.GetFloat("musicVolume", 0f);
    }

    private AudioClip getRandom(List<AudioClip> clips)
    {
        return clips[Random.Range(0, clips.Count)];
    }

    public void playHurtSound()
    {
        source.PlayOneShot(getRandom(playerHurtSounds));
    }

    public void playUndeadDeathSound()
    {
        source.PlayOneShot(getRandom(undeadDeathSounds));
    }

    public void playChessPieceDeathSound()
    {
        source.PlayOneShot(chessPieceDeathSound);
    }

    public void playSpiritAnimalSound(spiritType type)
    {
        source.PlayOneShot(spiritAnimalSounds[(int)type]);
    }

}
