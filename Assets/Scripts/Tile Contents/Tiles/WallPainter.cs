using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer.material.color = Dungeon.instance.getFloorColor();
    }
}
