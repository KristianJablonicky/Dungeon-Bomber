using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritButtonsManager : MonoBehaviour
{
    [SerializeField] private SpiritButton button;
    private void Awake()
    {
        instantiate(spiritType.bear);
        instantiate(spiritType.wolf);
        instantiate(spiritType.owl);
    }
    private void instantiate(spiritType type)
    {
        var spiritButton = Instantiate(button, transform);
        spiritButton.transform.SetParent(transform);
        spiritButton.setUp(type);
    }
}
