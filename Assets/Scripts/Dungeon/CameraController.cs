using System.Collections;
using System.Collections.Generic;
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
        player.positionUpdated += updatePosition;
        player.onDeath += unfollowPlayer;
    }

    private void updatePosition(object sender, System.EventArgs e)
    {
        transform.position = player.transform.position + offset;
    }
    private void unfollowPlayer(object sender, System.EventArgs e)
    {
        player.positionUpdated -= updatePosition;
    }
}
