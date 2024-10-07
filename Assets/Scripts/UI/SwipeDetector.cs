using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float x, y;
    private Player player;
    public void OnPointerDown(PointerEventData eventData)
    {
        x = eventData.position.x;
        y = eventData.position.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        x = eventData.position.x - x;
        y = eventData.position.y - y;

        // make sure the slide wasn't accidental
        if (Mathf.Abs(x) < 15f && Mathf.Abs(y) < 15f)
        {
            return;
        }

        if (Mathf.Abs(x) >= Mathf.Abs(y))
        {
            if (x >= 0)
            {
                player.jump(Movement.Right);
            }
            else
            {
                player.jump(Movement.Left);
            }
        }
        else
        {
            if (y >= 0)
            {
                player.jump(Movement.Up);
            }
            else
            {
                player.jump(Movement.Down);
            }
        }
    }

    private void Start()
    {
        if (false && !Application.isMobilePlatform)
        {
            Destroy(gameObject);
        }
        else
        {
            Dungeon.instance.playerSpawned += playerSpawned;
        }
    }

    private void playerSpawned(object sender, System.EventArgs e)
    {
        player = (Player)sender;
    }
}
