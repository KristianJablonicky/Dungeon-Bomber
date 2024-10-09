using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip metronomeAccented, metronomeUnaccented;
    [SerializeField] private List<Song> songs;

    private Song currentSong;

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
    }

    private void fadeOut(object sender, System.EventArgs e)
    {
        StartCoroutine(fadeOutVolume());
    }

    private IEnumerator fadeOutVolume()
    {
        float formerVolume = audioSource.volume, timeElapsed = 0f, duration = metronome.getBeatLength() * 4f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            audioSource.volume = formerVolume * (1f - timeElapsed / duration * 0.5f);
            yield return null;
        }
        audioSource.volume = 0.5f * formerVolume;
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
        audioSource.PlayOneShot(currentSong.intro);
    }
    private void seeIfIntroNeedsToEnd(object sender, System.EventArgs e)
    {
        currentBeat++;
        if ((currentBeat - 1) / 4 >= currentSong.introMeasures)
        {
            currentBeat = 0;
            metronome.onBeat -= seeIfIntroNeedsToEnd;
            metronome.onBeat += seeIfSongNeedsToBeLooped;
            audioSource.PlayOneShot(currentSong.loop);
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

}
