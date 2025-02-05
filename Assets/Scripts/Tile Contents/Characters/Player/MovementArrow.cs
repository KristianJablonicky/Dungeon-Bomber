using UnityEngine;

public class MovementArrow : MonoBehaviour
{
    [SerializeField] private Movement direction;
    private Player player;
    private void Start()
    {
        player = Dungeon.instance.getPlayer();
    }

    public void onTap()
    {
        player.jump(direction);
    }
}
