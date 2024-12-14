using UnityEngine;
using UnityEngine.UI;

public class Rune : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    public void setUp(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public RectTransform getRectTransform()
    {
        return rectTransform;
    }

    void Update()
    {
        var pos = rectTransform.localPosition;
        pos.y += 15f * Time.deltaTime;
        if (pos.y > 120f)
        {
            Destroy(gameObject);
        }
        rectTransform.localPosition = pos;

    }
}
