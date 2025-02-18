using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementPad : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private CanvasGroup[] highlights;
    private Vector2[][] triangles;
    private Player player;

    private void Awake()
    {
        triangles = new Vector2[4][];

        Vector2[] corners = new Vector2[]
        {
            new (1, 1),   // Top-right
            new (1, -1),  // Bottom-right
            new (-1, -1), // Bottom-left
            new (-1, 1)   // Top-left
        };

        // Create 4 triangles dynamically
        for (int i = 0; i < 4; i++)
        {
            triangles[i] = new Vector2[]
            {
                corners[i],              // Current corner
                corners[(i + 1) % 4],    // Next corner (wraps around)
                new (0, 0)               // Center point
            };
        }
    }
    private void Start()
    {
        player = Dungeon.instance.getPlayer();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            buttonRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        Vector2 normalizedPoint = new Vector2(
            (localPoint.x / (buttonRect.rect.width * 0.5f)),
            (localPoint.y / (buttonRect.rect.height * 0.5f))
        );

        Vector2[] triangle;
        for (int buttonIndex = 0; buttonIndex < 4; buttonIndex++)
        {
            triangle = triangles[buttonIndex];
            if (isPointInTriangle(normalizedPoint, triangle[0], triangle[1], triangle[2]))
            {
                player.jump((Movement)buttonIndex);
                StartCoroutine(flashHighlight(buttonIndex));
                break;
            }

        }
    }
    bool isPointInTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
    {
        float sign1 = (P.x - A.x) * (B.y - A.y) - (B.x - A.x) * (P.y - A.y);
        float sign2 = (P.x - B.x) * (C.y - B.y) - (C.x - B.x) * (P.y - B.y);
        float sign3 = (P.x - C.x) * (A.y - C.y) - (A.x - C.x) * (P.y - C.y);

        bool hasNeg = (sign1 < 0) || (sign2 < 0) || (sign3 < 0);
        bool hasPos = (sign1 > 0) || (sign2 > 0) || (sign3 > 0);

        return !(hasNeg && hasPos);
    }

    private IEnumerator flashHighlight(int index)
    {
        highlights[index].alpha = 1.0f;
        yield return new WaitForSeconds(0.2f);
        highlights[index].alpha = 0f;
    }
}
