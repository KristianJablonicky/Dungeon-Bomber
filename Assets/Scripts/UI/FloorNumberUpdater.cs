using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private TMP_Text floorText;
    private void Start()
    {
        floorText.text = $"Floor: {DataStorage.instance.floor}\nBest: {DataStorage.instance.highScore}";
    }
}
