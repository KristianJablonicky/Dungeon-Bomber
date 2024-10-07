using UnityEngine;

public class ResetButton : MonoBehaviour
{
    void Awake()
    {
        if (false && !Application.isMobilePlatform)
        {
            Destroy(gameObject);
        }
    }

    public void reset()
    {
        Dungeon.instance.reset();
    }
}
