using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    private Vector3 offset;
    private void Start()
    {
        Dungeon.instance.playerSpawned += onPlayerSpawn;
        offset = new Vector3(0, 0, -0.5f);
    }

    private void onPlayerSpawn(object sender, System.EventArgs e)
    {
        player = (Player)sender;
        enabled = true;
        player.defeated += onPlayerDeath;
    }

    private void onPlayerDeath(object sender, System.EventArgs e)
    {
        enabled = false;
    }

    private void LateUpdate()
    {
        if (!player)
        {
            return;
        }
        transform.position = player.transform.position + offset;
    }
}
