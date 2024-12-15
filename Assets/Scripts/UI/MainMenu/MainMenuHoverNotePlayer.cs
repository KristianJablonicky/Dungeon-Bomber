using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuHoverNotePlayer : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip beat, beatAccented;
    public void OnPointerEnter(PointerEventData eventData)
    {
        var source = MenuMusicPlayer.getAudioSourceNote();
        if (MenuMusicPlayer.getCurrentBeat() == 0)
        {
            source.clip = beatAccented;
        }
        else
        {
            source.clip = beat;
        }
        source.Play();
    }
}
