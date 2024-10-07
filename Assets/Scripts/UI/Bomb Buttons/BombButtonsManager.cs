using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombButtonsManager : MonoBehaviour
{
    [SerializeField] private BombButton button;
    private void Awake()
    {
        instantiate(bombTypes.square);
        instantiate(bombTypes.plus);
        instantiate(bombTypes.x);
    }
    private void instantiate(bombTypes type)
    {
        var bombButton = Instantiate(button, transform);
        bombButton.transform.SetParent(transform);
        bombButton.setUp(type);
    }
}
