using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuHoverNotePlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioClip beat, beatAccented;
    [SerializeField] private TMP_Text buttonText;
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

        buttonText.color = new Color(0.13f, 0.18f, 0.21f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = new Color(0.56f, 0.97f, 0.89f);
    }
}
