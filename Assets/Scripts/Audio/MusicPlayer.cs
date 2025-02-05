using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip metronomeAccented, metronomeUnaccented;
    [SerializeField] private List<Song> songs;

    private Song currentSong;

    private int voxEnabled = 0;

    private Metronome metronome;
    private int currentBeat = 0;
    private void Start()
    {
        metronome = Metronome.instance;
        int floor = DataStorage.instance.floor;

        if (floor > songs.Count)
        {
            setUpMetronome();
        }
        else
        {
            setUpSong(songs[floor-1]);
        }

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.25f);
            PlayerPrefs.SetInt("vox", 0);
        }

        setVolume();
    }

    private void setUpMetronome()
    {
        metronome.countInBeat += onBeat;
        metronome.onBeat += onBeat;
    }
    private void setUpSong(Song song)
    {
        currentSong = song;
        metronome.countInBeat += playIntro;
        metronome.onBeat += seeIfIntroNeedsToEnd;
        Dungeon.instance.ladderReached += fadeOut;
        Dungeon.instance.getPlayer().defeated += fadeOut;
    }

    private void fadeOut(object sender, System.EventArgs e)
    {
        if (sender is Player)
        {
            StartCoroutine(fadeOutVolume(0.25f));
        }
        else
        {
            StartCoroutine(fadeOutVolume(0.5f));
        }
    }

    private IEnumerator fadeOutVolume(float finalVolume)
    {
        float formerVolume = audioSource.volume, timeElapsed = 0f, duration = metronome.getBeatLength() * 4f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            audioSource.volume = formerVolume - (1f - finalVolume) * (timeElapsed / duration);
            yield return null;
        }
        audioSource.volume = finalVolume * formerVolume;
    }

    private void onBeat(object sender, System.EventArgs e)
    {
        if (currentBeat == 0)
        {
            audioSource.PlayOneShot(metronomeAccented);
        }
        else
        {
            audioSource.PlayOneShot(metronomeUnaccented);
        }
        currentBeat++;
        currentBeat %= 4;
    }

    private void playIntro(object sender, System.EventArgs e)
    {
        metronome.countInBeat -= playIntro;
        if (voxEnabled == 1)
        {
            audioSource.PlayOneShot(currentSong.introWithVox);
        }
        else
        {
            audioSource.PlayOneShot(currentSong.intro);
        }
    }
    private void seeIfIntroNeedsToEnd(object sender, System.EventArgs e)
    {
        currentBeat++;
        if ((currentBeat - 1) / 4 >= currentSong.introMeasures)
        {
            currentBeat = 0;
            metronome.onBeat -= seeIfIntroNeedsToEnd;
            metronome.onBeat += seeIfSongNeedsToBeLooped;
            if (voxEnabled == 1)
            {
                audioSource.PlayOneShot(currentSong.loopWithVox);
            }
            else
            {
                audioSource.PlayOneShot(currentSong.loop);
            }
        }
    }
    private void seeIfSongNeedsToBeLooped(object sender, System.EventArgs e)
    {
        currentBeat++;
        if (currentBeat / 4 >= currentSong.loopMeasures)
        {
            currentBeat = 0;
            audioSource.PlayOneShot(currentSong.loop);
        }
    }

    public void setVolume()
    {
        AudioListener.volume = 0.5f * PlayerPrefs.GetFloat("musicVolume");
        voxEnabled = PlayerPrefs.GetInt("vox");
    }
}
