using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    private Vector3 offset;
    private void Start()
    {
        enabled = false;
        Dungeon.instance.playerSpawned += onPlayerSpawn;
        offset = new Vector3(0, 0, -0.5f);
    }

    private void onPlayerSpawn(object sender, System.EventArgs e)
    {
        player = (Player)sender;
        enabled = true;
        player.onDeath += unfollowPlayer;
    }

    private void unfollowPlayer(object sender, System.EventArgs e)
    {
        if (sender is Player)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
