using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    private static Prefabs instance;

    public Explosion explosion;
    public HealthBar healthBar;
    public static Prefabs i
    {
        get
        {
            if (instance == null)
            {
                instance = Assets.i.GetComponent<Prefabs>();
            }
            return instance;
        }
    }
}
