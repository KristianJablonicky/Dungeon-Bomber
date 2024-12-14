using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text title, lyrics;
    List<Lyrics> songs;
    private int index = 0, highestFloor;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("HighestFloorReached"))
        {
            highestFloor = 0;
        }
        highestFloor = PlayerPrefs.GetInt("HighestFloorReached");
        songs = new List<Lyrics> {
                new ("Introduction", "Welcome to Necroblaster: Rhythm of the Dead!\n" +
                "Reach the lowest floor of the catacomb to unveil more of the lore.\n" +
                "Move by either arrows or WASD, switch spells by either JKL or 123\n" +
                "Defeat foes to level up - this increases your vitality " +
                "and allows you to select more upgrades.\n\n" +
                "See if you can purge the crypt of the undead in the lowest amount of time.\n" +
                "Good luck!"),

                new("Cemetary Slams", "As the graveyard\nBasks in twilight\nI became a victim to the urge\n\n" +
                "Fingers defy me\nAs they dance on the neck\nOf my guitar, they play the rhythms of the dead"),

                new ("Vow", "Evil\nSpirits\nBring back the dead\n" +
                "To groove to the rhythms that make them roam the land again\n\n" +
                "They came to kill, destroy and obliterate\nI shall take them down, and end their torment once again"),

                new ("The Cursed Catacomb", "The sounds of their rattling bones\nEscape the lowest floor" +
                "\nOf this cursed catacomb\nWhich is where they spawn\n\n" +
                "Pulverize their bones to dust\nNo foe shall stand in my path" +
                "\nI shall descend below\nTo the lowest depth\n\n" +
                "Floor by floor, descending deeper\nInto the jaws of necromancy"),

                new ("Necroectomy", "Hahahahaha\nOm nom mnam mnam hami papi hami papi\nOni chrumkaju" +
                "\nOm nom mnam mnam hami papi hami papi\nNa ludskom mase")
        };
        updateTexts();
    }

    private void updateTexts()
    {
        title.text = songs[index].title;
        lyrics.text = songs[index].lyrics;
    }

    public void changeSong(bool next)
    {
        if (next &&
            (index+1) < songs.Count &&
            highestFloor > index)
        {
            index++;
            updateTexts();
        }
        else if (!next && index > 0)
        {
            index--;
            updateTexts();
        }
    }

    private class Lyrics
    {
        public string title, lyrics;
        public Lyrics(string title, string lyrics)
        {
            this.title = title;
            this.lyrics = lyrics;
        }
    }

}
