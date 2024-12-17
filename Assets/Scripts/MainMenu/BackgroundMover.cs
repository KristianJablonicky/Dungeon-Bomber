using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 horizontalRange;
    private Vector2 verticalRange;
    private float baseX;

    private void Start()
    {
        float x;
        if (spriteRenderer.sortingOrder == 0)
        {
            x = 0.5f;
        }
        else if (spriteRenderer.sortingOrder == 1)
        {
            x = 1f;
        }
        else
        {
            x = 2f;
        }
        horizontalRange = new Vector2(-x, x);
        verticalRange = horizontalRange * ((float)Screen.height / Screen.width);
        baseX = transform.position.x;
    }

    void Update()
    {
        float normalizedMouseX = Input.mousePosition.x / Screen.width;
        float normalizedMouseY = Input.mousePosition.y / Screen.height;


        float targetX = Mathf.Lerp(horizontalRange.x, horizontalRange.y, 1 - normalizedMouseX);
        float targetY = Mathf.Lerp(verticalRange.x, verticalRange.y, 1 - normalizedMouseY);

        Vector3 newPosition = transform.position;
        newPosition.x = baseX + targetX;
        newPosition.y = targetY;

        transform.position = newPosition;
    }
}