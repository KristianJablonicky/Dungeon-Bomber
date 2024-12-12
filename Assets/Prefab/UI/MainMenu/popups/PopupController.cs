using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject popup;

    public void closePopup()
    {
        Destroy(popup);
    }
}
