using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject popup;

    public void closePopup()
    {
        ButtonHandler.hideFade();
        Destroy(popup);
    }
}
