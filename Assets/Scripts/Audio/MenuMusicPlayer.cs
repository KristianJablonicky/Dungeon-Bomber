using UnityEngine;

public class MenuMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource, audioSourceSingleNote;
    private static MenuMusicPlayer instance;
    private int currentBeat;
    void Awake()
    {
        instance = this;
        audioSourceSingleNote.volume = PlayerPrefs.GetFloat("musicVolume");
    }
    
    public static AudioSource getAudioSourceSong()
    {
        return instance.audioSource;
    }

    public static AudioSource getAudioSourceNote()
    {
        return instance.audioSourceSingleNote;
    }

    public static int getCurrentBeat()
    {
        var currentBeat = instance.currentBeat;
        instance.currentBeat = (instance.currentBeat + 1) % 4;
        return currentBeat;
    }
}
