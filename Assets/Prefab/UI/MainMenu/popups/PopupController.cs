using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    public static PopupController instance;

    private void Awake()
    {
        instance = this;
    }
    public void closePopup()
    {
        ButtonHandler.hideFade();
        Destroy(popup);
    }
}
