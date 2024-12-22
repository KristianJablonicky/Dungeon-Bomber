using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMover : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    Vector2 basePos, horizontalRange, verticalRange;
    private void Awake()
    {
        horizontalRange = new Vector2(-5f, 5f);
        verticalRange = horizontalRange * ((float)Screen.height / Screen.width);
        basePos = rectTransform.anchoredPosition;
    }
    void Update()
    {
        float normalizedMouseX = Input.mousePosition.x / Screen.width;
        float normalizedMouseY = Input.mousePosition.y / Screen.height;
        float targetX = Mathf.Lerp(horizontalRange.x, horizontalRange.y, 1 - normalizedMouseX);
        float targetY = Mathf.Lerp(verticalRange.x, verticalRange.y, 1 - normalizedMouseY);
        Vector3 newPosition = rectTransform.anchoredPosition;
        newPosition.x = basePos.x + targetX;
        newPosition.y = basePos.y + targetY;
        rectTransform.anchoredPosition = newPosition;
    }
}
