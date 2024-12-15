using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal, cursorClicked;
    private Vector2 hotspot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorClicked, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorNormal, hotspot, CursorMode.Auto);
        }
    }
}