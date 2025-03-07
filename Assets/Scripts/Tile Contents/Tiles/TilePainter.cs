using UnityEngine;

public class TilePainter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer.color = Dungeon.instance.getFloorColor();
    }
}
