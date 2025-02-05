using UnityEngine;

// https://www.youtube.com/watch?v=VprqsEsFb5w&t=201s

public class SafeArea : MonoBehaviour
{
    RectTransform rect_transform;
    Rect rect_safe_area;
    Vector2 min_anchor;
    Vector2 max_anchor;

    void Awake()
    {
        rect_transform = GetComponent<RectTransform>();
        rect_safe_area = Screen.safeArea;
        min_anchor = rect_safe_area.position;
        max_anchor = min_anchor + rect_safe_area.size;

        min_anchor.x /= Screen.width;
        min_anchor.y /= Screen.height;
        max_anchor.x /= Screen.width;
        max_anchor.y /= Screen.height;

        rect_transform.anchorMin = min_anchor;
        rect_transform.anchorMax = max_anchor;
    }
}
