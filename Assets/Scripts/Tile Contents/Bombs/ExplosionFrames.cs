using UnityEngine;
using System.Collections.Generic;

public class ExplosionFrames : MonoBehaviour
{
    [SerializeField] private List<Sprite> redExplosion, greenExplosion, blueExplosion;

    public static ExplosionFrames instance;
    private void Awake()
    {
        instance = this;
    }

    public List<Sprite> getExplosionList(spiritType type)
    {
        if (type == spiritType.bear)
        {
            return redExplosion;
        }
        else if (type == spiritType.wolf)
        {
            return greenExplosion;
        }
        else if (type == spiritType.owl)
        {
            return blueExplosion;
        }
        return null;
    }
}
